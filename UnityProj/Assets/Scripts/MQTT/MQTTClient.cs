using CqCore;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// MQTT封装客户端
/// </summary>
public class MQTTClient:IDisposable
{
    IMqttClient mqttClient;

    public bool IsConnected
    {
        get
        {
            return mqttClient != null && mqttClient.IsConnected;
        }
    }

    public string ip { get;private set; }
    public int port { get; private set; }
    static MqttFactory Factory;

    /// <summary>
    /// 收发的订阅格式
    /// </summary>
    public string topicFormat { get; private set; }

    /// <summary>
    /// 订阅列表
    /// </summary>
    public List<string> TopicList { get; private set; }

    public MQTTClient(string appTopicPath)
    {
        if(Factory==null)
        {
            Factory = new MqttFactory();
        }
        TopicList = new List<string>();
        topicFormat = string.Format("{0}/{1}({2})", appTopicPath, SystemInfo.deviceName,
            Application.isMobilePlatform ? SystemInfo.deviceModel : "pc");
        mqttClient = Factory.CreateMqttClient();

        mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived1;

        mqttClient.Connected += MqttClient_Connected;
        mqttClient.Disconnected += MqttClient_Disconnected;
    }

    public void Dispose()
    {
        if(mqttClient!=null)
        {
            Disconnect();
            mqttClient.ApplicationMessageReceived -= MqttClient_ApplicationMessageReceived1;

            mqttClient.Connected -= MqttClient_Connected;
            mqttClient.Disconnected -= MqttClient_Disconnected;
            
            mqttClient = null;
        }
    }
    private void MqttClient_ApplicationMessageReceived1(object sender, MqttApplicationMessageReceivedEventArgs e)
    {
        if (OnReceived != null)
        {
            try
            {
                var msgData = Torsion.Deserialize(StringCompress.CheckUnCompress(e.ApplicationMessage.Payload));
                var _topic = e.ApplicationMessage.Topic;
                GlobalCoroutine.Call(() => OnReceived(_topic, msgData));
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }

    public event Action<string, object> OnReceived;

    /// <summary>
    /// 用于发送过程中阻止又产生的嵌套发送调用
    /// </summary>
    bool sendLoging = false;

    MqttApplicationMessage mam=new MqttApplicationMessage();
    public bool SendObject(object obj, string topic=null)
    {
        if (IsConnected)
        {
            if (sendLoging) return false;
            sendLoging = true;
            
            //不阻塞主线程,在非主线程中序列化和压缩并发送.
            ThreadUtil.PoolCall(() =>
            {
                lock(mam)
                {
                    mam.Topic = (topic == null ? topicFormat : topic) + "/" + AssemblyUtil.GetName(obj.GetType());
                    var content = Torsion.Serialize(obj, true, true);
                    mam.Payload = StringCompress.CheckCompress(content);
                    //Debug.Log("原始大小:" + content.Length);
                    //Debug.Log("压缩比:" + content.Length * 1f / mam.Payload.Length);
                    mqttClient.PublishAsync(mam);
                }
            });
            
            sendLoging = false;
            return true;
        }
        return false;
    }

    public void Disconnect()
    {
        if(IsConnected)
        {
            mqttClient.DisconnectAsync();
        }
    }
    /// <summary>
    /// 连接到服务器<para/>
    /// 默认订阅defaultTopic
    /// </summary>
    public void Connect(string ip= MQTTProto.MQTTConst.ip, int port= MQTTProto.MQTTConst.port)
    {
        this.ip = ip;
        this.port = port;
        var options = new MqttClientOptionsBuilder()
        .WithClientId(SystemInfo.deviceUniqueIdentifier+"_"+Application.identifier+"_"+RandomUtil.Random(0,10000))
        .WithTcpServer(ip, port)              //onenet ip：183.230.40.39    port:6002
                                              //.WithCredentials(UserName, pwd)      //username为产品id       密码为鉴权信息或者APIkey
                                              //.WithTls()//服务器端没有启用加密协议，这里用tls的会提示协议异常
        .WithCleanSession(false)
        .WithKeepAlivePeriod(TimeSpan.FromSeconds(5000))
        .WithCommunicationTimeout(TimeSpan.FromSeconds(5))
        .Build();
        mqttClient.ConnectAsync(options);
    }
    public event Action OnConnected;
    public event Action OnDisconnected;
    private void MqttClient_Disconnected(object sender, MqttClientDisconnectedEventArgs e)
    {
        if (OnDisconnected != null)
        {
            GlobalCoroutine.Call(() =>
            {
                if (OnDisconnected != null)
                {
                    OnDisconnected();
                    OnDisconnected = null;
                }
            });
        }
    }

    private void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
    {
        if (OnConnected != null)
        {
            GlobalCoroutine.Call(() =>
            {
                if(OnConnected!=null)
                {
                    OnConnected();
                    OnConnected = null;
                }
            });
        }
    }

    /// <summary>
    /// 在自己的路径下,订阅特定类型消息
    /// </summary>
    public bool Subscribe(Type type)
    {
        return Subscribe(string.Format("{0}/{1}", topicFormat, AssemblyUtil.GetName(type)));
    }
    /// <summary>
    /// 在自己的路径下,订阅特定类型消息
    /// </summary>
    public bool Subscribe<T>()
    {
        return Subscribe(typeof(T));
    }

    /// <summary>
    /// 订阅消息<para/>
    /// 默认订阅路径下所有消息类型
    /// </summary>
    public bool Subscribe(string topic=null)
    {
        if (IsConnected)
        {
            if (topic == null) topic = topicFormat + "/#";
            //Debug.Log("订阅" + topic);
            mqttClient.SubscribeAsync(new TopicFilter(topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce));
            TopicList.Add(topic);
            return true;
        }
        else
        {
            //Debug.Log("订阅失败" + topic);
            return false;
        }
    }
    /// <summary>
    /// 取消订阅
    /// </summary>
    public bool UnSubscribeAsync(string topic)
    {
        if (IsConnected)
        {
            //Debug.Log("取消订阅" + topic);
            mqttClient.UnsubscribeAsync(topic);
            TopicList.Remove(topic);
            return true;
        }
        return false;
    }

    
}
