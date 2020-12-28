using MQTTProto;
using System;
using System.Collections.Generic;
using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 指令端<para/>
/// 默认订阅格式(公司/应用名称/设备名称/消息类型)
/// </summary>
public class MQTTCommandWindow : EditorWindow
{
    GUISkin skin;
    const string winName = "指令端";

    //[MenuItem("Assets/" + winName)]
    [MenuItem("工具/"+ winName)]
    static void Open()
    {
        GetWindow<MQTTCommandWindow>(winName);
    }

    string dstClient;
    HashSet<string> gameList = new HashSet<string>();

    MQTTClient client;

    string heartTopicFormat;
    string logTopicFormat;
    string commandTopicFormat;
    string Pattern;

    
    private void OnEnable()
    {
        MQTTConst.RegType();

        heartTopicFormat = string.Format("Game/{0}/+/+/{1}", Application.companyName,  AssemblyUtil.GetName(typeof(Heart_Game)));

        commandTopicFormat= string.Format("Game/{0}/{{0}}", Application.companyName);
        logTopicFormat = commandTopicFormat+"/"+ AssemblyUtil.GetName(typeof(LogMsg_Game));
        Pattern = heartTopicFormat.Replace("+/+", "(.+)");
        client = new MQTTClient("Command/");
        client.Connect(MQTTConst.ip, MQTTConst.port);

        client.OnConnected += () =>
        {
            Debug.Log("连接MQTT服务器成功");
            client.Subscribe(heartTopicFormat);

            client.Subscribe();
        };
        client.OnReceived += Client_OnReceived;
        client.OnDisconnected += () =>
        {
            Debug.Log("和MQTT服务器断开连接");
            client.Connect();
        };
    }


    private void OnDisable()
    {
        client.Dispose();
        client = null;
        Debug.Log("主动和MQTT服务器断开连接");
    }
    private void Client_OnReceived(string _topic, object msg)
    {
        if(msg is Heart_Game)
        {
            OnReceive(_topic,(Heart_Game)msg);
        }
        else if(msg is LogMsg_Game)
        {
            //防止指令端抓取本地客户端上传的打印,导致死循环打印
            if (!Application.isPlaying)OnReceive(_topic,(LogMsg_Game)msg);
        }
        else if(msg is HierarchyMsg_Game)
        {
            OnReceive(_topic, (HierarchyMsg_Game)msg);
        }
        else if (msg is ProfilerMsg_Game)
        {
            OnReceive(_topic, (ProfilerMsg_Game)msg);
        }
        //Debug.Log(target + "\n" + Torsion.Serialize(msg));
    }
    void OnReceive(string _topic, ProfilerMsg_Game msg)
    {
        var tree = Torsion.Deserialize<ProfilerMsg>(msg.data);
        var obj = new GameObject(string.Format("{0}({1})", msg.state,  TimeUtil.GetTimeStringByDate(DateTime.Now)));
        var mono = obj.AddComponent<ProfilerMsgMono>();
        mono.msg = tree;
        mono.data = msg.data;
        mono.time = DateTime.Now.Ticks;
        mono.MakeMsgTree();
    }
    void OnReceive(string _topic, HierarchyMsg_Game msg)
    {
        var tree = Torsion.Deserialize<CqCore.TreeNode<SerGameObject>>(msg.data);
        var obj = new GameObject(string.Format("{0}({1})", msg.state,  TimeUtil.GetTimeStringByDate(DateTime.Now)));
        //生成Hierarchy;
        var child = (GameObject)tree.Data;
        child.transform.SetParent(obj.transform);
        Selection.activeObject = obj;
    }

    void OnReceive(string _topic,Heart_Game msg)
    {
        var group = CqCore.RegexUtil.Matches(_topic, Pattern);
        var target = group[0].Groups[1].Value;
        gameList.Add(target);
    }
    void OnReceive(string _topic, LogMsg_Game msg)
    {
        for (LogType i = 0; i <= LogType.Exception; i++)
        {
            PlayerSettings.SetStackTraceLogType(i, StackTraceLogType.None);
        }
        Debug.unityLogger.Log(msg.type, msg.condition + "\n" + msg.stackTrace);
        for (LogType i = 0; i <= LogType.Exception; i++)
        {
            PlayerSettings.SetStackTraceLogType(i, StackTraceLogType.ScriptOnly);
        }
    }

    bool showTopicList;

    void OnGUI()
    {
        var data = ConsoleConfig.Inst;
        if (skin == null)
        {
            skin = GUI.skin;
        }
        BeginWindows();

        GUILayout.Label(client.IsConnected ? string.Format("已连接MQTT服务器{0}:{1}", client.ip,client.port) : "未连接MQTT服务器");

        showTopicList=GUILayout.Toggle(showTopicList, "查看订阅列表");
        if(showTopicList)
        {
            foreach (var it in client.TopicList)
            {
                GUILayout.Label(it);
            }
        }
        GUILayout.BeginVertical( skin.box);
        
        {
            
            {
                scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.ExpandHeight(true), GUILayout.Height(200));
                foreach (var game in gameList)
                {
                    var it = game;
                    var bl = (it == dstClient);
                    GUILayout.BeginVertical();
                    //GUILayoutUtil.Vertical(() =>
                    {
                        GUILayout.Label(it);
                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(new GUIContent("对象层级快照", "Hierarchy"), GUILayout.Width(100)))
                            {
                                client.SendObject(new Order_Command()
                                {
                                    opr=3,
                                    topic=client.topicFormat
                                }, string.Format(commandTopicFormat,it));
                            }
                            if (GUILayout.Button(new GUIContent("内存快照", "Memory")))
                            {
                                client.SendObject(new Order_Command()
                                {
                                    opr = 2,
                                    topic = client.topicFormat
                                }, string.Format(commandTopicFormat, it));
                            }
                            var _bl = GUILayout.Toggle(bl, "日志");
                            if (_bl != bl)
                            {
                                if(dstClient!=null)
                                {
                                    //取消订阅
                                    client.UnSubscribeAsync(string.Format(logTopicFormat, dstClient));
                                }
                                if (_bl)
                                {
                                    dstClient = it;
                                    //订阅
                                    client.Subscribe(string.Format(logTopicFormat, dstClient));
                                }
                                else
                                {
                                    dstClient = null;
                                }
                            }
                        };
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
                if (dstClient == null) return;
                GUILayout.BeginHorizontal();
                
                {
                    EditorGUILayout.LabelField("命令(发送:shift+Enter,换行:Enter)");

                    if (GUILayout.Button("代码模版"))
                    {
                        CodeTemplateWindow.Inst.Open((str)=>
                        {
                            if(!str.IsNullOrEmpty())
                            {
                                InputText = str;
                            }
                        });
                    }
                };
                GUILayout.EndHorizontal();
                
                {
                    var temp = EditorGUILayout.TextArea(inputText, GUILayout.ExpandHeight(true));
                    //var temp = EditorGUILayout.TextArea(inputText, GUILayout.ExpandHeight(true));
                    if (temp != inputText)
                    {
                        inputText = temp;
                    }
                    if (Event.current.isKey && Event.current.type == EventType.KeyUp)
                    {
                        if(Event.current.keyCode==KeyCode.UpArrow)
                        {
                            if(historyCommandIndex>0) historyCommandIndex--;
                            InputText = historyCommand[historyCommandIndex];
                        }
                        else if (Event.current.keyCode == KeyCode.DownArrow)
                        {
                            if(historyCommandIndex< historyCommand.Count-1) historyCommandIndex++;
                            InputText = historyCommand[historyCommandIndex];
                        }
                        else if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter && Event.current.modifiers == EventModifiers.Shift)
                        {
                            if (!inputText.IsNullOrEmpty())
                            {
                                DoCommand(inputText);
                                inputText = null;
                            }
                        }

                    }
                }

                //DoInput();
            }
        }
        GUILayout.EndVertical();

        EndWindows();
    }

    List<string> historyCommand = new List<string>();
    int historyCommandIndex = 0;
    public void DoCommand(string command)
    {
        if (command.IsNullOrEmpty()) return;
        
        if (client != null && client.IsConnected && dstClient != null)
        {
            client.SendObject(new Order_Command() { opr = 1, command = command }, string.Format(commandTopicFormat, dstClient));
            historyCommand.Add(command);
            historyCommandIndex = historyCommand.Count-1;
        }
    }

    Vector2 scroll;
    string inputText;
    string InputText
    {
        set
        {
            inputText = value;
            EditorGUIUtility.editingTextField = false;
            Repaint();
        }
    }

    /// <summary>
    /// 当窗口获得焦点时调用一次
    /// </summary>
    void OnFocus()
    {
    }
    /// <summary>
    /// 当窗口丢失焦点时调用一次
    /// </summary>
    void OnLostFocus()
    {
    }
    /// <summary>
    /// 当Hierarchy视图中的任何对象发生改变时调用一次
    /// </summary>
    void OnHierarchyChange()
    {
    }
    /// <summary>
    /// 当Project视图中的资源发生改变时调用一次
    /// </summary>
    void OnProjectChange()
    {
    }
    /// <summary>
    /// 窗口面板的更新
    /// 这里开启窗口的重绘，不然窗口信息不会刷新
    /// </summary>
    void OnInspectorUpdate()
    {
        Repaint();
    }
    /// <summary>
    /// 当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
    /// 有可能是多选，
    /// </summary>
    void OnSelectionChange()
    {
        //这里开启一个循环打印选中游戏对象的名称
        //foreach (Transform t in Selection.transforms)
        //{
        //    //Debug.Log("OnSelectionChange" + t.name);
        //}
    }
    /// <summary>
    /// 当窗口关闭时调用
    /// </summary>
    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
    }
}

