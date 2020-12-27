using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DlgMgr : SystemMgr<DlgMgr>
{
    
    Dictionary<string, Dlg> mDlgDic;

    /// <summary>
    /// 加载过的窗口列表
    /// </summary>
    Dictionary<string, Dlg> DlgDic
    {
        get
        {
            if (mDlgDic == null)
            {
                mDlgDic = new Dictionary<string, Dlg>();
            }
            return mDlgDic;
        }
    }

    /// <summary>
    /// 当前显示的窗口列表
    /// </summary>
    public HashSet<string> VisibleList { get; private set; }

    public void Init()
    {
        VisibleList = new HashSet<string>();
        EventMgr.WindowShow.CallBack((sender, data) => VisibleList.Add(data.win), GlobalMgr.instance.ExitHandle);
        EventMgr.WindowHide.CallBack((sender, data) => VisibleList.Remove(data.win), GlobalMgr.instance.ExitHandle);
    }
    
    public override void Dispose()
    {
        if (mDlgDic != null)
        {
            var list = mDlgDic.ToList();
            foreach (var it in list)
            {
                it.Value.Destroy();
            }
            mDlgDic = null;
        }
        base.Dispose();
    }
    
    public Dlg GetDlg(string name)
    {
        Dlg dlg=null;
        if (DlgDic.TryGetValue(name, out dlg))
        {
            return dlg;
        }
        Config_Win.DataItem data;
        Config_Win.Dic.TryGetValue(name, out data);
        if (data == null)
        {
            Debug.LogError(string.Format("配置表中找不到这个窗口({0})", name));
        }
        else
        {
            dlg = new Dlg(data);
            DlgDic[name] = dlg;
        }
        return dlg;
    }

    /// <summary>
    /// 加载完后打开窗口
    /// 1.有时立即打开
    /// 2.没有时,先加载资源,后打开
    /// 返回终止回调
    /// </summary>
    public void Open(string name, object parameter = null)
    {
        var dlg = GetDlg(name);
        if(dlg!=null)
        {
            dlg.Open(parameter);
        }
    }

    /// <summary>
    /// 隐藏所有显示的窗口
    /// </summary>
    public void HideAll(ICancelHandle cancelHandle)
    {
        var winNames = VisibleList.ToArray();
        foreach(var winName in winNames)
        {
            var dlg = GetDlg(winName);
            dlg.Hide();
            cancelHandle.CancelAct += () => dlg.Show();
        }
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void Close(string name)
    {
        Dlg dlg;
        if (DlgDic.TryGetValue(name, out dlg))
        {
            dlg.Close();
        }
        else
        {
            Debug.LogError("没有这个窗口" + name);
        }
    }
    public bool Remove(string name)
    {
        return DlgDic.Remove(name);
    }
}
