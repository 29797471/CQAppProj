using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CreateObjectMenu 
{
    [MenuItem("GameObject/UI/Horizontal - Custom", false, 2036)]
    static public void AddHorizontal(MenuCommand command)
    {
        AddLayoutGroup(command,"Horizontal");
    }
    [MenuItem("GameObject/UI/Vertical - Custom", false, 2036)]
    static public void AddVertical(MenuCommand command)
    {
        AddLayoutGroup(command, "Vertical");
    }
    static void AddLayoutGroup(MenuCommand command,string LayoutGroup)
    {
        GameObject go = new GameObject(LayoutGroup);
        go.AddComponent(AssemblyUtil.GetType(string.Format("UnityEngine.UI.{0}LayoutGroup",LayoutGroup)));
        // Add support for new prefab mode
        StageUtility.PlaceGameObjectInCurrentStage(go);
        go.GetComponent<RectTransform>().ReSet();
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        GameObject contextObject = command.context as GameObject;
        if (contextObject != null)
        {
            GameObjectUtility.SetParentAndAlign(go, contextObject);
            //支持撤销的操作
            Undo.SetTransformParent(go.transform, contextObject.transform, "Parent " + go.name);
        }
    }
}
