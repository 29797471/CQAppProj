using System;
using System.Linq;
using UnityCore;
using UnityEngine;

/// <summary>
/// 定义出版本相关参数
/// </summary>
[Serializable]
[CreateAssetMenu]
public class BuildAsset : ScriptableObject
{
    static BuildAsset mInst;
    public static BuildAsset Inst
    {
        get
        {
            if (mInst == null)
            {
                mInst = Resources.Load<BuildAsset>("BuildAsset");
            }
            return mInst;
        }
    }
    [ComBox("生成版本配置", ComBoxStyle.RadioBox), Items("AppConfigNameList"), IsEnabled(false)]
    public int makeAppConfig;

    public string[] AppConfigNameList
    {
        get
        {
            return Config_AppConfig.Dic.Keys.ToArray();
        }
    }

    /*不再自定义apk文件名,以应用安装后的名称作为apk名称
    [TextBox("apk")]
    public string apkName = "duola.apk";
    */
    public string appFileName
    {
        get
        {
            return UnityEditor.PlayerSettings.productName+".apk";
        }
    }

    [TextBox("本地资源服根目录")]
    public string serverAssets ="../ServerAssets";

    

    [TextBox("ab包出包目录")]
    public string assetBundleDir = "../AssetBundles";

    /// <summary>
    /// ab包目录
    /// </summary>
    public string assetBundleFullPath { get {return Application.dataPath + "/" + assetBundleDir; } }

    /// <summary>
    /// 包含完整资源
    /// </summary>
    [CheckBox("包含完整资源")]
    public bool fullRes;

    /// <summary>
    /// 开发模式下出版本包含BuildOptions.AllowDebugging | BuildOptions.Development
    /// </summary>
    [CheckBox("开发模式")]
    public bool developModel;

    [Header("随包资源列表"), Tooltip("生成应用程序必须包含的资源清单,比如更新UI,更新弹窗提示")]
    public UnityEngine.Object[] packageResources;

    [Header("APP图标")]
    public Texture2D appIcon;

    public GUISkin buildAndroidSkin;
}
