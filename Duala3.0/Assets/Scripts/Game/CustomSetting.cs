using CqCore;
using System;
using System.Linq;
using UnityCore;
using UnityEngine;


/// <summary>
/// 服务器信息
/// </summary>
public class ServerInfoAttribute : Attribute
{
    public int httpId;
    public int resId;
    public ServerInfoAttribute(int httpId,int resId)
    {
        this.httpId = httpId;
        this.resId = resId;
    }
}
public enum ClientTypeEnum
{
    [ServerInfo(0,0)]
    [EnumLabel("开发内测端")]
    LocalDevelop,

    [ServerInfo(1, 1)]
    [EnumLabel("开发外测端")]
    NetDevelop,

    [ServerInfo(2, 2)]
    [EnumLabel("正式端")]
    Online,
}

public enum DownloadStyle
{
    WWW,
    Ftp
}

/// <summary>
/// 定义一些不常改的,不受研发项目需求影响的核心数据.
/// 这些数据通常和app版本关联
/// </summary>
[Serializable]
[CreateAssetMenu]
public class CustomSetting : ScriptableObject
{
    static CustomSetting mInst;
    public static CustomSetting Inst
    {
        get
        {
            if (mInst == null)
            {
                mInst = Resources.Load<CustomSetting>("CustomSetting");
            }
            return mInst;
        }
    }

    [TextBox("说明:",true),IsEnabled(false),Height(40)]
    public string desc1 = "定义一些不常改的,不受研发项目需求影响的核心数据,\n这些数据运行时也会用到";

    [TextBox("预设目录")]
    public string prefabDir;

    /// <summary>
    /// 预设目录(用于Assetbundle生成)
    /// </summary>
    public string PrefabFullPath
    {
        get
        {
            return Application.dataPath + "/" + prefabDir;
        }
    }

    [TextBox("UIRoot")]
    public string uiRootPath;

    [TextBox("缓存资源路径")]//cachRes
    public string cachResPath;

    public string cachLuaFolder
    {
        get
        {
            return cachResPath + "/" + files + "/" + luaFolderName;
        }
    }

    public string luaFolderName
    {
        get
        {
            return FileOpr.GetNameByShort(luaFolder);
        }
    }

    [TextBox("版本目录")]//files
    public string files;

    [TextBox("本地Lua文件夹")]
    public string luaFolder;

    [TextBox("热更新地址")]
    public string updateUrl;

    [CheckBox("覆盖安装时清空App数据目录")]
    public bool clearPersistentData;


    [CheckBox("记录协程堆栈")]
    public bool recordStacktrace = false;

    /*
     * [TextBox("应用程序下载地址")]
    public string appDownloadUrl;
     * 应用程序下载地址调整为动态读取热更新目录info信息获取最新的安装包名.
     */
}
