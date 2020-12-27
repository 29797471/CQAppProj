using CqCore;
using UnityCore;
using UnityEngine;

[System.Serializable]
public class RoadArgs_Draw
{

    [CheckBox("建造45度限制")]
    public bool limit45 = true;

    [Color("拆除道路")]
    public Color removeRoad = Color.yellow;

    [Color("拆除后失效的道路")]
    public Color invalidRoad = Color.red;

    [Color("修建道路")]
    public Color addRoad = Color.white;

    [Color("不可建造")]
    public Color notAllowBuild = Color.red;

    [Color("显示我的道路")]
    public Color showMyselfRoad = Color.green;

    /// <summary>
    /// 拾取到辅助线颜色
    /// </summary>
    [Color("拾取到辅助线颜色")]
    public Color pickupHelpLine = Color.green;


    [Color("行车路线")]
    public Color carRoadLine = Color.green;

    /// <summary>
    /// 路点网格间距
    /// </summary>
    [TextBox("路点网格间距")]
    public int deltaGrid = 10;

    // <summary>
    /// 拐点半径
    /// </summary>
    [TextBox("拐点半径")]
    public float drawCorPointR = 1f;

    /// <summary>
    /// 路点半径
    /// </summary>
    [TextBox("路点半径")]
    public float drawRoadPointR = 5f;

    /// <summary>
    /// 绘制线条宽度
    /// </summary>
    [TextBox("绘制线条宽度")]
    public float drawLineWidth = 0.5f;

    /// <summary>
    /// 辅助线长
    /// </summary>
    [TextBox("辅助线长")]
    public float helpLineLength = 100f;

    /// <summary>
    /// 辅助线最大段数
    /// </summary>
    [TextBox("辅助线最大段数")]
    public int helpLineMaxCount = 3;

    /// <summary>
    /// 辅助线吸附距离
    /// </summary>
    [TextBox("辅助线吸附距离")]
    public float helpLineAbsorbDis = 15f;

    /// <summary>
    /// 辅助线每虚线段的长度
    /// </summary>
    [TextBox("辅助线每虚线段的长度")]
    public float helpLinePartWidth = 10f;

    [TextBox("辅助平移道路时的移动单位")]
    public float moveDeltaDis = 10f;
}