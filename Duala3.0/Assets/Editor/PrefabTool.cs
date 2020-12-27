using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PrefabTool : EditorWindow
{
    [MenuItem("Assets/Set as Topcap (设置为顶盖)")]
    static void SetPrefabAsTopcap()
    {
        Material topcapMaterial = null;
        /// 查找是否存在顶盖材质 ///
        string[] paths = AssetDatabase.FindAssets("share_topcap");
        if (paths != null && paths.Length > 0)
        {
            topcapMaterial = (Material)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(paths[0]), typeof(Material));
        }
        else
        {
            string[] shaderpaths = AssetDatabase.FindAssets("DepthCover3D");
            if (shaderpaths == null || shaderpaths.Length <= 0)
            {
                Debug.LogError("Can not find shader DepthCover3D !");
            }

            string shaderFilePath = "";
            for (int a = 0; a < shaderpaths.Length; a++)
            {
                shaderFilePath = AssetDatabase.GUIDToAssetPath(shaderpaths[a]);
                string PrefabExt = Path.GetExtension(shaderFilePath);
                PrefabExt = PrefabExt.ToLower();
                if (PrefabExt == ".shader")
                {
                    break;
                }
            }

            Shader shader = (Shader)AssetDatabase.LoadAssetAtPath(shaderFilePath, typeof(Shader));
            if (shader == null)
            {
                Debug.LogError("Can not load shader DepthCover3D !");
                return;
            }
            topcapMaterial = new Material(shader);

            string filePath = "Assets/Art/ShareMaterials/share_topcap.mat";
            AssetDatabase.CreateAsset(topcapMaterial, filePath);
        }

        topcapMaterial.renderQueue = 2010;

        Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
        foreach (Object obj in selection)
        {
            GameObject go = obj as GameObject;
            MeshRenderer meshRen = go.GetComponent<MeshRenderer>();
            if (meshRen == null)
            {
                continue;
            }

            meshRen.sharedMaterial = topcapMaterial;
        }

        //Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        //foreach (Object obj in selection)
        //{
        //    GameObject go = obj as GameObject;
        //    SetPrefabAsTopcap(go);
        //}

        Debug.Log("Topcap OK !");
    }

    [MenuItem("Assets/Set as Above Ground (设置地面上材质)")]
    static void SetMaterialAsAboveGround()
    {
        Object[] selection = Selection.GetFiltered(typeof(Material), SelectionMode.Deep);
        foreach (Object obj in selection)
        {
            Material mat = obj as Material;
            mat.renderQueue = 2510;
        }
    }

    [MenuItem("Assets/Set as Ground (设置为地面)")]
    static void SetPrefabAsGround()
    {
        Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
        foreach (Object obj in selection)
        {
            GameObject go = obj as GameObject;
            MeshRenderer meshRen = go.GetComponent<MeshRenderer>();
            if (meshRen == null)
            {
                continue;
            }

            foreach (Material mat in meshRen.sharedMaterials)
            {
                mat.renderQueue = 2510;
            }
        }

        Debug.Log("Holl OK !");
    }

    ///// 迭代修改子节点为顶盖 ///
    //static void SetPrefabAsTopcap(GameObject root)
    //{
    //    /// 查找是否存在顶盖材质 ///
    //    string prefabName = root.name + ".prefab";
    //    string[] paths = AssetDatabase.FindAssets(prefabName);
    //    if (paths == null || paths.Length == 0 || paths.Length > 1)
    //    {
    //        Debug.LogError("Prefab path error !");
    //        return;
    //    }

    //    string path = AssetDatabase.GUIDToAssetPath(paths[0]);
    //    path = Path.GetDirectoryName(path);
    //    string filePath;
    //    if (Utility.ContainsFolder(path, "Materials"))
    //    {
    //        filePath = path + "/Materials/topcap";
    //    }
    //    else
    //    {
    //        filePath = path + "/topcap";
    //    }

    //    Material topcapMaterial = null;
    //    if (Directory.Exists(filePath))
    //    {
    //        topcapMaterial = (Material)AssetDatabase.LoadAssetAtPath(filePath, typeof(Material));
    //    }
    //    else
    //    {
    //        Shader shader = (Shader)AssetDatabase.LoadAssetAtPath(filePath, typeof(Shader));
    //        topcapMaterial = new Material(shader);
    //        topcapMaterial.renderQueue = 2001;
    //        AssetDatabase.CreateAsset(topcapMaterial, filePath);
    //    }

    //    SetPrefabAndChildenAsTopcap(root, topcapMaterial);
    //}

    //static void SetPrefabAndChildenAsTopcap(GameObject root, Material mat)
    //{
    //    MeshRenderer meshRen = root.GetComponent<MeshRenderer>();
    //    if (meshRen != null)
    //    {
    //        meshRen.sharedMaterial = mat;
    //    }

    //    int nChild = root.transform.childCount;
    //    for (int a = 0; a < nChild; a++)
    //    {
    //        SetPrefabAndChildenAsTopcap(root.transform.GetChild(a).gameObject, mat);
    //    }
    //}
}