using CqCore;
using CustomModel;
using UnityEditor;
using UnityEditorCore;
using UnityEngine;

public class ApkWindow : EditorWindow
{
    
    [MenuItem ("工具/出安卓APK %#&P")]
	public static void Open()
	{
		var x=EditorWindow.GetWindow<ApkWindow>();
        x.Show();
	}
    private void OnEnable()
    {
        if (Application.isPlaying) return;
        titleContent = new GUIContent("出安卓APK");
        
    }
    string[] _appConfigNameList;
    string[] appConfigNameList
    {
        get
        {
            if(_appConfigNameList==null) _appConfigNameList = BuildAsset.Inst.AppConfigNameList;
            return _appConfigNameList;
        }
    }

    void OnGUI()
    {
        GUI.skin = BuildAsset.Inst.buildAndroidSkin;

        if (Application.isPlaying)
        {
            GUILayout.Label("非运行模式下出包");
            return;
        }

        //BuildAsset.Inst.buildAndroidSkin.button = GUI.skin.button;
        //BuildAsset.Inst.buildAndroidSkin.toggle = GUI.skin.toggle;
        BeginWindows();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("打开AB包生成目录"))
        {
            AssetBundleBuilder.OpenFolder();
        }
        if (GUILayout.Button("打开缓存目录"))
        {
            ProcessUtil.OpenFileOrFolderByExplorer(Application.persistentDataPath);
        }
        if (GUILayout.Button("打开资源服版本对应目录"))
        {
            ProcessUtil.OpenFileOrFolderByExplorer(AndroidBuilder.localUpateAppDir);
            //ProcessUtil.OpenFileOrFolderByExplorer(FileOpr.ToAbsolutePath(AndroidBuilder.localUpateAppDir));
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        

        var btn = AndroidBuilder.BuildApk_Desc;
        if (GUILayout.Button(btn))
		{
            ProgressBarRuning.RunBuilder(btn, AndroidBuilder.MakeApk);
        }
        btn = AndroidBuilder.MakeNewRes_Desc;
        if (GUILayout.Button(btn))
		{
            ProgressBarRuning.RunBuilder(btn, AndroidBuilder.MakeNewRes);
        }
        btn = AndroidBuilder.MakeMobileTest_Desc;
        if (GUILayout.Button(btn))
		{
            ProgressBarRuning.RunBuilder(btn, AndroidBuilder.MakeMobileTest);
        }
        
        EditorGUILayout.EndHorizontal();
        //EditorGUILayout.BeginHorizontal();
        

        //EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        
        GUILayout.Label("生成版本配置:" );
        var _index = EditorGUILayout.Popup(BuildAsset.Inst.makeAppConfig, appConfigNameList,
            GUI.skin.GetStyle("popup"), GUILayout.Height(GUI.skin.button.fixedHeight));
        if (_index != BuildAsset.Inst.makeAppConfig)
        {
            BuildAsset.Inst.makeAppConfig = _index;
            var data = Config_AppConfig.List[_index];
            CustomSetting.Inst.updateUrl = data.updateUrl + "/" + data.appId;
            CustomSetting.Inst.clearPersistentData = data.clearPersistentData;
            BuildAsset.Inst.fullRes = data.fullRes;
            BuildAsset.Inst.developModel = data.developModel;
            BuildAsset.Inst.appIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(data.appIcon);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, data.appId);
            PlayerSettings.productName = data.appName;
            PlayerSettingsUtil.SetDefaultIcon(BuildAsset.Inst.appIcon);
            EditorUtility.SetDirty(CustomSetting.Inst);
            EditorUtility.SetDirty(BuildAsset.Inst);
            AndroidBuilder.MakeAndroidManifest();
            AndroidBuilder.VersionInfo = null;
            PlayerSettings.bundleVersion = AndroidBuilder.VersionInfo.appVersion;
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        var fullRes = GUILayout.Toggle(BuildAsset.Inst.fullRes, "包含完整资源");
        if (fullRes != BuildAsset.Inst.fullRes)
        {
            BuildAsset.Inst.fullRes = fullRes;
            EditorUtility.SetDirty(BuildAsset.Inst);
        }
        var debugModel = GUILayout.Toggle(BuildAsset.Inst.developModel, "开发模式");
        if (debugModel != BuildAsset.Inst.developModel)
        {
            BuildAsset.Inst.developModel = debugModel;
            EditorUtility.SetDirty(BuildAsset.Inst);
        }

        var clearPersistentData = GUILayout.Toggle(CustomSetting.Inst.clearPersistentData, "覆盖安装时清空App数据目录");
        if (clearPersistentData != CustomSetting.Inst.clearPersistentData)
        {
            CustomSetting.Inst.clearPersistentData = clearPersistentData;
            EditorUtility.SetDirty(CustomSetting.Inst);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("app版本号:" + AndroidBuilder.VersionInfo.appVersion);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("res版本号:" + AndroidBuilder.VersionInfo.resVersion);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("热更新地址:" + CustomSetting.Inst.updateUrl);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("应用名称:" + PlayerSettings.productName);
        GUILayout.Label("应用Id:" + Application.identifier);
        EditorGUILayout.ObjectField("图标:", BuildAsset.Inst.appIcon, typeof(Texture2D), true);
        EditorGUILayout.EndHorizontal();
        
        EndWindows();
	}

    
    
    /// <summary>
    /// 当窗口获得焦点时调用一次
    /// </summary>
    void OnFocus()
	{
	}
	/// <summary>
	/// 当窗口丢失焦点时调用一次
	/// </summary>
	void OnLostFocus()
	{
	}
	/// <summary>
	/// 当Hierarchy视图中的任何对象发生改变时调用一次
	 /// </summary>
	void OnHierarchyChange()
	{
	}
	/// <summary>
	/// 当Project视图中的资源发生改变时调用一次
	/// </summary>
	void OnProjectChange()
	{
	}
	/// <summary>
	/// 窗口面板的更新
	/// 这里开启窗口的重绘，不然窗口信息不会刷新
	/// </summary>
	void OnInspectorUpdate()
	{
	}
	/// <summary>
	/// 当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
	/// 有可能是多选，
	/// </summary>
	void OnSelectionChange()
	{
	}
	/// <summary>
	/// 当窗口关闭时调用
	/// </summary>
	void OnDestroy()
	{
	}

}