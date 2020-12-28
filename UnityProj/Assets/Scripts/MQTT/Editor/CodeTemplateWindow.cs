using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CodeTemplateWindow : EditorWindow
{
    public static CodeTemplateWindow Inst
    {
        get
        {
            return GetWindow<CodeTemplateWindow>(winName);
        }
    }
    const string dataFile = "Assets/Editor/command.txt";
    Dictionary<string, string> dic;
    Dictionary<string, string> Dic
    {
        get
        {
            if (dic == null)
            {
                if(!FileOpr.IsFilePath(dataFile))
                {
                    dic = new Dictionary<string, string>();
                }
                else
                {
                    dic = FileOpr.ReadObject<Dictionary<string, string>>(dataFile);
                    if (dic == null) dic = new Dictionary<string, string>();
                }
            }
            return dic;
        }
    }

    const string winName = "代码模版";

    System.Action<string> OnCode;

    public void Open(System.Action<string> OnCode)
    {
        this.OnCode = OnCode;
    }
    private void OnEnable()
    {

    }
    void OnGUI()
    {
        foreach (var it in Dic.Keys)
        {
            var key = it;
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(key))
                {
                    OnCode(Dic[key]);
                    Close();
                }
                if (GUILayout.Button("修改", GUILayout.Width(50)))
                {
                    TitileContentEditorWindow.Open(key,Dic[key], (title,conent) =>
                    {
                        Dic.Remove(key);
                        Dic[title] = conent;
                        SaveDic();
                    }, "模版编辑");
                }
                if (GUILayout.Button("-", GUILayout.Width(50)))
                {
                    Dic.Remove(key);
                    SaveDic();
                    return;
                }
            };
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+", GUILayout.Width(50)))
        {
            int index = 0;
            while (dic.ContainsKey("key" + index)) index++;
            dic["key" + index] = "";
            SaveDic();
        }
    }

    void SaveDic()
    {
        FileOpr.SaveObject(dataFile, dic);
    }
    private void OnDisable()
    {
    }
}
