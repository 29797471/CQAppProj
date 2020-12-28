using MQTTProto;
using System.Collections;
using UnityCore;
using UnityEngine;

/// <summary>
/// MQTT游戏端
/// 1.上传日志
/// 2.接收命令
/// </summary>
public class MQTTGame : MonoBehaviourExtended
{
    [TextBox("心跳间隔")]
    public float heartLoopDelta=1f;
    MQTTClient client;

    [TextBox("游戏状态:"),IsEnabled(false)]
    public string state;
    
    void Awake()
    {
        MQTTConst.RegType();
        //topic = string.Format("{0}/{1}", Application.cloudProjectId, Application.identifier);
        
        client = new MQTTClient(string.Format("Game/{0}/{1}",Application.companyName,Application.productName));

        client.Connect();
        client.OnConnected += () =>
        {
            Debug.Log("连接MQTT服务器成功");
            client.Subscribe<Order_Command>();
        };
        client.OnReceived += Client_OnReceived;
        client.OnDisconnected += () =>
        {
            Debug.Log("和MQTT服务器断开连接");
            client.Connect();
        };
        ApplicationUtil.logMessageCallBack(Application_logMessageReceived, DestroyHandle);

        DestroyHandle.CancelAct += () =>
        {
            client.Dispose();
            client = null;
        };
        StartCoroutine(LoopHeart());
        
    }

    IEnumerator LoopHeart()
    {
        Heart_Game x = new Heart_Game();
        
        while (true)
        {
            yield return Sleep(heartLoopDelta);
            x.Unix_timestamp_long = TimeUtil.Unix_timestamp_long;
            client.SendObject(x);
        }
    }

    private void Client_OnReceived(string topic, object msg)
    {
        if(msg is Order_Command)
        {
            OnReceived((Order_Command)msg);
        }
    }

    public System.Func<string, object> DoCommand;
    void OnReceived(Order_Command msg)
    {
        switch(msg.opr)
        {
            case 1:
                {
                    if (DoCommand != null)
                    {
                        var result = DoCommand(msg.command);
                        if(result!=null) Debug.Log("执行结果:"+result);
                    }
                    break;
                }
            case 2:
                {
                    var x = new ProfilerMsg_Game()
                    {
                        data = CqSerialize.Serialize(CqProfiler.MakeProfilerMsg()),
                        state = state,
                    };

                    client.SendObject(x, msg.topic);
                    Debug.Log("上传内存快照");
                    break;
                }
            case 3:
                {
                    var x = new HierarchyMsg_Game()
                    {
                        data = CqSerialize.Serialize(CqProfiler.SaveHierarchyData()),
                        state = state,
                    };
                    client.SendObject(x, msg.topic);
                    Debug.Log("上传Hierarchy");
                    break;
                }
        }
    }

    
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        var lmg = new LogMsg_Game();
        lmg.condition = condition;
        lmg.stackTrace = stackTrace;
        lmg.type = type;
        client.SendObject(lmg);
    }
    
}
