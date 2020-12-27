using CqCore;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

/// <summary>
/// 当前服务器系统时间
/// </summary>
public class ServerTime : Singleton<ServerTime>
{

    /// <summary>
    /// 当前服务器时间戳(秒)
    /// </summary>
    public int Unix_timestamp { get; private set; }

    /// <summary>
    /// 当前服务器时间戳(毫秒)
    /// </summary>
    long Unix_timestamp_long { get; set; }
    
    /// <summary>
    /// 当前服务器时刻的本地显示时间(本地时区)
    /// </summary>
    public DateTime CurrentDate
    {
        get
        {
            return TimeUtil.ToLocalTime(Unix_timestamp_long);
        }
    }

    /// <summary>
    /// 形如 2017-04-12 15:23:33
    /// </summary>
    public string CurrentDateStr
    {
        get
        {
            return TimeUtil.GetTimeStringByDate(CurrentDate);
        }
    }

    /// <summary>
    /// 在线时间
    /// </summary>
    public int OnLineTime
    {
        get
        {
            return (int)Time.time;
        }
    }

    /// <summary>
    /// 形如 2017-04-12
    /// </summary>
    public string CurrentDay
    {
        get
        {
            return TimeUtil.TimeFormat(Unix_timestamp, "yyyy-MM-dd");
        }
    }

    /// <summary>
    /// 同步服务器时间戳<para/>
    /// 通过发送一次心跳请求,并获取服务器返回,来计算服务器当前真实时间戳<para/>
    /// 传入请求返回的总时间和服务器返回的时间戳
    /// </summary>
    public void SyncTime(long ServerUnix_timestamp_long,float deltaTime)
    {
        var realUnix_timestamp_long = ServerUnix_timestamp_long+(long)(deltaTime/2*1000);
        Debug.LogWarning(string.Format("校准相差:{0}秒",(realUnix_timestamp_long - Unix_timestamp_long) /1000f ));
        Unix_timestamp_long = realUnix_timestamp_long;
    }

    /// <summary>
    /// 时间步进长度(毫秒)
    /// </summary>
    const long step = 50;
    public void Init()
    {
        var cancelHandle = GlobalMgr.instance.ExitHandle;
        Unix_timestamp_long = TimeUtil.Unix_timestamp_long;
        
        var t = new Timer((o) =>
        {
            Unix_timestamp_long += step;
        }, null, 0, step);
        if(cancelHandle!=null)
        {
            cancelHandle.CancelAct += t.Dispose;
        }

        GlobalCoroutine.Start(UpdateTime(), cancelHandle);
    }
    IEnumerator UpdateTime()
    {
        int temp;
        while (true)
        {
            temp = (int)(Unix_timestamp_long / 1000);
            if (temp!=Unix_timestamp)
            {
                Unix_timestamp = temp;
                EventMgr.ServerTimeUpdate.Notify(Unix_timestamp);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 和本地时间比较,得到日期差 
    /// 明天=1,今天=0,后天=2,昨天=-1
    /// </summary>
    public int DayDelta(int dayOfSeconds)
    {
        var d1 = TimeUtil.ToLocalTime(dayOfSeconds * 1000L);
        var d2=CurrentDate;
        var e1 = d1 - d1.TimeOfDay;
        var e2 = d2 - d2.TimeOfDay;
        return (e1 - e2).Days;
    }

    public string TimeSpanHms(int seconds)
    {
        if (seconds <= 0) return "00:00:00";

        System.TimeSpan timeSpan = new System.TimeSpan(0, 0, seconds);
        if (timeSpan.Days == 0)
            return TimeUtil.TimeSpanFormat(seconds, "HH:mm:ss");

        // 超过24小时的处理
        return (timeSpan.Days * 24 + timeSpan.Hours) + ":" + TimeUtil.TimeSpanFormat(seconds, "mm:ss");
    }

    public string TimeSpanMs(int seconds)
    {
        if (seconds <= 0) return "00:00";

        return TimeUtil.TimeSpanFormat(seconds, "mm:ss");
    }


    
}
