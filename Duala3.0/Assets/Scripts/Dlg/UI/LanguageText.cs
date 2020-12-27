using CqCore;
using MVL;
using SLua;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 定义一个支持对文本添加参数,段落筛选,多语言翻译的组件
/// </summary>
[RequireComponent(typeof(Text))]
public class LanguageText : MonoBehaviourExtended
{
    ContentSizeFitter csf;
    
    
    Text mText;
    public string Text
    {
        get
        {
            if (mText == null) mText = GetComponent<Text>();
            return mText.text;
        }
        set
        {
            if (mText == null) mText = GetComponent<Text>();
            mText.text = value;
        }
    }

    [TextBox("多语言id"), OnValueChanged("UpdateText")]
    public int mId;
    public int Id
    {
        get { return mId; }
        set { mId = value; UpdateText(); }
    }

    /// <summary>
    /// -1表示默认值,不处理摘括号
    /// 0表示去除所有括号
    /// 其它按位来保留括号内容
    /// </summary>
    [SerializeField]
    [ComBox("段落", ComBoxStyle.CheckBox),Items("LanguageFlags"), OnValueChanged("UpdateText")]
    int mFlag=-1;
    public int Flag
    {
        get { return mFlag; }
        set { mFlag = value; UpdateText(); }
    }

    LuaTable mLuaTbl;
    LuaTable LuaTbl
    {
        get
        {
            if (mLuaTbl == null)
            {
                mLuaTbl = new LuaTable(LuaState.main);
            }
            return mLuaTbl;
        }
    }
    #region 参数表
    public object Arg0
    {
        get
        {
            return LuaTbl[0];
        }
        set
        {
            LuaTbl[0] = value;
            UpdateText();
        }
    }
    public object Arg1
    {
        get
        {
            return LuaTbl[1];
        }
        set
        {
            LuaTbl[1] = value;
            UpdateText();
        }
    }

    public object Arg2
    {
        get
        {
            return LuaTbl[2];
        }
        set
        {
            LuaTbl[2] = value;
            UpdateText();
        }
    }
    public object Arg3
    {
        get
        {
            return LuaTbl[3];
        }
        set
        {
            LuaTbl[3] = value;
            UpdateText();
        }
    }
    #endregion
    public List<string> LanguageFlags
    {
        get
        {
            if(!Application.isPlaying)LuaMgr.instance.require("Mgr/TextMgr");
            var txt = LuaGlobal.instance.TextMgr_GetText(mId, -1, null);
            var list = RegexUtil.Matches(txt, "{.+?}");
            return list.ConvertAll(x => x.ToString().SubstringEx(1, 1));
        }
    }

    [ListBox("链接变量表"),ToolTip("每一变量关联一个lua属性")]
    public List<string> members;

    private void Awake()
    {
        LuaGlobal.instance.TextMgr_SetCallBack(this);
        if (members != null)
        {
            int i = 0;
            foreach (var it in members)
            {
                var lm = gameObject.AddComponent<LinkMember>();
                lm.type = BindingFPType.System_Object;
                lm.Name = it;
                lm.comp = new ComponentProperty()
                {
                    com = this,
                    name = "Arg" + i++,
                };
                lm.LinkParent();
            }
        }
        csf = GetComponent<ContentSizeFitter>();
        if (csf != null)
        {
            csf.SetLayoutHorizontal();
            csf.SetLayoutVertical();
        }
    }
    
    public void UpdateText()
    {
        if (!Application.isPlaying) LuaMgr.instance.require("Mgr/TextMgr");
        Text=LuaGlobal.instance.TextMgr_GetText(mId, mFlag, LuaTbl);
    }
}