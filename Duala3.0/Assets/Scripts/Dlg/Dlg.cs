using CqCore;
using MVL;
using SLua;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对话框数据层
/// </summary> 
public class Dlg
{
    Config_Win.DataItem data;

    public string name
    {
        get
        {
            return data.winName;
        }
    }

    /// <summary>
    /// 独立窗口<para/>
    /// 自己管理打开关闭,不会因为点击返回,关闭等按钮,而打开关闭
    /// </summary>
    public bool IsAlone { get => data.isAlone; }

    LuaTable luaWin;

    GameObject mObj;
    DlgMono dm;
    public GameObject Obj
    {
        get
        {
            return mObj;
        }
    }
    /// <summary>
    /// 在资源加载中时不为null
    /// </summary>
    CancelHandle loadingHandle;

    /// <summary>
    /// 窗口隐藏的回调
    /// </summary>
    CancelHandle mHideHandle;
    public ICancelHandle HideHandle
    {
        get
        {
            if (mHideHandle == null)
            {
                mHideHandle = new CancelHandle();
            }
            return mHideHandle;
        }
    }

    /// <summary>
    /// 窗口销毁的回调
    /// </summary>
    CancelHandle mDestroyHandle;
    public ICancelHandle DestroyHandle
    {
        get
        {
            if (mDestroyHandle == null)
            {
                mDestroyHandle = new CancelHandle();
            }
            return mDestroyHandle;
        }
    }

    public Dlg(Config_Win.DataItem data)
    {
        this.data = data;
    }

    IEnumerator Create()
    {
        var group = DlgLayerMgr.Inst.GetLayer(data.layer);
        loadingHandle = new CancelHandle();
        //var a= Time.frameCount;
        var returnObj = new AsyncReturn<GameObject>();
        yield return ResMgr.Instantiate_IT("Windows/" + data.prefab, returnObj,data.winName, group.Obj.transform, loadingHandle);
        var instWinObj = returnObj.data;
        loadingHandle = null;
        //Debug.Log("加载帧" + (Time.frameCount-a));
        instWinObj.SetActive(false);

        dm = instWinObj.GetComponent<DlgMono>();
        if (dm == null)
        {
            dm = instWinObj.AddComponent<DlgMono>();
        }
        {
            //添加点击事件通知
            var btns = instWinObj.GetComponentsInChildren<Button>();
            foreach (var btn in btns)
            {
                if (btn.gameObject.GetComponent<ButtonNotify>() == null)
                {
                    btn.gameObject.AddComponent<ButtonNotify>();
                }
            }
        }

        dm.dlg = this;

        var luaPath = "Window/Windows/";
        luaPath+= data.code.IsNullOrEmpty()? data.prefab: data.code;

        if (LuaMgr.instance.IsLuaFile(luaPath))
        {
            luaWin = LuaGlobal.instance.CreateWindowTbl(luaPath);
        }
        
        mObj = instWinObj;
        CallLuaFun("Init", this);

    }

    void CallLuaFun(string funName, object parameter = null)
    {
        if (luaWin != null)
        {
            ((LuaFunction)luaWin[funName]).call(luaWin, parameter);
        }
    }
    bool show;

    public void Hide()
    {
        if (!show)
        {
            Debug.LogError(string.Format("窗口({0})并未显示", name));
            return;
        }
        mObj.SetActive(false);
        show = false;

        EventMgr.WindowHide.Notify(data.winName, data.desc);

        if (dm.OnHide != null)
        {
            dm.OnHide.Invoke();
        }
        CallLuaFun("OnHide");
        if(mHideHandle!=null)
        {
            mHideHandle.CancelAll();
        }
    }
    public void Show(object parameter=null)
    {
        if (show)
        {
            Debug.LogError(string.Format("窗口({0})已显示", name));
            return;
        }
        mObj.SetActive(true);
        show = true;

        EventMgr.WindowShow.Notify(data.winName, data.desc);

        if (dm.OnShow != null)
        {
            dm.OnShow.Invoke();
        }
        CallLuaFun("OnShow", parameter);
    }


    /// <summary>
    /// 打开窗口<para/>
    /// </summary>
    public void Open(object parameter = null)
    {
        GlobalCoroutine.Start(_Open(parameter));
    }

    /// <summary>
    /// 打开窗口<para/>
    /// </summary>
    IEnumerator _Open(object parameter=null)
    {
        if (mObj == null)
        {
            yield return Create();
        }
        if (show) yield break;

        Show(parameter);
    }
    
    /// <summary>
    /// 关闭窗口<para/>
    /// </summary>
    public void Close()
    {
        if (loadingHandle!=null)//如果正在加载过程中,终止操作
        {
            loadingHandle.CancelAll();
            DlgMgr.Inst.Remove(data.winName);
            //GlobalMgr.instance.Collect();
            return;
        }

        Hide();
        if (data.destroyByClose)
        {
            Destroy();
        }
    }

    public void Destroy(bool immediate=true)
    {
        DlgMgr.Inst.Remove(data.winName);
        if(immediate) UnityEngine.Object.DestroyImmediate(mObj);
        else UnityEngine.Object.Destroy(mObj);
        
        CallLuaFun("OnDestroy");

        if (mDestroyHandle != null)
        {
            mDestroyHandle.CancelAll();
        }
        //GlobalMgr.instance.Collect();
    }
    /// <summary>
    /// 设置窗口关联的数据对象
    /// </summary>
    public object DataContent
    {
        set
        {
            LinkObj.DataContent = value;
        }
        get
        {
            return LinkObj.DataContent;
        }
    }
    /// <summary>
    /// 获取窗口关联数据对象的组件
    /// </summary>
    public LinkObject LinkObj
    {
        get
        {
            if(mLinkObj==null)
            {
                mLinkObj = Obj.GetComponent<LinkObject>();
                if (mLinkObj == null)
                {
                    mLinkObj = Obj.AddComponent<LinkObject>();
                }
            }
            return mLinkObj;
        }
    }
    LinkObject mLinkObj;
}
