using UnityEngine;
using UnityEditor;
public class AWindow : EditorWindow
{

	[MenuItem ("Window/TestA %#&P")]
	public static void Open()
	{
		EditorWindow.GetWindow<AWindow>();
	}
	void OnGUI()
	{
		BeginWindows();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("xxx");
		if (GUILayout.Button("abc"))
		{
			Debug.Log("abc");
		}
		if (GUILayout.Button("bbb"))
		{
			Debug.Log("bbb");
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("ddd"))
		{
			Debug.Log("ddd");
		}
		if (GUILayout.Button("fff"))
		{
			Debug.Log("fff");
		}
		if (GUILayout.Button("ccc"))
		{
			Debug.Log("ccc");
		}

		EditorGUILayout.EndHorizontal();

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