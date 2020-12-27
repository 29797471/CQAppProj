using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextControl : MonoBehaviour
{
    Text mTextCom;
    public Text TextCom
    {
        get
        {
            if (mTextCom == null)
            {
                mTextCom = GetComponent<Text>();
            }
            return mTextCom;
        }
    }

    string allText;
    string TextContent
    {
        get
        {
            return TextCom.text;
        }
        set
        {
            TextCom.text = value;
        }
    }
    string content;
    /// <summary>
    /// 0~1
    /// </summary>
    public float PrintPos
    {
        set
        {
            mPos = value;
            if (allText == null)
            {
                allText = TextContent;
            }
            var index=Mathf.Clamp(mPos, 0, 1)* allText.Length;
            TextContent = allText.Substring(0, (int) index);
        }
        get
        {
            return mPos;
        }
    }
    [SerializeField, SetProperty("PrintPos")]
    float mPos;

    
    public float FontSize
    {
        set
        {
            TextCom.fontSize = (int)value;
        }
        get
        {
            return TextCom.fontSize;
        }
    }

    /// <summary>
    /// 0~1
    /// </summary>
    public float PrintSmallBig
    {
        set
        {
            mPrintSmallBig = value;
            if (allText == null)
            {
                allText = TextContent;
            }
            var index = Mathf.Clamp(mPrintSmallBig, 0, 1) * allText.Length;

            if((int)index==allText.Length)
            {
                TextContent = allText;
            }
            else
            {
                TextContent = string.Format("{0}<size={1}>{2}</size>",
                allText.Substring(0, (int)index),
                Mathf.Lerp(15, TextCom.fontSize,
                index - (int)index),
                allText[(int)index]);
            }
        }
        get
        {
            return mPrintSmallBig;
        }
    }

    [SerializeField, SetProperty("PrintSmallBig")]
    float mPrintSmallBig;
}
