using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityCore;
using UnityEngine;
using UnityEngine.UI;

public enum CommonBtnStyle
{
    /// <summary>
    /// 返回到对应窗口的打开之前
    /// </summary>
    [EnumLabel("返回")]
    Return,

    /// <summary>
    /// 循环返回到对应的窗口
    /// </summary>
    [EnumLabel("关闭")]
    Close,
}

/// <summary>
/// 挂在同Button下面,按钮点击执行返回
/// </summary>.0.
[RequireComponent(typeof(Button))]
public class CommonBtn : MonoBehaviourExtended
{
    [ComBox("通用按钮", ComBoxStyle.RadioBox)]
    public CommonBtnStyle style;
    DlgMono dlgMono;

    [CheckBox("隐藏所有窗口"),ToolTip("隐藏当前所有显示的窗口,返回时要显示回来")]
    public bool hideAll;

    /// <summary>
    /// 返回时需要关闭的窗口列表
    /// </summary>
    public HashSet<Dlg> returnCloseWins;

    CancelHandle returnHandle;
    void Awake()
    {
        dlgMono = GetComponentInParent<DlgMono>();

        if (hideAll)
        {
            returnHandle = new CancelHandle();
            DlgMgr.Inst.HideAll(returnHandle);
        }

        var btn = GetComponent<Button>();

        if (dlgMono != null)
        {
            switch (style)
            {
                case CommonBtnStyle.Return:
                    {
                        returnCloseWins = new HashSet<Dlg>();
                        EventMgr.WindowShow.CallBack((sender,data) =>
                        {
                            var dlg = DlgMgr.Inst.GetDlg(data.win);
                            if(!dlg.IsAlone)
                            {
                                returnCloseWins.Add(dlg);
                            }
                        },DestroyHandle);

                        EventMgr.WindowHide.CallBack((sender, data) =>
                        {
                            var dlg = DlgMgr.Inst.GetDlg(data.win);
                            if (!dlg.IsAlone)
                            {
                                returnCloseWins.Remove(dlg);
                            }
                        }, DestroyHandle);

                        btn.onClick.SetCallBack(Return, DestroyHandle);
                    }
                    break;
                case CommonBtnStyle.Close:
                    {
                    }
                    break;
            }
        }
    }
    void Return()
    {
        var list = returnCloseWins.ToList();
        foreach (var win in list)
        {
            win.Close();
        }
        if (returnHandle != null)
        {
            returnHandle.CancelAll();
        }
    }
}
