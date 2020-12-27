using MVL;
using System.Collections;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂在同Button下面,按钮点击执行事件通知
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonNotify : MonoBehaviourExtended
{
    LinkMethod lm;
    DlgMono dm;
    void Awake()
    {
        dm = GetComponentInParent<DlgMono>();
        lm = GetComponent<LinkMethod>();
        if(lm!=null && dm!=null)
        {
            var btn = GetComponent<Button>();
            btn.onClick.SetCallBack(OnButtonClick,DestroyHandle);
        }
    }
    void OnButtonClick()
    {
        EventMgr.ButtonClick.Notify(dm.name, lm.FullPath, this);
    }
}