using UnityEditor;
/// <summary>
/// 处理编辑器下当配置文件更改时,刷新数据
/// </summary>
public class ReFlashExcelData : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var it in importedAssets)
        {
            if (it.EndsWith("mark_config_changed._temp"))
            {
                FileOpr.DeleteFile(it);
                FileOpr.DeleteFile(it+".meta");
                //Debug.Log(FileOpr.GetNameByShort(it));
                //LuaMgr.instance.require("Excel/More/Config_AppConfig");
                LuaMgr.instance.Dispose();
                AssetDatabase.Refresh();
            }
        }
    }
}
