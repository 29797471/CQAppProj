using CqCore;
using CustomModel;
using System;
using System.Collections;
using UnityEngine;

public partial class GlobalMgr : Singleton<GlobalMgr>
{
    public Main main;

    public bool isMobilePlatform
    {
        get
        {
            if (!Application.isPlaying) return false;
            return Application.isMobilePlatform || ApplicationUtil.runByMobileDevice;
        }
    }

    /// <summary>
    /// 震动
    /// </summary>
    public void Vibrate()
    {
        Handheld.Vibrate();
    }

    /// <summary>
    /// 推送通知
    /// </summary>
    public void PushNotification(float delay,string title,string message)
    {
        Android.LocalNotification.SendNotification(2, title, message, null, true, true, true, "", "boing");
    }

    /// <summary>
    /// 对应内网版,外网版等各个版本
    /// </summary>
    //public Config_Version.DataItem Version
    //{
    //    get
    //    {
    //        if(Application.isMobilePlatform)
    //        {
    //            if(SettingModel.instance.Version.IsNullOrEmpty())
    //            {
    //                SettingModel.instance.Version = CustomSetting.Inst.version;
    //            }
    //            return Config_Version.Dic[SettingModel.instance.Version];
    //        }
    //        return Config_Version.Dic[CustomSetting.Inst.version];
    //    }
    //}
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 剪切板
    /// </summary>
    public string systemCopyBuffer
    {
        set
        {
            GUIUtility.systemCopyBuffer = value;
        }
        get
        {
            return GUIUtility.systemCopyBuffer;
        }
    }
    /*
    /// <summary>
    /// APP重启,暂未实现
    /// </summary>
    /// <param name="delay"></param>
    public void Restart(int delay)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        mainActivity.Call("doRestart", delay);
        jc.Dispose();
        mainActivity.Dispose();
    }
    */

    public void ReStart(bool checkUpdate = false)
    {
        SettingModel.instance.mCheckUpdate = checkUpdate;
        
        GlobalCoroutine.DelayCall(1,() =>
        {
            DisposeAll();
            Start();
        });
    }

    public void DisposeAll()
    {
        mExitHandle .CancelAll();
        GlobalCoroutine.StopAllCoroutines();

        //EventMgr.Dispose();
        LuaMgr.instance.Dispose();
    }

    public void Collect()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    

    public AppInstallState State { get; private set; }
    int mPingMs;
    /// <summary>
    /// 网络延时
    /// </summary>
    public int pingMs
    {
        set
        {
            if(mPingMs!=value)
            {
                mPingMs = value;
                EventMgr.NetPingUpdate.Notify(mPingMs);
            }
        }
        get
        {
            return mPingMs;
        }
    }
    /// <summary>
    /// 启动游戏
    /// </summary>
    public void Start()
    {
        GlobalCoroutine.Start(Start_It(), ExitHandle);
    }

    /// <summary>
    /// 重启时或者退出时调用的委托.
    /// </summary>
    CancelHandle mExitHandle =new CancelHandle();
    public ICancelHandle ExitHandle 
    {
        get => mExitHandle;
    }
    IEnumerator Start_It()
    {
        State = AppInstallState.None;
        if (SettingModel.instance.LastAppVersion.IsNullOrEmpty())//当首次安装
        {
            State = AppInstallState.FristStart;
        }
        else if (SettingModel.instance.LastAppVersion != Application.version)//覆盖安装时清除用户目录
        {
            State = AppInstallState.OverlayInstall;
            if (CustomSetting.Inst.clearPersistentData)
            {
                DirOpr.Delete(ApplicationUtil.persistentDataPath + "/" + CustomSetting.Inst.cachResPath);
            }
        }
        SettingModel.instance.LastAppVersion = Application.version;

        Debug.Log(string.Format("FileVersionMgr.Init({0},{1})", CustomSetting.Inst.updateUrl, CustomSetting.Inst.cachResPath));
        FileVersionMgr.instance.Init(CustomSetting.Inst.updateUrl, CustomSetting.Inst.cachResPath, CustomSetting.Inst.files);


        var types = AssemblyUtil.GetTypesByNamespace("CustomModel");
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].GetInterface("IModel") != null)
            {
                var x = AssemblyUtil.GetStaticMemberValue(types[i], "instance") as IModel;
                if (x != null) x.Init();
            }
        }
        if(isMobilePlatform && State!= AppInstallState.None)
        {
            yield return LuaMgr.instance.UnZipLua();
        }
        yield return LuaMgr.instance.StartLua();
        yield return ResMgr.Init_It();
        DlgMgr.Inst.Init();
        DlgLayerMgr.Inst.Init();
        ServerTime.instance.Init();
    }

}
