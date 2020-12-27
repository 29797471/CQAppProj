using UnityEditor;
using UnityEngine;

/// <summary>
/// 编译完成重启游戏
/// </summary>
[InitializeOnLoad]
public static class ReLoadProj
{
    static ReLoadProj()
    {
        if(Application.isPlaying)
        {
            //GlobalMgr.instance.ReStart();
        }
    }

    [MenuItem("Assets/刷新编辑器Lua缓存")]
    static void Refresh()
    {
        LuaMgr.instance.Dispose();
        AssetDatabase.Refresh();
    }

}
