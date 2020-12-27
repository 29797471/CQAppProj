using CqCore;
using CustomModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorCore;
using UnityEngine;

public static class AndroidBuilder
{
    static FileVersionMgr.ResVersionInfo mVersionInfo;
    /// <summary>
    /// 对应资源服版本目录中的版本信息
    /// </summary>
    public static FileVersionMgr.ResVersionInfo VersionInfo
    {
        set
        {
            mVersionInfo = value;
        }
        get
        {
            if (mVersionInfo == null)
            {
                var AppConfigInfoFile = Application.dataPath + "/" +
                BuildAsset.Inst.serverAssets + "/" + Application.identifier + "/" + FileVersionMgr.infoFile;
                if (FileOpr.Exists(AppConfigInfoFile))
                {
                    var dat = FileOpr.ReadFile(AppConfigInfoFile);
                    mVersionInfo = Torsion.Deserialize<FileVersionMgr.ResVersionInfo>(dat);
                }
                else
                {
                    var version = TimeUtil.NowVersion3;
                    mVersionInfo = new FileVersionMgr.ResVersionInfo()
                    {
                        appVersion = version,
                        resVersion = version,
                    };
                }
            }
            return mVersionInfo;
        }
    }

    public const string BuildApk_Desc = "发布热更新资源并生成apk";
    public const string MakeNewRes_Desc = "发布热更新资源";
    public const string MakeMobileTest_Desc = "构建移动测试环境";

    /// <summary>
    /// 电脑端lua路径
    /// </summary>
    public static string LuaDir
    {
        get
        {
            return Application.dataPath + @"\..\" + CustomSetting.Inst.luaFolder;
        }
    }

    /// <summary>
    /// 应用程序资源目录
    /// </summary>
    public static string StreamingAssetsCachResDir
    {
        get
        {
            return Application.streamingAssetsPath + "/" + CustomSetting.Inst.cachResPath;
        }
    }

    /// <summary>
    /// 资源服下对应的版本目录
    /// </summary>
    public static string localUpateAppDir
    {
        get
        {
            return Application.dataPath + "/" + BuildAsset.Inst.serverAssets + "/" + Application.identifier;
        }
    }

    /// <summary>
    /// 资源服下对应版本的Files目录
    /// </summary>
    public static string localUpdateFilesDir
    {
        get
        {
            return localUpateAppDir + "/"+CustomSetting.Inst.files;
        }
    }


    /// <summary>
    /// 沙盒资源目录
    /// </summary>
    public static string cachResDir
    {
        get
        {
            return Application.streamingAssetsPath + "/" + CustomSetting.Inst.cachResPath;
        }
    }

    /// <summary>
    /// 沙盒Assets目录
    /// </summary>
    public static string cachResAssetsDir
    {
        get
        {
            return cachResDir +"/" + CustomSetting.Inst.files;
        }
    }

    /// <summary>
    /// apk生成路径
    /// </summary>
    public static string apkPath
    {
        get
        {
            return localUpateAppDir + "/" + BuildAsset.Inst.appFileName;
        }
    }


    /// <summary>
    /// 拷贝资源到临时目录生成资源版本信息和文件列表清单
    /// </summary>
    static IEnumerator CopyToServerAssetsAndMakeAll(ProgressBarData data,float weight)
    {
        data.info = "拷贝资源文件到临时目录.."; yield return null;
        DirOpr.ClearOrCreate(localUpdateFilesDir);
        DirOpr.Copy(Application.dataPath + "/../" + CustomSetting.Inst.luaFolder, localUpdateFilesDir);
        DirOpr.Copy(BuildAsset.Inst.assetBundleFullPath, localUpdateFilesDir,true, ".manifest");

        data.progress += weight / 2; 

        data.info = "生成发布资源信息清单..";  yield return null;
        FileVersionMgr.MakeInfoAndMd5File(
            localUpdateFilesDir, VersionInfo.appVersion, VersionInfo.resVersion, BuildAsset.Inst.appFileName,
            (path,info) =>
            //动态更新的策略:(动态更新的清单,启动必定要下载,所以不宜过大)
            //1.资源文件超过500k的放在启动进度更新
            //2.lua启动更新
            //3.AssetBundles清单启动更新
            {
                if(info.size>500*1024)return false;//让一些比较大的资源文件一开始就更新好.
                if (path.EndsWith(".lua")) return false;
                if (path.EndsWith("AssetBundles")) return false;
                return true;
            });
        data.progress += weight / 2;
        //return () => FileOpr.DeleteFolder(makePath.tempDir);
    }

    /// <summary>
    /// 拷贝临时资源到打包目录
    /// </summary>
    static IEnumerator CopyFilesToStreamAssets(ProgressBarData data,float weight)
    {
        data.info = "生成出包资源"; yield return null;

        DirOpr.Delete(cachResDir);

        if(BuildAsset.Inst.fullRes)
        {
            //拷贝整个热更新目录到沙河目录
            DirOpr.Copy(localUpateAppDir + @"\", cachResDir);
        }
        else
        {
            //拷贝lua代码到沙河目录,并单独生成清单
            DirOpr.Copy(localUpateAppDir + @"\" + CustomSetting.Inst.files + "/" + CustomSetting.Inst.luaFolderName, cachResAssetsDir);

            var packageResources = new HashSet<string>();//随包资源总单
            
            foreach(var it in BuildAsset.Inst.packageResources)
            {
                var path=AssetDatabase.GetAssetPath(it);
                var files = AssetDatabase.GetDependencies(path, true);//递归包含所有关联资源

                foreach(var file in files)
                {
                    if (file.EndsWith(".cs") || file.EndsWith(".dll")) continue;
                    var pathWithOutAssets = FileOpr.ToRelativePath(file, "Assets/");
                    pathWithOutAssets = pathWithOutAssets.Replace("\\", "/");
                    packageResources.Add(pathWithOutAssets.ToLower() + ".bundle");
                }
            }
            
            packageResources.Add("AssetBundles");
            foreach (var file in packageResources)
            {
                //随包文件的依赖的资源ab包并不一定存在,当它只被这个随包文件依赖时,它直接打包在这个资源中了.
                if(FileOpr.Exists(localUpdateFilesDir + "/AssetBundles/" + file))
                {
                    FileOpr.Copy(localUpdateFilesDir + "/AssetBundles/" + file, cachResAssetsDir + "/AssetBundles/" + file);
                }
            }
            FileVersionMgr.MakeInfoAndMd5File(
                cachResAssetsDir, VersionInfo.appVersion, "",BuildAsset.Inst.appFileName);
        }

        //lua 需要同步读取,所以需要做成压缩包,在移动端解压到用户目录后可以同步读取.
        //(Android移动端同步读取SteamAssets下非assetbundle文件只有通过WWW,isDone锁死循环的方式,相当恶心)
        var luaDir = cachResAssetsDir + "/"+ CustomSetting.Inst.luaFolderName;

        ZipHelper.ZipDir(luaDir, luaDir + ".zip");
        DirOpr.Delete(luaDir);

        data.progress += weight;
        //return () => FileOpr.DeleteFolder(makePath.cachResDir);
    }

    public static IEnumerator MakeApk(ProgressBarData data)
    {
        data.info = "更新app和res版本号"; yield return null;
        var version= TimeUtil.NowVersion3;
        VersionInfo.appVersion = version;
        VersionInfo.resVersion = version;
        PlayerSettings.bundleVersion = version;
        data.progress = 0.1f;
        yield return AssetBundleBuilder.BuildAssetBundle_IT(data, 0.2f);

        yield return CopyToServerAssetsAndMakeAll(data, 0.1f);

        yield return CopyFilesToStreamAssets(data, 0.3f);

        yield return BuildPlayer(data, 0.3f);
    }

    public static IEnumerator BuildPlayer(ProgressBarData data,float weight)
    {
        data.info = "生成apk"; yield return null;
        var options = BuildOptions.None;
        if (BuildAsset.Inst.developModel)
        {
            options |= BuildOptions.AllowDebugging | BuildOptions.Development;
        }
        var report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, apkPath, EditorUserBuildSettings.activeBuildTarget, options);
        var bl = (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded);
        data.progress += weight;
    }

    public static IEnumerator MakeNewRes(ProgressBarData data)
    {
        data.info = "更新res版本号"; yield return null;
        var version = TimeUtil.NowVersion3;
        VersionInfo.resVersion = version;
        data.progress = 0.1f;
        yield return AssetBundleBuilder.BuildAssetBundle_IT(data, 0.2f);

        yield return CopyToServerAssetsAndMakeAll(data,0.7f);

        //yield return UploadTempDirToServer(ref info, ref progress, makePath, false);

    }

    /// <summary>
    /// 1.清除移动测试目录,和缓存配置
    /// 2.生成StreamAssets资源
    /// </summary>
    public static IEnumerator MakeMobileTest(ProgressBarData data)
    {
        data.info = "清除用户目录和缓存"; yield return null;
        ApplicationUtil.runByMobileDevice = true;
        SettingModel.instance.Clear();
        DirOpr.Delete(ApplicationUtil.persistentDataPath);
        data.progress = 0f;
        //yield return AssetBundleBuilder.BuildAssetBundle_IT(data, 0.2f);
        //yield return CopyToServerAssetsAndMakeAll(data,0.2f);
        yield return CopyFilesToStreamAssets(data, 1f);

        //FileOpr.DeleteFolder(tempDir);
        AssetDatabase.Refresh();
    }

    public static void MakeAndroidManifest()
    {
        var AndroidManifest = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
        if (File.Exists(AndroidManifest+".temp"))
        {
            var content = File.ReadAllText(AndroidManifest + ".temp");
            File.WriteAllText(AndroidManifest,string.Format(content, Application.identifier));
        }
    }
}

