using UnityCore;
using UnityEngine;

/// <summary>
/// 当有错误时直接出现在GUI上
/// </summary>
public class DisplayErrorLog : MonoBehaviourExtended
{
    public GUISkin skin;

    [Button("清屏"),Click("Clear")]
    public string _1;
    
    void Awake()
    {
        ApplicationUtil.logMessageCallBack(HandleLog, DestroyHandle);
        enabled = false;
    }
    private string m_logs;
    /// <summary>
    /// 
    /// /// </summary>    
    /// /// <param name="logString">错误信息</param>    /// 
    /// <param name="stackTrace">跟踪堆栈</param>    /// 
    /// <param name="type">错误类型</param>    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if(type == LogType.Error || type == LogType.Exception)
        {
            m_logs += "\n" + logString + "\n" + stackTrace;
        }
        enabled = !m_logs.IsNullOrEmpty();
    }
    private Vector2 m_scroll;
    internal void OnGUI()
    {
        if (m_logs.IsNullOrEmpty())
            return;
        
        if (GUILayout.Button("清屏",skin.button))
        {
            m_logs = "";
        }
        m_scroll = GUILayout.BeginScrollView(m_scroll,skin.scrollView);
        GUILayout.Label(m_logs, skin.label);
        GUILayout.EndScrollView();
    }
    public void Clear()
    {
        m_logs = "";
    }
}
