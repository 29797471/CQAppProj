using UnityCore;
using UnityEngine;
using UnityEngine.Events;

public class DlgMono : MonoBehaviourExtended
{
    public Dlg dlg;

    public void Open(object parameter=null)
    {
        dlg.Open(parameter);
    }

    public void Close()
    {
        dlg.Close();
    }

    [Header("显示回调")]
    public UnityEvent OnShow;
    [Header("隐藏回调")]
    public UnityEvent OnHide;

}
