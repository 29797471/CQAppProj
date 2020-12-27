using UnityEngine;
using System.Collections.Generic;
using SLua;

/// <summary>
/// 应用生成配置.xlsx - 应用生成配置
/// </summary>
public static class Config_AppConfig
{
    static List<DataItem> mList;
    public static List<DataItem> List
    {
        get
        {
            if (mList == null)
            {
                if(!Application.isPlaying)LuaMgr.instance.require("Excel/More/Config_AppConfig");
                var x = (string)LuaMgr.instance.MainState.getFunction("ConfigData_AppConfig_GetTorsion").call();
                mList = Torsion.Deserialize<List<DataItem>>(x);
                LuaSvr.OnDispose += () => mList = null;
            }
            return mList;
        }
    }
    static Dictionary<string, Dictionary<string, DataItem>> mDic;
    public static Dictionary<string, Dictionary<string, DataItem>> Dic
    {
        get
        {
            if (mDic == null)
            {
                var luaList = List;
                mDic = new Dictionary<string, Dictionary<string, DataItem>>();

                foreach (var it in luaList)
                {
                    if(!mDic.ContainsKey(it.configName))
                    {
                        mDic[it.configName] = new Dictionary<string, DataItem>();
                    }
                    mDic[it.configName][it.updateUrl] = it;
                }
            }
            return mDic;
        }
    }
    /// <summary>
    /// 应用生成配置.xlsx - 应用生成配置
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// 生成配置样式
        /// </summary>
        public string configName;
        /// <summary>
        /// 热更新地址
        /// </summary>
        public string updateUrl;
        /// <summary>
        /// 应用Id
        /// </summary>
        public string appId;
        /// <summary>
        /// 包含完整资源
        /// </summary>
        public bool fullRes;
        /// <summary>
        /// App名称
        /// </summary>
        public string appName;
        /// <summary>
        /// 开发模式
        /// </summary>
        public bool developModel;
        /// <summary>
        /// 应用图标
        /// </summary>
        public string appIcon;
        /// <summary>
        /// 覆盖安装时清空App缓存目录
        /// </summary>
        public bool clearPersistentData;
    }
}
