using SLua;
using System;

/// <summary>
/// 封装lua的一些管理类的方法给C#调用
/// </summary>
public class LuaGlobal : Singleton<LuaGlobal>
{

    LuaFunction _TextMgr_SetCallBack;

    /// <summary>
    /// 获取文本转译表的文本
    /// </summary>
    public void TextMgr_SetCallBack(LanguageText lt)
    {
        if (_TextMgr_SetCallBack == null)
        {
            _TextMgr_SetCallBack = LuaMgr.instance.MainState.getFunction("TextMgr.SetCallBack");
        }
        _TextMgr_SetCallBack.call(lt);
    }



    LuaFunction TextMgr_GetText_LuaFunction;
    
    /// <summary>
    /// 获取文本转译表的文本
    /// </summary>
    public string TextMgr_GetText(int id, int flag,LuaTable args)
    {
        if (TextMgr_GetText_LuaFunction == null)
        {
            TextMgr_GetText_LuaFunction = LuaMgr.instance.MainState.getFunction("TextMgr.GetText");
        }
        return (string)TextMgr_GetText_LuaFunction.call(id, flag, args);
    }


    LuaFunction CommonWinMgr_Open_LuaFunction;
    public void CommonWinMgr_Open(int messageId,Action<int> callbackFunc,int flag,LuaTable args)
    {
        if (CommonWinMgr_Open_LuaFunction == null)
        {
            CommonWinMgr_Open_LuaFunction = LuaMgr.instance.MainState.getFunction("CommonWinMgr.Open");
        }

        CommonWinMgr_Open_LuaFunction.call(messageId, callbackFunc, flag,args);
    }

    LuaFunction CommonWinMgr_DebugOpen_LuaFunction;
    public void CommonWinMgr_DebugOpen(string content)
    {
        if (CommonWinMgr_DebugOpen_LuaFunction == null)
        {
            CommonWinMgr_DebugOpen_LuaFunction = LuaMgr.instance.MainState.getFunction("CommonWinMgr.DebugOpen");
        }

        CommonWinMgr_DebugOpen_LuaFunction.call(content);
    }

    LuaFunction table_SerializeValue;
    public string GetJsonByLuaTable(LuaTable tbl)
    {
        if(table_SerializeValue==null)
        {
            table_SerializeValue= LuaMgr.instance.MainState.getFunction("table.SerializeValue");
        }
        return (string)table_SerializeValue.call(tbl);
    }

    LuaFunction CreateWindow;
    public LuaTable CreateWindowTbl(string luaPath)
    {
        if (CreateWindow == null)
        {
            CreateWindow = LuaMgr.instance.MainState.getFunction("CreateWindow");
        }
        return (LuaTable)CreateWindow.call(luaPath);
    }
}
