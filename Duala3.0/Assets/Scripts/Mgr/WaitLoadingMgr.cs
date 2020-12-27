using CqCore;
using System;
using System.Collections;

/// <summary>
/// 用户操作请求返回系统<para/>
/// 一开始不阻止用户操作,<para/>
/// 一段时间后如果没有返回,立即阻止用户操作,打开加载界面,<para/>
/// 再持续一段时间后,终止请求,并结束加载界面,弹出请求失败的界面<para/>
/// </summary>
public class WaitLoadingMgr : Singleton<WaitLoadingMgr>
{
    /// <summary>
    /// 打开加载界面计数
    /// </summary>
    int openLoadingCount;

    /// <summary>
    /// 启动一个协程,如果长时间没有完成会被打断弹出提示,短时间没有返回,根据不同样式作处理
    /// </summary>
    /// <param name="iterator"></param>
    /// <param name="allowUserOprTime">开始加载时不阻止用户操作的时间</param>
    /// <param name="loadingPlayDuration">loading并阻止用户一段时间后终止操作</param>
    public void Start(IEnumerator iterator, float allowUserOprTime = 1f, float loadingPlayDuration = 3f)
    {
        CancelHandle  requestComplete = new CancelHandle();
        var userRequest = GlobalCoroutine.Start(iterator, null,  requestComplete.CancelAll);
        GlobalCoroutine.Start(StartCoroutine(userRequest, requestComplete, allowUserOprTime, loadingPlayDuration),  requestComplete);
    }
    IEnumerator StartCoroutine(CqCoroutine userRequest, ICancelHandle requestComplete,
        float allowUserOprTime,float loadingPlayDuration)
    {
        //用户界面未阻止操作
        yield return GlobalCoroutine.Sleep(allowUserOprTime);

        //阻止用户操作,等待操作完成
        OpenLoadingUI();
        if (requestComplete != null)
        {
            requestComplete.CancelAct += CloseLoadingUI;
        }
        yield return GlobalCoroutine.Sleep(loadingPlayDuration);
        if (requestComplete != null)
        {
            requestComplete.CancelAct -= CloseLoadingUI;
        }
        CloseLoadingUI();

        //终止操作行为,弹出失败通知
        userRequest.Stop();
        
        EventMgr.DoOpenUserOprFail.Notify();
    }
    void OpenLoadingUI()
    {
        if (openLoadingCount == 0) EventMgr.DoOpenOrCloseLoadingDlg.Notify(true);
        openLoadingCount++;
    }
    void CloseLoadingUI()
    {
        openLoadingCount--;
        if (openLoadingCount == 0) EventMgr.DoOpenOrCloseLoadingDlg.Notify(false);
    }


}

