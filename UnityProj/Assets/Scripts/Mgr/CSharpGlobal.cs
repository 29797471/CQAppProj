using CqCore;
using SLua;
using System;
using UnityEngine;

/// <summary>
/// 封装C#的一些静态方法给lua调用
/// </summary>
public static class CSharpGlobal
{
    /// <summary>
    /// 打开一个以http开头的网页
    /// </summary>
    public static bool OpenURL(string url)
    {
        if (url.StartsWith("http"))
        {
            Application.OpenURL(url/*"http://captaincxf.eicp.net"*/);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// 添加托管移除的委托
    /// </summary>
    public static void AddCancelAct(ICancelHandle cancelHandle, Action action)
    {
        if(cancelHandle!=null)
        {
            cancelHandle.CancelAct += action;
        }
    }
}
