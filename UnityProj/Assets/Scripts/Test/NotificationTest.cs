using UnityEngine;
using System.Collections;
using CqCore;
using System.Threading;
using UnityEngine.UI;
public class NotificationTest : MonoBehaviour
{
    void Awake()
    {
        dh = new CancelHandle();
        Android.LocalNotification.ClearNotifications();
    }
    private void OnDestroy()
    {
        Stop();
    }

    public void OneTime()
    {
        //Debug.Log("OneTime");
        Android.LocalNotification.SendNotification( 2, "操盘通知", "阿里巴巴可以买入了!"+ RandomUtil.Random(1, 100), dh,true,true,true,"", "boing");
    }

    public void OneTimeBigIcon()
    {
        Handheld.Vibrate();
        GlobalCoroutine.DelayCall(2f, Handheld.Vibrate);
        //LocalNotification.SendNotification(1, 5000, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon")    Android.LocalNotification.Action action1 = new Android.LocalNotification.Action("background", "In Background", this);
        Android.LocalNotification.Action action1 = new Android.LocalNotification.Action("background", "In Background", this);
        action1.Foreground = false;
        Android.LocalNotification.Action action2 = new Android.LocalNotification.Action("foreground", "In Foreground", this);
        Android.LocalNotification.SendNotification(5, "Title", "Long message text with actions", dh, true, true, true, null, "boing", "default", action1, action2);
    }
    CancelHandle dh;

    public void Repeating()
    {
        //OneTime();
        //dh = UnityDelay.Call(10, OneTime);
        Android.LocalNotification.SendRepeatingNotification( 5, 8, "Title", "Long message text"+RandomUtil.Random(1,100), dh);
    }

    public void Stop()
    {
        if(dh!=null)
        {
            dh.CancelAll();
        }
    }

    public void OnAction(string identifier)
    {
        Debug.Log("Got action " + identifier);
    }

}
