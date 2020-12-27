using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateAsset 
{

    // 在菜单栏创建功能项
    [MenuItem("Assets/Create/创建Asset")]
    static void Create()
    {
        // 实例化类  
        ScriptableObject obj = ScriptableObject.CreateInstance<CustomSetting>();

        // 如果实例化 Bullet 类为空，返回
        if (!obj)
        {
            Debug.LogWarning("Bullet not found");
            return;
        }

        // 自定义资源保存路径
        string path = Application.dataPath + "/Resources";

        // 如果项目总不包含该路径，创建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //拼接保存自定义资源（.asset） 路径
        path = string.Format("Assets/Resources/{0}.asset", (typeof(CustomSetting).ToString()));

        // 生成自定义资源到指定路径
        AssetDatabase.CreateAsset(obj, path);
    }
}
