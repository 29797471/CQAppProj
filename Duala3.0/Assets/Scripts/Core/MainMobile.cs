using UnityEngine;

/// <summary>
/// 模拟移动设备测试入口
/// </summary>
public class MainMobile : Main
{
    private void Awake()
    {
        Debug.Log("移动模式下启动");
        ApplicationUtil.runByMobileDevice = true;
    }
}

