using CqCore;
using System.Collections;
using UnityCore;
using UnityEngine;

/// <summary>
/// 测试Ping 一个IP的延迟
/// </summary>
public class TestPing : MonoBehaviourExtended
{
    [TextBox("域名或者ip"),OnValueChanged("IPAddressChanged")]
    public string IPAddress = "www.baidu.com";
    CqCoroutine cc;
    public void IPAddressChanged()
    {
        if (cc != null) cc.Stop();
        cc= StartCoroutine(GetIP());
    }
    AsyncReturn<string> data=new AsyncReturn<string>();
    IEnumerator GetIP()
    {
        yield return PingUtil.GetIPv4(IPAddress, data);
        ip = data.data;
    }
    [TextBox("IP"),IsEnabled(false)]
    public string ip;

    [TextBox("每次PIng间隔(s)")]
    public float delta = 1f;


    [TextBox("延迟(ms)"), IsEnabled(false)]
    public int delayTime;
    public int DelayTime
    {
        set
        {
            if (delayTime != value)
            {
                delayTime = value;
                GlobalMgr.instance.pingMs = value;
            }
        }
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            IPAddressChanged();
            StartCoroutine(TryPing(), DisabledHandle);
        }
    }

    IEnumerator TryPing()
    {
        while (true)
        {
            if (ip.IsNullOrEmpty())
            {
                DelayTime = -1;
            }
            else
            {
                var ping = new Ping(ip);
                float lastTime = Time.time;
                while (!ping.isDone)
                {
                    if(Time.time- lastTime>0.45)
                    {
                        break;
                    }
                    if (ping.ip.IsNullOrEmpty()) break;
                    yield return null;
                }

                DelayTime = ping.time;
                ping.DestroyPing();
            }
            yield return GlobalCoroutine.Sleep(delta);
        }
    }

}