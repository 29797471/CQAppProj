using CqCore;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

/// <summary>
/// 资源策略
/// 1.使用一个相对路径去访问资源
/// a.文本文件返回string
/// b.assetbundle返回object
/// c.图片返回Texture2D
/// 2.保证资源不重复加载（a.如果已经加载完成则直接拿缓存 b.如果正在加载则添加加载完成的回调 c.开始加载）
/// 3.加载时先去本地找,如果没有或者本地文件比较旧时就从网络下载下到本地,下载后更新文件版本,再从本地加载到内存
/// </summary>
public static class ResMgr
{
    /// <summary>
    /// 异步读取AB包
    /// </summary>
    public static bool readAssetBundleAsync=true;

    static AssetBundleManifest manifest;

    public static IEnumerator Init_It()
    {
        if (GlobalMgr.instance.isMobilePlatform)
        {
            if (manifest == null)
            {
                var bundle = new AsyncReturn<AssetBundle>();
                yield return LoadBundleBase("AssetBundles/AssetBundles", false, bundle);
                manifest = bundle.data.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                if (manifest == null)
                {
                    Debug.LogError("读manifest失败");
                }
            }
        }
    }

    public static void Instantiate(string relativeName, Action<GameObject> OnLoad, string newName = null,
        Transform parent = null, ICancelHandle handle = null, bool destroyImmediate = true)
    {
        AsyncReturn<GameObject> returnObj = new AsyncReturn<GameObject>();
        GlobalCoroutine.Start(Instantiate_IT(relativeName, returnObj, newName, parent, handle), handle, () =>
        {
            returnObj.data.SetActive(true);
            OnLoad(returnObj.data);
            if (handle != null)
            {
                handle.CancelAct += () =>
                {
                    if (destroyImmediate) UnityEngine.Object.DestroyImmediate(returnObj.data);
                    else UnityEngine.Object.Destroy(returnObj.data);
                };
            }
        });
    }


    /// <summary>
    /// 加载预设并实例化<para/>
    /// 在移除句柄中回调DestroyImmediate立即删除
    /// </summary>
    public static IEnumerator Instantiate_IT(string relativeName, AsyncReturn<GameObject> returnObj, string newName = null,
        Transform parent = null,ICancelHandle handle=null)
    {
        var unloadAB = new CancelHandle();
        if (handle != null)
        {
            handle.CancelAct += unloadAB.CancelAll;
        }
        var returnPrefab = new AsyncReturn<GameObject>();
        yield return LoadAsset(relativeName + ".prefab", returnPrefab, unloadAB);
        if (handle != null)
        {
            handle.CancelAct -= unloadAB.CancelAll;
        }
        var activeSelf = returnPrefab.data.activeSelf;
        if(activeSelf) returnPrefab.data.SetActive(false);
        returnObj.data = returnPrefab.data.Clone(newName, parent);
        if(activeSelf) returnPrefab.data.SetActive(true);
        GlobalCoroutine.DelayCall(1, unloadAB.CancelAll);
        //unloadAB.CancelAll();
    }

    public static void LoadAsset(string relativePath, Action<GameObject> OnLoad, ICancelHandle handle = null)
    {
        AsyncReturn<GameObject> returnObj = new AsyncReturn<GameObject>();
        GlobalCoroutine.Start(LoadAsset(relativePath, returnObj, handle), handle, () =>
        {
            OnLoad(returnObj.data);
        });
    }


    /// <summary>
    /// 下载最新资源到本地后加载
    /// </summary>
    /// <param name="relativePath"></param>
    /// <param name="OnLoad"></param>
    /// <returns></returns>
    public static IEnumerator LoadAsset(string relativePath, AsyncReturn<GameObject> returnObj, ICancelHandle unloadAB = null)
    {
        var prefabPath = CustomSetting.Inst.prefabDir + "/" + relativePath;
        if (GlobalMgr.instance.isMobilePlatform )
        {
            var bundlePath = "AssetBundles/" + (prefabPath + ".bundle").ToLower();
            if (manifest == null)
            {
                throw new Exception("没有 manifest");
            }
            
            AsyncReturn<AssetBundle> returnData = new AsyncReturn<AssetBundle>();

            yield return LoadBundleBase(bundlePath, true, returnData, unloadAB);
            //加载依赖
            var dependencies = manifest.GetAllDependencies(returnData.data.name);

            if (dependencies.Length != 0)
            {
                foreach (string s in dependencies)
                {
                    //Debug.Log("加载依赖" + s);
                    yield return LoadBundleBase("AssetBundles/" + s, true, null, unloadAB);
                }
            }

            returnObj.data=returnData.data.LoadAsset<GameObject>("Assets/" + prefabPath);
        }
        else
        {
            GameObject prefab = null;
#if UNITY_EDITOR
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + prefabPath);
#endif
            if (prefab == null)
            {
                throw new System.Exception("没有找到这个预设:" + prefabPath);
            }
            else
            {
                returnObj.data=prefab;
            }
        }
    }


    public static void LoadAsync(string relativePath, Action<string> OnLoad, ICancelHandle handle = null)
    {
        GlobalCoroutine.Start(LoadFile(relativePath, true, bytes => OnLoad(Encoding.UTF8.GetString(bytes))), handle);
    }

    /// <summary>
    /// 加载独立Bundle
    /// </summary>
    static IEnumerator LoadBundleBase(string relativePath, bool checkUpdate, AsyncReturn<AssetBundle> returnData, ICancelHandle unloadAB =null)
    {
        var _AsyncReturn = new AsyncReturn<byte[]>();
        yield return FileVersionMgr.instance.LoadFile(relativePath, checkUpdate, _AsyncReturn);

        AssetBundle assetBundle;
        if(readAssetBundleAsync)
        {
            
            var abcr = AssetBundle.LoadFromMemoryAsync(_AsyncReturn.data);
            //转到unity自己的协程中异步读取数据流
            yield return abcr;
            //Debug.Log("加载完成" + relativePath);
            if(abcr==null)
            {

            }
            assetBundle = abcr.assetBundle;
        }
        else
        {
            assetBundle = AssetBundle.LoadFromMemory(_AsyncReturn.data);
        }
        if (unloadAB  != null)
        {
            unloadAB.CancelAct += () =>
            {
                //Debug.Log("释放" + relativePath);
                assetBundle.Unload(false);
            };
        }
        if(returnData!=null) returnData.data = assetBundle;
    }
    
    /// <summary>
    /// 加载独立资源文件
    /// </summary>
    static IEnumerator LoadFile(string relativePath, bool needNew, Action<byte[]> OnComplete)
    {
        var _AsyncReturn = new AsyncReturn<byte[]>();
        yield return FileVersionMgr.instance.LoadFile(relativePath, needNew, _AsyncReturn);
        if (OnComplete != null) OnComplete(_AsyncReturn.data);
    }
}

