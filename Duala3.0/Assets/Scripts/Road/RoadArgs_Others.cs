using CqCore;
using UnityCore;
using UnityEngine;

[System.Serializable]
public class RoadArgs_Others
{
    /// <summary>
    /// 生成相关装饰物
    /// </summary>
    [CheckBox("生成相关装饰物")]
    public bool makeOthers = true;


    /// <summary>
    /// 路灯间距
    /// </summary>
    [TextBox("路灯间距")]
    public float lightSpacing = 60;

    /// <summary>
    /// 路边物件距离
    /// </summary>
    [TextBox("路边物件距离")]
    public float roadSideDis = 0.5f;


    /// <summary>
    /// 井盖间距
    /// </summary>
    [TextBox("井盖间距")]
    public float manholeCoverSpacing = 43f;

    /// <summary>
    /// 井盖出现概率
    /// </summary>
    [TextBox("井盖出现概率")]
    public float manholeCoverK = 0.5f;

    [TextBox("井盖高度")]
    public float manholeCoverHeight = 0.01f;

    public GameObject manholeCover;

    public GameObject streetLight;

    public GameObject trafficLight_Person;

    public GameObject trafficLight_Car;

    public GameObject turnSign;
}