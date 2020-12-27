using CqCore;
using System;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

public class PrintScreenM : MonoBehaviour
{
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

    public static PrintScreenM instance;
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 截图，并且存盘
    /// </summary>
    public void PrintSreen(Action<Sprite> OnCap, string saveFilePath = null)
    {
        Camera.CameraCallback PostRender = null;
        PostRender = (c) =>
        {
            var screenK = 1f / baseWidth * baseHight * Screen.width / Screen.height;
            var realrect = new Rect((startPos.x - sK / screenK) * Screen.width, startPos.y * Screen.height, (2 * sK / screenK) * Screen.width, Screen.height * height);

            Camera.onPostRender -= PostRender;
            // 先创建一个的空纹理，大小可根据实现需要来设置
            Texture2D screenShot = new Texture2D((int)realrect.width, (int)realrect.height, TextureFormat.RGB24, false);

            // 读取屏幕像素信息并存储为纹理数据，  
            screenShot.ReadPixels(realrect, 0, 0);
            screenShot.Apply();
            if (saveFilePath != null)
            {
                FileOpr.SaveFile(saveFilePath, screenShot.EncodeToPNG());
            }
            OnCap(Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), Vector2.zero));
        };
        Camera.onPostRender += PostRender;
    }
}
