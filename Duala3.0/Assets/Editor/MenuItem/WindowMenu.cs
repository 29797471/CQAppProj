using UnityEngine;
using UnityEditor;
using System.Collections;

public class WindowMenu
{
    static EditorWindow editor = null;

    /// <summary>
    /// 统一由该函数打开自定义窗口是为了,缓存上一次打开的窗口并让其关闭
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static T OpenWindow<T>() where T:EditorWindow
    {
        if (editor)
            editor.Close();
        editor = EditorWindow.GetWindow<T>();
        return (T)editor;
    }
}
