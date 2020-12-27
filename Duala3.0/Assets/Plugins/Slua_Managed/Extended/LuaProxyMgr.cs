using CqCore;
using SLua;
using System;
using System.ComponentModel;

/// <summary>
/// 提供对lua对象属性改变的监听接口
/// </summary>
public partial class LuaProxyMgr : Singleton<LuaProxyMgr>
{
    LuaFunction Proxy_AddChanged_LuaFunction;

    /// <summary>
    /// 监听一个lua字典中的属性改变
    /// </summary>
    public void AddMemberChanged(LuaTable tbl, Action<string> OnChanged, ICancelHandle cancelHandle = null)
    {
        if (Proxy_AddChanged_LuaFunction == null)
        {
            Proxy_AddChanged_LuaFunction = LuaSvr.mainState.getFunction("Proxy.AddChanged");
        }
        Proxy_AddChanged_LuaFunction.call(tbl, OnChanged, cancelHandle);
    }

    LuaFunction Proxy_AddListChanged_LuaFunction;

    /// <summary>
    /// 监听一个lua列表的改变
    /// </summary>
    public void AddListChanged(LuaTable tbl, Action<ListChangedType, int,int> OnChanged,ICancelHandle cancelHandle=null)
    {
        if (Proxy_AddListChanged_LuaFunction == null)
        {
            Proxy_AddListChanged_LuaFunction = LuaSvr.mainState.getFunction("Proxy.AddListChanged");
        }
        var removeAction = (LuaFunction)Proxy_AddListChanged_LuaFunction.call(tbl, OnChanged);
        if (cancelHandle != null)
        {
            cancelHandle.CancelAct += () =>
            {
                removeAction.call();
            };
        }
    }

    LuaFunction Proxy_SetCallBack_LuaFunction;

    /// <summary>
    /// 注入一个c#回调函数到lua字典的一个成员函数对象
    /// </summary>
    public void SetCallBack(LuaTable tbl,string luaMember, Delegate callBack)
    {
        if (Proxy_SetCallBack_LuaFunction == null)
        {
            Proxy_SetCallBack_LuaFunction = LuaSvr.mainState.getFunction("Proxy.SetCallBack");
        }
        Proxy_SetCallBack_LuaFunction.call(tbl, luaMember, callBack);
    }
}
