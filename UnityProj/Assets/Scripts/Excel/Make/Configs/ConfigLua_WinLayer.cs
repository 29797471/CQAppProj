using UnityEngine;
using System.Collections.Generic;
using SLua;

/// <summary>
/// 窗口.xlsx - 显示层级表
/// </summary>
public static class Config_WinLayer
{
    static List<DataItem> mList;
    public static List<DataItem> List
    {
        get
        {
            if (mList == null)
            {
                if(!Application.isPlaying)LuaMgr.instance.require("Excel/More/Config_WinLayer");
                var x = (string)LuaMgr.instance.MainState.getFunction("ConfigData_WinLayer_GetTorsion").call();
                mList = Torsion.Deserialize<List<DataItem>>(x);
                LuaSvr.OnDispose += () => mList = null;
            }
            return mList;
        }
    }
    static Dictionary<string, DataItem> mDic;
    public static Dictionary<string,DataItem> Dic
    {
        get
        {
            if(mDic==null)
            {
                var luaList = List;
                mDic = new Dictionary<string, DataItem>();
                
                foreach (var it in luaList)
                {
                    mDic[it.layer] = it;
                }
                LuaSvr.OnDispose += () => mDic = null;
            }
            return mDic;
        }
    }
    /// <summary>
    /// 窗口.xlsx - 显示层级表
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// 层级
        /// </summary>
        public string layer;
        /// <summary>
        /// 优先级
        /// </summary>
        public int priority;
    }
}
