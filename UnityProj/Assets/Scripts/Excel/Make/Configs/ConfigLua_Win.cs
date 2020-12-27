using UnityEngine;
using System.Collections.Generic;
using SLua;

/// <summary>
/// 窗口.xlsx - 窗口表
/// </summary>
public static class Config_Win
{
    static List<DataItem> mList;
    public static List<DataItem> List
    {
        get
        {
            if (mList == null)
            {
                if(!Application.isPlaying)LuaMgr.instance.require("Excel/More/Config_Win");
                var x = (string)LuaMgr.instance.MainState.getFunction("ConfigData_Win_GetTorsion").call();
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
                    mDic[it.winName] = it;
                }
                LuaSvr.OnDispose += () => mDic = null;
            }
            return mDic;
        }
    }
    /// <summary>
    /// 窗口.xlsx - 窗口表
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// 窗口
        /// </summary>
        public string winName;
        /// <summary>
        /// 备注
        /// </summary>
        public string desc;
        /// <summary>
        /// 窗口名语言id
        /// </summary>
        public int nameId;
        /// <summary>
        /// 独立窗口
        /// </summary>
        public bool isAlone;
        /// <summary>
        /// 显示层级
        /// </summary>
        public string layer;
        /// <summary>
        /// 打开方式
        /// </summary>
        public int tweenStyle;
        /// <summary>
        /// 关闭时销毁
        /// </summary>
        public bool destroyByClose;
        /// <summary>
        /// 预设
        /// </summary>
        public string prefab;
        /// <summary>
        /// 代码文件
        /// </summary>
        public string code;
    }
}
