using CqCore;
using UnityCore;
using UnityEditor;
using UnityEditor.Hardware;
using UnityEngine;

public static class AssetsMenu
{
   
    //重新载入
    [MenuItem("Assets/注册类型(UnityUtil.RegUnityType)")]
    static void RegUnityType()
    {
        UnityUtil.RegUnityType();
    }

    [MenuItem("Assets/清内存")]
    public static void CleanRes()
    {
        Resources.UnloadUnusedAssets();
        EditorUtility.UnloadUnusedAssetsImmediate(true);
        System.GC.Collect();
    }
    [MenuItem("Assets/打印连接设备")]
    public static void PrintDevDeviceList()
    {
        var devices = DevDeviceList.GetDevices();
        SerializeTypeUtil.RegType(typeof(DevDevice), "DevDevice", SerializeTypeStyle.Field);
        Debug.Log(CqSerialize.Serialize(devices));
    }

}
