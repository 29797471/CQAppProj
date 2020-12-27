﻿using System;
using System.IO;
using UnityEngine;
using CqCore;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using System.Collections.Generic;

namespace Android
{
    /// <summary>
    /// Android本地通知,应用程序内发起,一段时间后回调,当切换到后台时会有提示,和声音
    /// </summary>
    public class LocalNotification
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";
    private static string actionClassName = "net.agasper.unitynotification.NotificationAction";
#endif
        private static string bundleIdentifier { get { return Application.identifier; } }

        /// <summary>
        ///  安卓消息通知,由于切换到后台后主线程会被暂停,所以要延迟推送.如果在前台推送时,不会产生声音和震动提示.
        /// </summary>
        /// <param name="delay">延迟时间（秒） </param>
        /// <param name="title">推送内容标题 </param>
        /// <param name="message">推送内容 </param>
        /// <param name="bgColor">nity提供的API，以32位格式表示RGBA颜色 </param>
        /// <param name="sound"> 声音 </param>
        /// <param name="vibrate">颤动 </param>
        /// <param name="lights">闪亮 </param>
        /// <param name="bigIcon">缺省值 </param>
        /// <param name="soundName">声音文件名 </param>
        /// <param name="channel">/都同意给false，表示本地推送（我理解的） </param>
        /// <param name="actions">回调</param>
        /// <returns></returns>
        public static void SendNotification(float delay, string title, string message,
            CancelHandle handle = null, bool sound = true, bool vibrate = true, bool lights = true,
            string bigIcon = "", string soundName = null, string channel = "default", params Action[] actions)
        {
            int id = new System.Random().Next();
            SendNotification(id, (long)delay * 1000, title, message, new Color32(0xff, 0x44, 0x44, 255), sound, vibrate, lights, bigIcon, soundName, channel, actions);
            if (handle != null)
            {
                handle.CancelAct += () => CancelNotification(id);
            }
        }

        static int SendNotification(int id, long delayMs, string title, string message,
            Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true,
            string bigIcon = "", string soundName = null, string channel = "default", params Action[] actions)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetNotification", id, delayMs, title, message, message,
                sound ? 1 : 0, soundName, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small",
                ToInt(bgColor), bundleIdentifier, channel, PopulateActions(actions));
        }
        return id;
#elif UNITY_IOS && !UNITY_EDITOR
        iOSNotification notification = new iOSNotification();
        notification.identifier = id;
        notification.message = message;
        notification.delay = ((double) delayMs) / 1000.0;
        notification.repeat = 0;
        notification.category = channel;
        notification.sound = sound;
        notification.soundName = soundName;
        SetActions(ref notification, actions);
        scheduleNotification(ref notification);
        return id;
#else
            return 0;
#endif
        }
        public static void SendRepeatingNotification(float delayMs, float timeoutMs, string title, string message,
            CancelHandle handle = null, bool sound = true, bool vibrate = true, bool lights = true,
            string bigIcon = "", string soundName = null, string channel = "default", params Action[] actions)
        {
            int id = new System.Random().Next();
            SendRepeatingNotification(id, (long)delayMs * 1000, (long)timeoutMs * 1000, title, message, new Color32(0xff, 0x44, 0x44, 255), sound, vibrate, lights, bigIcon, soundName, channel, actions);
            if (handle != null)
            {
                handle.CancelAct += () => CancelNotification(id);
            }
        }
        static int SendRepeatingNotification(int id, long delayMs, long timeoutMs, string title, string message,
            Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true,
            string bigIcon = "", string soundName = null, string channel = "default", params Action[] actions)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delayMs, title, message, message, timeoutMs,
                sound ? 1 : 0, soundName, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small",
                ToInt(bgColor), bundleIdentifier, channel, PopulateActions(actions));
        }
        return id;
#elif UNITY_IOS && !UNITY_EDITOR
        iOSNotification notification = new iOSNotification();
        notification.identifier = id;
        notification.message = message;
        notification.delay = ((double) delayMs) / 1000.0;
        notification.repeat = CalculateiOSRepeat(timeoutMs);
        notification.category = channel;
        notification.sound = sound;
        notification.soundName = soundName;
        SetActions(ref notification, actions);
        scheduleNotification(ref notification);
        return id;
#else
            return 0;
#endif
        }

        static void CancelNotification(int id)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("CancelPendingNotification", id);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        cancelNotification(id);
#endif
        }

        public static void ClearNotifications()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("ClearShowingNotifications");
        }
#elif UNITY_IOS && !UNITY_EDITOR
        cancelAllNotifications();
#endif
        }


        /// <summary>
        /// 这允许您为不同类型的通知创建自定义通道。
        /// Android或更高版本需要频道。如果不调用此方法，将使用您提供给SendNotification的配置为您创建通道。
        /// </summary>
        public static void CreateChannel(string identifier, string name, string description, Color32 lightColor, bool enableLights = true, string soundName = null, Importance importance = Importance.Default, bool vibrate = true, long[] vibrationPattern = null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("CreateChannel", identifier, name, description, (int) importance, soundName, enableLights ? 1 : 0, ToInt(lightColor), vibrate ? 1 : 0, vibrationPattern, bundleIdentifier);
        }
#endif
        }

        public enum Importance
        {
            /// Default notification importance: shows everywhere, makes noise, but does not visually intrude.
            Default = 3,

            /// Higher notification importance: shows everywhere, makes noise and peeks. May use full screen intents.
            High = 4,

            /// Low notification importance: shows everywhere, but is not intrusive.
            Low = 2,

            /// Unused.
            Max = 5,

            /// Min notification importance: only shows in the shade, below the fold. This should not be used with Service.startForeground since a foreground service is supposed to be something the user cares about so it does not make semantic sense to mark its notification as minimum importance. If you do this as of Android version O, the system will show a higher-priority notification about your app running in the background.
            Min = 1,

            /// A notification with no importance: does not show in the shade.
            None = 0
        }

        public class Action
        {
            public Action(string identifier, string title, MonoBehaviour handler)
            {
                this.Identifier = identifier;
                this.Title = title;
                if (handler != null)
                {
                    this.GameObject = handler.gameObject.name;
                    this.HandlerMethod = "OnAction";
                }
            }

            public string Identifier;
            public string Title;
            public string Icon;
            public bool Foreground = true;
            public string GameObject;
            public string HandlerMethod;
        }

        private static int ToInt(Color32 color)
        {
            return color.r * 65536 + color.g * 256 + color.b;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject PopulateActions(Action[] actions)
    {
        AndroidJavaObject actionList = null;
        if (actions.Length > 0)
        {
            actionList = new AndroidJavaObject("java.util.ArrayList");
            for (int i = 0; i < actions.Length; i++)
            {
                var action = actions[i];
                using (AndroidJavaObject notificationObject = new AndroidJavaObject(actionClassName))
                {
                    notificationObject.Call("setIdentifier", action.Identifier);
                    notificationObject.Call("setTitle", action.Title);
                    notificationObject.Call("setIcon", action.Icon);
                    notificationObject.Call("setForeground", action.Foreground);
                    notificationObject.Call("setGameObject", action.GameObject);
                    notificationObject.Call("setHandlerMethod", action.HandlerMethod);
                    actionList.Call<bool>("add", notificationObject);
                }
            }
        }
        return actionList;
    }
#endif

#if UNITY_IOS && !UNITY_EDITOR
    internal struct iOSNotification {
        public int identifier;
        public string message;
        public double delay;
        public UnityEngine.iOS.CalendarUnit repeat;
        public string category;
        public bool sound;
        public string soundName;
        public int actionCount;
        public iOSNotificationAction action1;
        public iOSNotificationAction action2;
        public iOSNotificationAction action3;
        public iOSNotificationAction action4;
    }

    internal struct iOSNotificationAction {
        public string identifier;
        public string title;
        public bool foreground;
        public string gameObject;
        public string handlerMethod;
    }

	[DllImport ("__Internal")] internal static extern void scheduleNotification(ref iOSNotification notification);
    [DllImport ("__Internal")] internal static extern void cancelNotification(int identifier);
    [DllImport ("__Internal")] internal static extern void cancelAllNotifications();

    internal static void SetActions(ref iOSNotification notification, Action[] actions)
    {
        notification.actionCount = actions.Length;
        if (actions.Length > 0)
            notification.action1 = CreateAction(actions[0]);
        if (actions.Length > 1)
            notification.action2 = CreateAction(actions[1]);
        if (actions.Length > 2)
            notification.action3 = CreateAction(actions[2]);
        if (actions.Length > 3)
            notification.action4 = CreateAction(actions[3]);
    }

    internal static iOSNotificationAction CreateAction(Action from)
    {
        iOSNotificationAction action;
        action.identifier = from.Identifier;
        action.title = from.Title;
        action.foreground = from.Foreground;
        action.gameObject = from.GameObject;
        action.handlerMethod = from.HandlerMethod;
        return action;
    }

    public static UnityEngine.iOS.CalendarUnit CalculateiOSRepeat(long timeoutMS)
    {
        if (timeoutMS == 0)
            return 0;

        long timeoutMinutes = timeoutMS / (1000 * 60);
        if (timeoutMinutes == 1)
            return UnityEngine.iOS.CalendarUnit.Minute;
        if (timeoutMinutes == 60)
            return UnityEngine.iOS.CalendarUnit.Hour;
        if (timeoutMinutes == 60 * 24)
            return UnityEngine.iOS.CalendarUnit.Day;
        if (timeoutMinutes >= 60 * 24 * 2 && timeoutMinutes <= 60 * 24 * 5)
            return UnityEngine.iOS.CalendarUnit.Weekday;
        if (timeoutMinutes == 60 * 24 * 7)
            return UnityEngine.iOS.CalendarUnit.Week;
        if (timeoutMinutes >= 60 * 24 * 28 && timeoutMinutes <= 60 * 24 * 31)
            return UnityEngine.iOS.CalendarUnit.Month;
        if (timeoutMinutes >= 60 * 24 * 91 && timeoutMinutes <= 60 * 24 * 92)
            return UnityEngine.iOS.CalendarUnit.Quarter;
        if (timeoutMinutes >= 60 * 24 * 365 && timeoutMinutes <= 60 * 24 * 366)
            return UnityEngine.iOS.CalendarUnit.Year;
        throw new ArgumentException("Unsupported timeout for iOS - must equal a minute, hour, day, 2-5 days (for 'weekday'), week, month, quarter or year but was " + timeoutMS);
    }
#endif
    }
}

