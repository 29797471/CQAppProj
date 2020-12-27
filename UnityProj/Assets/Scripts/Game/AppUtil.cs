using System;
using UnityEngine;

public static class AppUtil
{
    public static void Capture(Action<Sprite> OnCap, Rect rect)
    {
        Camera.CameraCallback PostRender = null;
        PostRender = (c)=>
        {
            Camera.onPostRender -= PostRender;
            // 先创建一个的空纹理，大小可根据实现需要来设置
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            // 读取屏幕像素信息并存储为纹理数据，  
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            OnCap( Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), Vector2.zero));
        };
        Camera.onPostRender += PostRender;
    }
    public static void SavePNG(Texture2D tex,string path)
    {
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
    }
}
