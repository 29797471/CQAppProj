using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using CqCore;
using System.Collections;
using System.Linq;
using UnityEditorCore;

/// <summary>
/// 资源包生成器
/// </summary>
public class AssetBundleBuilder : Editor
{
    public class AssetNode
    {
        public List<AssetNode> parents = new List<AssetNode>();
        public List<AssetNode> childs = new List<AssetNode>();
        /// <summary>
        /// 通过AssetDatabase.GetDependencies获取到的以Assets起始的路径
        /// </summary>
        public string path;

        public float pri;

        public bool package;

        public AssetBundleBuild ToAssetBundleBuild()
        {
            var abb = new AssetBundleBuild();
            abb.assetBundleName = FileOpr.ToRelativePath(path,"Assets/")+".bundle";
            //abb.assetBundleName = path + ".bundle";
            var list = new List<string>();
            CheckAddPath(list);
            abb.assetNames = list.ToArray();
            return abb;
        }

        public void CheckAddPath(List<string> list)
        {
            list.Add(path);
            foreach (var child in childs)
            {
                if (!child.package)
                {
                    child.CheckAddPath(list);
                }
            }
        }

        /// <summary>
        /// 构造依赖树<para/>
        /// 当一个资源被多个资源依赖时独立打包
        /// </summary>
        /// <param name="GetNode"></param>
        /// <param name="packageByCount">颗粒度</param>
        public void CheckDependencies(System.Func<string,AssetNode> GetNode,int packageByCount=2)
        {
            var dependcy = AssetDatabase.GetDependencies(path, false);
            foreach (var it in dependcy)
            {
                if (IgnoreFileByExtension(it)) continue;
                var depNode = GetNode(it);
                depNode.pri = Mathf.Max(depNode.pri, pri + 1);
                depNode.parents.Add(this);
                childs.Add(depNode);
                if(!depNode.package && depNode.parents.Count>= packageByCount)
                {
                    depNode.package = true;
                }
                depNode.CheckDependencies(GetNode, packageByCount);
            }
        }
    }
    const string AssetsMenu = "Assets/AB包";
    [MenuItem(AssetsMenu + "/检查bundle资源包")]
    static void CheckAssetBundle()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!path.Contains(".bundle")) return;

        GlobalCoroutine.Start(CheckBundleAll(path));
    }
    static IEnumerator CheckBundleAll(string path)
    {
        Debug.Log("检查:" + path);
        var abcq = AssetBundle.LoadFromFileAsync(path);
        while(abcq.isDone==false)
        {
            yield return null;
        }
        var ab = abcq.assetBundle;
        var abq = ab.LoadAllAssetsAsync();
        while (abq.isDone == false)
        {
            yield return null;
        }
        var objs = abq.allAssets;
        foreach (var it in objs)
        {
            Debug.Log(it.ToString());
        }

        ab.Unload(false);

        EditorUtility.DisplayDialog("提示", Torsion.Serialize(objs.ToList().ConvertAll(x=>x.ToString())), "确定");
    }

    [MenuItem(AssetsMenu + "/打开目录")]
    public static void OpenFolder()
    {
        ProcessUtil.OpenFileOrFolderByExplorer(BuildAsset.Inst.assetBundleFullPath);
    }
    [MenuItem(AssetsMenu + "/清空目录")]
    public static void ClearBuild()
    {
        DirOpr.ClearOrCreate(BuildAsset.Inst.assetBundleFullPath);
    }

    /// <summary>
    /// 策略,资源被多个其它资源依赖时需要独立打包,且在这些资源之前,当资源没有依赖时也需要独立打包
    /// </summary>
    [MenuItem(AssetsMenu + "/增量生成AB包")]
    public static void BuildAssetBundle()
    {
        GlobalCoroutine.Start(BuildAssetBundle_IT(null,0));
    }
    public static IEnumerator BuildAssetBundle_IT(ProgressBarData data,float weight)
    {
        if(data!=null)
        {
            data.info = "构建AB包依赖树"; yield return null;
        }
        var dicAssets = new Dictionary<string, AssetNode>();
        var files = Directory.GetFiles(CustomSetting.Inst.PrefabFullPath, "*.prefab", SearchOption.AllDirectories);


        System.Func<string, AssetNode> GetNode = (assetPath) =>
        {
            AssetNode root = null;
            dicAssets.TryGetValue(assetPath, out root);
            if (root == null)
            {
                root = new AssetNode();
                root.path = assetPath;
                dicAssets[assetPath] = root;
            }
            return root;
        };

        for (int j = 0; j < files.Length; j++)
        {
            var assetPath = FileOpr.ToRelativePath(files[j], ApplicationUtil.ProjPath).ReplaceAll("\\", "/");
            var node = GetNode(assetPath);
            node.package = true;
            node.CheckDependencies(GetNode);
        }

        var list = dicAssets.Values.ToList().FindAll(x => x.package);
        list.Sort(x => -x.pri);

        var opt = BuildAssetBundleOptions.None;
        opt |= BuildAssetBundleOptions.ChunkBasedCompression;
        opt |= BuildAssetBundleOptions.DeterministicAssetBundle;//使每个Object具有唯一的,不变的hashID,便于后续查找,可用于增量式发布AB包
        var xxx = list.ConvertAll(x => x.path);
        if (!Directory.Exists(BuildAsset.Inst.assetBundleFullPath))
        {
            Directory.CreateDirectory(BuildAsset.Inst.assetBundleFullPath);
        }

        var ary = list.ConvertAll(x => x.ToAssetBundleBuild()).ToArray();
        if (data != null)
        {
            data.info = "增量生成AB包"; yield return null;
        }
        BuildPipeline.BuildAssetBundles(BuildAsset.Inst.assetBundleFullPath,ary , opt, EditorUserBuildSettings.activeBuildTarget);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        Debug.Log("生成AB包完成");
        if (data != null)
        {
            data.progress += weight; yield return null;
        }
    }

    
    [MenuItem(AssetsMenu + "/选中预设递归列出依赖资源")]
    public static void TestDependenciesRecursive()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var ary = AssetDatabase.GetDependencies(path, true);
        //EditorUtility.CollectDependencies
        EditorUtility.DisplayDialog("提示", Torsion.Serialize(ary), "确定");
    }

    [MenuItem(AssetsMenu + "/选中预设列出下级依赖资源")]
    public static void TestDependencies()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        var ary = AssetDatabase.GetDependencies(path, false);
        EditorUtility.DisplayDialog("提示", Torsion.Serialize(ary), "确定");
    }

    static Dictionary<string, bool> ignoreDic;

    /// <summary>
    /// 通过文件扩展名忽略
    /// </summary>
    static bool IgnoreFileByExtension(string fileName)
    {
        var ext = FileOpr.GetNameByExtension(fileName);
        if(ignoreDic==null)
        {
            ignoreDic = new Dictionary<string, bool>();
            var ignoreList = new string[] { ".cs", ".DS_Store", ".dll" };
            foreach(var it in ignoreList)
            {
                ignoreDic[it] = true;
            }
        }
        bool ignore;
        ignoreDic.TryGetValue(ext, out ignore);
        return ignore;
    }
}