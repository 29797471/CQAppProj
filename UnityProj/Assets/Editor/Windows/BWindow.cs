using UnityEngine;
using UnityEditor;
public class BWindow : EditorWindow
{
    [MenuItem ("Window/命令行 %M")]
	public static void Open()
	{
        GetWindow<BWindow>("命令行");
	}
    string content;
    string input;
	void OnGUI()
	{
        //CustomModel.MenuModel.instance.ShowSaveDesign=0
        BeginWindows();
        #region 垂直
        EditorGUILayout.BeginVertical();
        #region 水平布局
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(content,  GUILayout.Height(200));
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.EndHorizontal();
        #endregion
        #region 水平布局
        EditorGUILayout.BeginHorizontal();
        input = EditorGUILayout.TextArea(input, GUILayout.MinHeight(50));
        EditorGUILayout.EndHorizontal();
        #endregion
        #region 水平布局
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("执行"))
        {
            content += "\n";
            content += input;
            Debug.Log(AssemblyUtil.CompileAndExec(input));
            input = "";
        }

        EditorGUILayout.EndHorizontal();
        #endregion
        EditorGUILayout.EndVertical();
        #endregion
        EndWindows();
	}
	/// <summary>
	/// 当窗口获得焦点时调用一次
	/// </summary>
	void OnFocus()
	{
	}
	/// <summary>
	/// 当窗口丢失焦点时调用一次
	/// </summary>
	void OnLostFocus()
	{
	}
	/// <summary>
	/// 当Hierarchy视图中的任何对象发生改变时调用一次
	 /// </summary>
	void OnHierarchyChange()
	{
	}
	/// <summary>
	/// 当Project视图中的资源发生改变时调用一次
	/// </summary>
	void OnProjectChange()
	{
	}
	/// <summary>
	/// 窗口面板的更新
	/// 这里开启窗口的重绘，不然窗口信息不会刷新
	/// </summary>
	void OnInspectorUpdate()
	{
	}
	/// <summary>
	/// 当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
	/// 有可能是多选，
	/// </summary>
	void OnSelectionChange()
	{
	}
	/// <summary>
	/// 当窗口关闭时调用
	/// </summary>
	void OnDestroy()
	{
	}
}