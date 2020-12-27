using CqCore;
using CustomModel;
using UnityCore;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// Main后于控制台在Start中开始执行
/// </summary>
public class Main : MonoBehaviourExtended
{
    public enum TestChangeSystemLanguage
    {
        [EnumLabel("中文")]
        Chinese=6,
        [EnumLabel("英文")]
        English =10,
    }

    [ComBox("语言", ComBoxStyle.RadioBox),OnValueChanged("OnSystemLanguageChanged")]
    public TestChangeSystemLanguage language;

    public void OnSystemLanguageChanged()
    {
        SettingModel.instance.SystemLanguage = (SystemLanguage)language;
    }

    void Start()
    {
        GlobalMgr.instance.main = this;
        SettingModel.instance.CheckUpdate = GlobalMgr.instance.isMobilePlatform;
        language = (TestChangeSystemLanguage)SettingModel.instance.SystemLanguage;
        
        GlobalCoroutine.recordStacktrace = CustomSetting.Inst.recordStacktrace;

        CqDebug.BeginSample = (str) => Profiler.BeginSample(str);
        CqDebug.EndSample = () => Profiler.EndSample();

        InputMgr.instance.KeyBoardInst.KeyDown_CallBack(KeyBoardInst_OnKeyDown, DestroyHandle);

        ApplicationUtil.Init();

        GlobalMgr.instance.Start();
    }
    
    private void KeyBoardInst_OnKeyDown(KeyCode obj)
    {
        EventMgr.KeyBoardDown.Notify(obj,this);
    }


    private void OnApplicationQuit()
    {
        EventMgr.ApplicationQuit.Notify();
        GlobalMgr.instance.DisposeAll();
    }

    void OnApplicationPause(bool pause)
    {
        EventMgr.ApplicationPause.Notify(pause);
    }
}

