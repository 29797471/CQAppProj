using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using SLua;

/// <summary>
/// 事件管理器<para/>
/// 主要处理那些可能会存在多个事件监听者的事件<para/>
/// 当是一对一的时候,或者 要处理返回值,没有必要使用事件管理器,参数传递委托就好<para/>
/// example: <para/>
///     EventMgr.ButtonClick.EventHandler +=OnButtonClick;<para/>
///     EventMgr.ButtonClick.Notify("btn1", "win2", this);
/// </summary>
public static class EventMgr
{
    /// <summary>
    /// 将一个序列化的事件结构,反序列化生成事件消息,并发送
    /// </summary>
    public static void UrlNotify(string url, object sender = null)
    {
        if (!url.IsNullOrEmpty())
        {
            var o = (CustomEventArgs)Torsion.Deserialize(url);
            if (o != null) o.Notify(sender);
        }
    }
    public static void Dispose()
    {
        if(OnDispose!=null)
        {
            OnDispose();
            //OnDispose = null;有可能会多次清除
        }
    }
    static event Action OnDispose;

    
    #region 本地 -> 服务器时间戳更新
    /// <summary>服务器时间戳更新</summary>
    public static class ServerTimeUpdate
    {
        static ServerTimeUpdate()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 时间戳(秒)
            /// </summary>
            public readonly int timestamp;
            
            /// <summary>服务器时间戳更新</summary>
            public _EventArgs(int timestamp,object sender=null)
            {
                this.timestamp=timestamp;
            }
            public override bool Notify(object sender=null)
            {
                return ServerTimeUpdate.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 服务器时间戳更新
        /// </summary>
        /// <param name="timestamp">时间戳(秒)</param>
        public static void Notify( int timestamp, object sender = null)
        {
            var eventData = new _EventArgs(timestamp,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 网络Ping延时变更
    /// <summary>网络Ping延时变更</summary>
    public static class NetPingUpdate
    {
        static NetPingUpdate()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 毫秒
            /// </summary>
            public readonly int time;
            
            /// <summary>网络Ping延时变更</summary>
            public _EventArgs(int time,object sender=null)
            {
                this.time=time;
            }
            public override bool Notify(object sender=null)
            {
                return NetPingUpdate.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 网络Ping延时变更
        /// </summary>
        /// <param name="time">毫秒</param>
        public static void Notify( int time, object sender = null)
        {
            var eventData = new _EventArgs(time,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 多语言 -> 语言切换
    /// <summary>语言切换</summary>
    public static class LanguageChange
    {
        static LanguageChange()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 语言
            /// </summary>
            public readonly UnityEngine.SystemLanguage sysLanguage;
            
            /// <summary>语言切换</summary>
            public _EventArgs(UnityEngine.SystemLanguage sysLanguage,object sender=null)
            {
                this.sysLanguage=sysLanguage;
            }
            public override bool Notify(object sender=null)
            {
                return LanguageChange.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 语言切换
        /// </summary>
        /// <param name="sysLanguage">语言</param>
        public static void Notify( UnityEngine.SystemLanguage sysLanguage, object sender = null)
        {
            var eventData = new _EventArgs(sysLanguage,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 消息 -> 批处理
    /// <summary>批处理</summary>
    public static class CmdCommand
    {
        static CmdCommand()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 命令
            /// </summary>
            public readonly string command;
            
            /// <summary>批处理</summary>
            public _EventArgs(string command,object sender=null)
            {
                this.command=command;
            }
            public override bool Notify(object sender=null)
            {
                return CmdCommand.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="command">命令</param>
        public static void Notify( string command, object sender = null)
        {
            var eventData = new _EventArgs(command,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 窗口显示
    /// <summary>窗口显示</summary>
    public static class WindowShow
    {
        static WindowShow()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口
            /// </summary>
            public readonly string win;
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string desc;
            
            /// <summary>窗口显示</summary>
            public _EventArgs(string win,string desc,object sender=null)
            {
                this.win=win;
                this.desc=desc;
            }
            public override bool Notify(object sender=null)
            {
                return WindowShow.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 窗口显示
        /// </summary>
        /// <param name="win">窗口</param>
        /// <param name="desc">窗口名称</param>
        public static void Notify( string win, string desc, object sender = null)
        {
            var eventData = new _EventArgs(win,desc,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 窗口隐藏
    /// <summary>窗口隐藏</summary>
    public static class WindowHide
    {
        static WindowHide()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口
            /// </summary>
            public readonly string win;
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string desc;
            
            /// <summary>窗口隐藏</summary>
            public _EventArgs(string win,string desc,object sender=null)
            {
                this.win=win;
                this.desc=desc;
            }
            public override bool Notify(object sender=null)
            {
                return WindowHide.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 窗口隐藏
        /// </summary>
        /// <param name="win">窗口</param>
        /// <param name="desc">窗口名称</param>
        public static void Notify( string win, string desc, object sender = null)
        {
            var eventData = new _EventArgs(win,desc,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 缓动 -> 窗口缓入开始
    /// <summary>窗口缓入开始</summary>
    public static class WindowFadeInStart
    {
        static WindowFadeInStart()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
            
            /// <summary>窗口缓入开始</summary>
            public _EventArgs(string winName,object sender=null)
            {
                this.winName=winName;
            }
            public override bool Notify(object sender=null)
            {
                return WindowFadeInStart.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 窗口缓入开始
        /// </summary>
        /// <param name="winName">窗口名称</param>
        public static void Notify( string winName, object sender = null)
        {
            var eventData = new _EventArgs(winName,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 缓动 -> 窗口缓入结束
    /// <summary>窗口缓入结束</summary>
    public static class WindowFadeInEnd
    {
        static WindowFadeInEnd()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
            
            /// <summary>窗口缓入结束</summary>
            public _EventArgs(string winName,object sender=null)
            {
                this.winName=winName;
            }
            public override bool Notify(object sender=null)
            {
                return WindowFadeInEnd.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 窗口缓入结束
        /// </summary>
        /// <param name="winName">窗口名称</param>
        public static void Notify( string winName, object sender = null)
        {
            var eventData = new _EventArgs(winName,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 缓动 -> 窗口缓出开始
    /// <summary>窗口缓出开始</summary>
    public static class WindowFadeOutStart
    {
        static WindowFadeOutStart()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
            
            /// <summary>窗口缓出开始</summary>
            public _EventArgs(string winName,object sender=null)
            {
                this.winName=winName;
            }
            public override bool Notify(object sender=null)
            {
                return WindowFadeOutStart.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 窗口缓出开始
        /// </summary>
        /// <param name="winName">窗口名称</param>
        public static void Notify( string winName, object sender = null)
        {
            var eventData = new _EventArgs(winName,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 缓动 -> 窗口缓出结束
    /// <summary>窗口缓出结束</summary>
    public static class WindowFadeOutEnd
    {
        static WindowFadeOutEnd()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
            
            /// <summary>窗口缓出结束</summary>
            public _EventArgs(string winName,object sender=null)
            {
                this.winName=winName;
            }
            public override bool Notify(object sender=null)
            {
                return WindowFadeOutEnd.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 窗口缓出结束
        /// </summary>
        /// <param name="winName">窗口名称</param>
        public static void Notify( string winName, object sender = null)
        {
            var eventData = new _EventArgs(winName,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 按钮点击
    /// <summary>按钮点击</summary>
    public static class ButtonClick
    {
        static ButtonClick()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
             
            /// <summary>
            /// 按钮名称
            /// </summary>
            public readonly string btnPath;
            
            /// <summary>按钮点击</summary>
            public _EventArgs(string winName,string btnPath,object sender=null)
            {
                this.winName=winName;
                this.btnPath=btnPath;
            }
            public override bool Notify(object sender=null)
            {
                return ButtonClick.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 按钮点击
        /// </summary>
        /// <param name="winName">窗口名称</param>
        /// <param name="btnPath">按钮名称</param>
        public static void Notify( string winName, string btnPath, object sender = null)
        {
            var eventData = new _EventArgs(winName,btnPath,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 开关点击
    /// <summary>开关点击</summary>
    public static class ToggleClick
    {
        static ToggleClick()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
             
            /// <summary>
            /// 开关名称
            /// </summary>
            public readonly string btnPath;
             
            /// <summary>
            /// 是否打开
            /// </summary>
            public readonly bool select;
            
            /// <summary>开关点击</summary>
            public _EventArgs(string winName,string btnPath,bool select,object sender=null)
            {
                this.winName=winName;
                this.btnPath=btnPath;
                this.select=select;
            }
            public override bool Notify(object sender=null)
            {
                return ToggleClick.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 开关点击
        /// </summary>
        /// <param name="winName">窗口名称</param>
        /// <param name="btnPath">开关名称</param>
        /// <param name="select">是否打开</param>
        public static void Notify( string winName, string btnPath, bool select, object sender = null)
        {
            var eventData = new _EventArgs(winName,btnPath,select,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 模拟开关点击
    /// <summary>模拟开关点击</summary>
    public static class SimulatorToggleClick
    {
        static SimulatorToggleClick()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 窗口名称
            /// </summary>
            public readonly string winName;
             
            /// <summary>
            /// 开关名称
            /// </summary>
            public readonly string togglePath;
            
            /// <summary>模拟开关点击</summary>
            public _EventArgs(string winName,string togglePath,object sender=null)
            {
                this.winName=winName;
                this.togglePath=togglePath;
            }
            public override bool Notify(object sender=null)
            {
                return SimulatorToggleClick.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 模拟开关点击
        /// </summary>
        /// <param name="winName">窗口名称</param>
        /// <param name="togglePath">开关名称</param>
        public static void Notify( string winName, string togglePath, object sender = null)
        {
            var eventData = new _EventArgs(winName,togglePath,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 通知 -> 执行打开或者关闭加载窗口
    /// <summary>执行打开或者关闭加载窗口</summary>
    public static class DoOpenOrCloseLoadingDlg
    {
        static DoOpenOrCloseLoadingDlg()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 打开/关闭
            /// </summary>
            public readonly bool isOpen;
            
            /// <summary>执行打开或者关闭加载窗口</summary>
            public _EventArgs(bool isOpen,object sender=null)
            {
                this.isOpen=isOpen;
            }
            public override bool Notify(object sender=null)
            {
                return DoOpenOrCloseLoadingDlg.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 执行打开或者关闭加载窗口
        /// </summary>
        /// <param name="isOpen">打开/关闭</param>
        public static void Notify( bool isOpen, object sender = null)
        {
            var eventData = new _EventArgs(isOpen,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 窗口 -> 通知 -> 执行弹出用户操作失败窗口
    /// <summary>执行弹出用户操作失败窗口</summary>
    public static class DoOpenUserOprFail
    {
        static DoOpenUserOprFail()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>执行弹出用户操作失败窗口</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return DoOpenUserOprFail.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 执行弹出用户操作失败窗口
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 游戏状态 -> 游戏启动
    /// <summary>游戏启动</summary>
    public static class GameStartup
    {
        static GameStartup()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>游戏启动</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return GameStartup.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 游戏启动
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 游戏状态 -> UI画布初始化完成
    /// <summary>UI画布初始化完成</summary>
    public static class CanvasInitComplete
    {
        static CanvasInitComplete()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>UI画布初始化完成</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return CanvasInitComplete.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// UI画布初始化完成
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 游戏状态 -> 资源热更新完成
    /// <summary>资源热更新完成</summary>
    public static class ResUpdateComplete
    {
        static ResUpdateComplete()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>资源热更新完成</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return ResUpdateComplete.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 资源热更新完成
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 游戏状态 -> 初始化服务器信息完成
    /// <summary>初始化服务器信息完成</summary>
    public static class InitServerAddressComplete
    {
        static InitServerAddressComplete()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>初始化服务器信息完成</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return InitServerAddressComplete.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 初始化服务器信息完成
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 游戏状态 -> 登录成功
    /// <summary>登录成功</summary>
    public static class LoginComplete
    {
        static LoginComplete()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>登录成功</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return LoginComplete.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 登录成功
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 游戏状态 -> 初始化模块
    /// <summary>初始化模块</summary>
    public static class InitModel
    {
        static InitModel()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>初始化模块</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return InitModel.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 初始化模块
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 加载 -> 开始加载资源
    /// <summary>开始加载资源</summary>
    public static class ResLoadStart
    {
        static ResLoadStart()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>开始加载资源</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return ResLoadStart.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 开始加载资源
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 加载 -> 资源加载完成
    /// <summary>资源加载完成</summary>
    public static class ResLoadEnd
    {
        static ResLoadEnd()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>资源加载完成</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return ResLoadEnd.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 资源加载完成
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 本地 -> 绘制样式改变
    /// <summary>绘制样式改变</summary>
    public static class DrawStyleChanged
    {
        static DrawStyleChanged()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 
            /// </summary>
            public readonly RoadDrawStyle style;
            
            /// <summary>绘制样式改变</summary>
            public _EventArgs(RoadDrawStyle style,object sender=null)
            {
                this.style=style;
            }
            public override bool Notify(object sender=null)
            {
                return DrawStyleChanged.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 绘制样式改变
        /// </summary>
        /// <param name="style"></param>
        public static void Notify( RoadDrawStyle style, object sender = null)
        {
            var eventData = new _EventArgs(style,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 设备交互 -> 退出应用
    /// <summary>退出应用</summary>
    public static class ApplicationQuit
    {
        static ApplicationQuit()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
            
            /// <summary>退出应用</summary>
            public _EventArgs(object sender=null)
            {
            }
            public override bool Notify(object sender=null)
            {
                return ApplicationQuit.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 退出应用
        /// </summary>
        public static void Notify( object sender = null)
        {
            var eventData = new _EventArgs(sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 设备交互 -> 应用暂停
    /// <summary>应用暂停</summary>
    public static class ApplicationPause
    {
        static ApplicationPause()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 暂停
            /// </summary>
            public readonly bool pause;
            
            /// <summary>应用暂停</summary>
            public _EventArgs(bool pause,object sender=null)
            {
                this.pause=pause;
            }
            public override bool Notify(object sender=null)
            {
                return ApplicationPause.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 应用暂停
        /// </summary>
        /// <param name="pause">暂停</param>
        public static void Notify( bool pause, object sender = null)
        {
            var eventData = new _EventArgs(pause,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 设备交互 -> Android交互
    /// <summary>Android交互</summary>
    public static class AndroidClick
    {
        static AndroidClick()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 按键<para>0.Escape</para><para>1.Home</para>
            /// </summary>
            public readonly int style;
            
            /// <summary>Android交互</summary>
            public _EventArgs(int style,object sender=null)
            {
                this.style=style;
            }
            public override bool Notify(object sender=null)
            {
                return AndroidClick.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// Android交互
        /// </summary>
        /// <param name="style">按键<para>0.Escape</para><para>1.Home</para></param>
        public static void Notify( int style, object sender = null)
        {
            var eventData = new _EventArgs(style,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 设备交互 -> 键盘按下
    /// <summary>键盘按下</summary>
    public static class KeyBoardDown
    {
        static KeyBoardDown()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 按键
            /// </summary>
            public readonly UnityEngine.KeyCode key;
            
            /// <summary>键盘按下</summary>
            public _EventArgs(UnityEngine.KeyCode key,object sender=null)
            {
                this.key=key;
            }
            public override bool Notify(object sender=null)
            {
                return KeyBoardDown.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="key">按键</param>
        public static void Notify( UnityEngine.KeyCode key, object sender = null)
        {
            var eventData = new _EventArgs(key,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 设备交互 -> 多触点移动
    /// <summary>多触点移动</summary>
    public static class MoreInputMove
    {
        static MoreInputMove()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 移动增量<para>放开为正,夹住为负</para>
            /// </summary>
            public readonly float delta;
            
            /// <summary>多触点移动</summary>
            public _EventArgs(float delta,object sender=null)
            {
                this.delta=delta;
            }
            public override bool Notify(object sender=null)
            {
                return MoreInputMove.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 多触点移动
        /// </summary>
        /// <param name="delta">移动增量<para>放开为正,夹住为负</para></param>
        public static void Notify( float delta, object sender = null)
        {
            var eventData = new _EventArgs(delta,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 设备交互 -> 单触点移动
    /// <summary>单触点移动</summary>
    public static class OneInputMove
    {
        static OneInputMove()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 移动增量
            /// </summary>
            public readonly Vector2 delta;
            
            /// <summary>单触点移动</summary>
            public _EventArgs(Vector2 delta,object sender=null)
            {
                this.delta=delta;
            }
            public override bool Notify(object sender=null)
            {
                return OneInputMove.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 单触点移动
        /// </summary>
        /// <param name="delta">移动增量</param>
        public static void Notify( Vector2 delta, object sender = null)
        {
            var eventData = new _EventArgs(delta,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 系统 -> 设备数据更新
    /// <summary>设备数据更新</summary>
    public static class DeviceDataUpdate
    {
        static DeviceDataUpdate()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 左边
            /// </summary>
            public readonly bool isLeft;
             
            /// <summary>
            /// 力量
            /// </summary>
            public readonly float power;
            
            /// <summary>设备数据更新</summary>
            public _EventArgs(bool isLeft,float power,object sender=null)
            {
                this.isLeft=isLeft;
                this.power=power;
            }
            public override bool Notify(object sender=null)
            {
                return DeviceDataUpdate.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 设备数据更新
        /// </summary>
        /// <param name="isLeft">左边</param>
        /// <param name="power">力量</param>
        public static void Notify( bool isLeft, float power, object sender = null)
        {
            var eventData = new _EventArgs(isLeft,power,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 研发工具事件 -> 弹框消息
    /// <summary>弹框消息</summary>
    public static class MsgBox
    {
        static MsgBox()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 内容
            /// </summary>
            public readonly string content;
             
            /// <summary>
            /// 标题
            /// </summary>
            public readonly string title;
             
            /// <summary>
            /// 弹框样式
            /// </summary>
            public readonly int style;
            
            /// <summary>弹框消息</summary>
            public _EventArgs(string content,string title,int style,object sender=null)
            {
                this.content=content;
                this.title=title;
                this.style=style;
            }
            public override bool Notify(object sender=null)
            {
                return MsgBox.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 弹框消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="style">弹框样式</param>
        public static void Notify( string content, string title, int style, object sender = null)
        {
            var eventData = new _EventArgs(content,title,style,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 研发工具事件 -> 气泡消息
    /// <summary>气泡消息</summary>
    public static class MsgBalloon
    {
        static MsgBalloon()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 内容
            /// </summary>
            public readonly string content;
             
            /// <summary>
            /// 标题
            /// </summary>
            public readonly string title;
             
            /// <summary>
            /// 图标<para>0.无</para><para>1.信息</para><para>2.警告</para><para>3.错误</para>
            /// </summary>
            public readonly int icon;
             
            /// <summary>
            /// 持续时间
            /// </summary>
            public readonly float duration;
            
            /// <summary>气泡消息</summary>
            public _EventArgs(string content,string title,int icon,float duration,object sender=null)
            {
                this.content=content;
                this.title=title;
                this.icon=icon;
                this.duration=duration;
            }
            public override bool Notify(object sender=null)
            {
                return MsgBalloon.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 气泡消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="icon">图标<para>0.无</para><para>1.信息</para><para>2.警告</para><para>3.错误</para></param>
        /// <param name="duration">持续时间</param>
        public static void Notify( string content, string title, int icon, float duration, object sender = null)
        {
            var eventData = new _EventArgs(content,title,icon,duration,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
    #region 研发工具事件 -> 打印消息
    /// <summary>打印消息</summary>
    public static class MsgPrint
    {
        static MsgPrint()
        {
            OnDispose+=()=> mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs:CustomEventArgs 
        {
             
            /// <summary>
            /// 内容
            /// </summary>
            public readonly string content;
             
            /// <summary>
            /// 持续时间
            /// </summary>
            public readonly float duration;
            
            /// <summary>打印消息</summary>
            public _EventArgs(string content,float duration,object sender=null)
            {
                this.content=content;
                this.duration=duration;
            }
            public override bool Notify(object sender=null)
            {
                return MsgPrint.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        
        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 打印消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="duration">持续时间</param>
        public static void Notify( string content, float duration, object sender = null)
        {
            var eventData = new _EventArgs(content,duration,sender);
            mEventHandler?.Invoke(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                 try
                 {
                     mEventHandler(sender, eventData);
                 }
                 catch (Exception e)
                 {
                     Debug.LogError(e);
                     return false;
                 }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action,ICancelHandle handle=null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct+=() => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle=null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct+=() => mEventHandler -= temp;
        }
    }
    #endregion
}
