using CqCore;
using SLua;
using System;
using System.Collections;
using System.IO;
using UnityCore;
using UnityEngine;

/// <summary>
/// 包装第三方lua类,提供项目需要的接口
/// </summary>
public class LuaMgr : Singleton<LuaMgr>,IDisposable
{
    
    public override void Dispose()
    {
        LuaProxyMgr.instance.Dispose();
        LuaGlobal.instance.Dispose();
        LuaSvr.Dispose();
        if(LuaSvr.mainState!=null)
        {
            LuaSvr.mainState.Dispose();
            LuaSvr.mainState = null;
        }
        base.Dispose();
    }
    /// <summary>
    /// 执行一段lua代码
    /// </summary>
    public object DoCommand(string command)
    {
        if (MainState != null)
        {
            Debug.Log("执行lua命令:" + command);
            return MainState.doString(command);
        }
        return null;
    }

    /// <summary>
    /// 加载lua文件
    /// </summary>
    public object DoFile(string fn)
    {
        if (MainState != null)
        {
            return MainState.doFile(fn);
        }
        return null;
    }
    /// <summary>
    /// require lua文件
    /// </summary>
    public object require(string fn)
    {
        if (MainState != null)
        {
            return MainState.doString(string.Format("require '{0}'",fn));
        }
        return null;
    }


    /// <summary>
    /// 提供给编辑时调用lua接口
    /// </summary>
    public LuaSvr.MainState MainState
    {
        get
        {
            if (LuaSvr.mainState == null)
            {
                if (!Application.isPlaying )
                {
                    StartEditorLua();
                }
            }
            return LuaSvr.mainState;
        }
    }

    /// <summary>
    /// 是否有这个lua文件
    /// </summary>
    /// <param name="fn">lua项目中require对应的路径</param>
    /// <returns></returns>
    public bool IsLuaFile(string fn)
    {
        var file = string.Format("{0}/{1}.lua", luaDir, fn);
        return File.Exists(file);
    }

    public string luaDir;

    /// <summary>
    /// 启动lua
    /// </summary>
    public IEnumerator StartLua()
    {
        var ls = new LuaSvr();

        
        if (GlobalMgr.instance.isMobilePlatform)
        {
            luaDir = ApplicationUtil.persistentDataPath + "/" + CustomSetting.Inst.cachLuaFolder;
        }
        else
        {
            luaDir = CustomSetting.Inst.luaFolder;
        }
        LuaSvr.mainState.loaderDelegate = (string fn, ref string y) =>
        {
            var file = string.Format("{0}/{1}.lua", luaDir, fn);
            //Debug.Log("读Lua文件:"+file);
            var bytes = File.ReadAllBytes(file);
            if (bytes == null)
            {
                Debug.LogError(string.Format("文件({0})未找到", file));
            }
            return bytes;
        };

        bool complete = false;
        //Debug.Log(LuaSvr.mainState.luaFolder);
        ls.init(null/*p=> Debug.Log(string.Format("加载lua进度{0}%", p))*/,
            () =>
            {
                ls.start("main");
                complete = true;
            },
            LuaSvrFlag.LSF_BASIC | LuaSvrFlag.LSF_EXTLIB);
        while (!complete) yield return null;

    }
    
    void StartEditorLua()
    {
        var ls = new LuaSvr();
        LuaSvr.mainState.loaderDelegate = (string fn, ref string y) =>
        {
            var file = string.Format("{0}/{1}.lua", CustomSetting.Inst.luaFolder, fn);
            var bytes = File.ReadAllBytes(file);
            if (bytes == null)
            {
                Debug.LogError(string.Format("文件({0})未找到", file));
            }
            return bytes;
        };
        ls.init(null, () => ls.start("mainEditor"), LuaSvrFlag.LSF_BASIC | LuaSvrFlag.LSF_EXTLIB);
    }

    /// <summary>
    /// 解压lua压缩包
    /// </summary>
    public IEnumerator UnZipLua()
    {
        //if (GlobalMgr.instance.isMobilePlatform)
        {
            //LUA需要拷贝到用户目录下同步读取

            var luaUserDir = ApplicationUtil.persistentDataPath + "/" + CustomSetting.Inst.cachLuaFolder;

            //Lua用户目录不存在时
            //if (!Directory.Exists(luaUserDir))
            {
                var AsyncReturn = new AsyncReturn<byte[]>();
                yield return UnityFileUtil.ReadStreamAssetsFile(CustomSetting.Inst.cachLuaFolder + ".zip", AsyncReturn);
                var luaZipPath = luaUserDir + ".zip";
                if (AsyncReturn.data != null)
                {
                    var localFolderPath = Path.GetDirectoryName(luaZipPath);
                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }
                    File.WriteAllBytes(luaZipPath, AsyncReturn.data);

                    ZipHelper.UnZip(luaZipPath, luaUserDir);
                    FileOpr.DeleteFile(luaZipPath);
                    Debug.Log("解压lua压缩包");
                }
                else
                {
                    Debug.LogError("拷贝lua压缩包失败");
                }
            }
        }
    }
}
