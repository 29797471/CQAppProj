using UnityCore;
using UnityEngine;

/// <summary>
/// 先于Main启动在Awake执行
/// </summary>
public class ReportState : MonoBehaviourExtended
{
    private void Awake()
    {
        Debug.developerConsoleVisible = false;
        /*
         var cc = GetComponent<MQTTGame>();
        if(cc!=null)
        {
            EventMgr.WindowShow.CallBack((sender, data) =>
            {
                cc.state = data.win;
            }, DestroyHandle);
            cc.DoCommand = LuaMgr.instance.DoCommand;
        }*/
    }
}
