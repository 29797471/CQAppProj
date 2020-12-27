using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// </summary>
public class AutoAssets : AssetPostprocessor
{
    //所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的  
    public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        return;
        //Debug.Log("OnPostprocessAllAssets");
        //foreach (string str in importedAsset)
        //{
        //    Debug.Log("importedAsset = " + str);
        //}
        //foreach (string str in deletedAssets)
        //{
        //    Debug.Log("deletedAssets = " + str);
        //}
        //foreach (string str in movedAssets)
        //{
        //    Debug.Log("movedAssets = " + str);
        //}
        //foreach (string str in movedFromAssetPaths)
        //{
        //    Debug.Log("movedFromAssetPaths = " + str);
        //}
    }
}
