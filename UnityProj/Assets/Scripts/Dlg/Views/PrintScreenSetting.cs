using UnityCore;
using UnityEngine;

public class PrintScreenSetting : ScriptableObject
{
    static PrintScreenSetting mSetting;
    public static PrintScreenSetting Setting
    {
        get
        {
            if (mSetting == null)mSetting = Resources.Load<PrintScreenSetting>("CustomSetting");
            return mSetting;
        }
    }

    [Vector("截屏上中心点")]
    public Vector2 startPos;

    [TextBox("截屏高度")]
    public float height;

    [TextBox("截屏左右宽度")]
    public float sK;

    [TextBox("基本宽")]
    public float baseWidth;

    [TextBox("基本高")]
    public float baseHight;
}
