using System;

public static class LoadMgr
{
    static bool _showLoading;
    public static bool showLoading { get { return _showLoading; } }
    public static void ShowLoading()
    {
        _showLoading = true;
        LoadingCount = 0;
    }
    public static void HideLoading()
    {
        EventMgr.ResLoadEnd.Notify();
        _showLoading = false;
    }
    /// <summary>
    /// 开始加载
    /// </summary>
    public static Action StartLoading()
    {
        if (LoadingCount == 0)
        {
            LoadingCount++;
            EventMgr.ResLoadStart.Notify();
        }
        else
        {
            LoadingCount++;
        }
        return () =>
        {
            LoadingCount--;
            if (LoadingCount == 0) EventMgr.ResLoadEnd.Notify();
        };
    }
    /// <summary>
    /// 加载数量
    /// </summary>
    static int LoadingCount;

    /// <summary>
    /// 是否正在加载
    /// </summary>
    public static bool IsLoading
    {
        get
        {
            return LoadingCount != 0;
        }
    }
}
