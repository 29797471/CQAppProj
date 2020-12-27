using CqCore;
using UnityCore;
using UnityEngine;

[System.Serializable]
public class RoadArgs_Make
{
    /// <summary>
    /// 直行夹角
    /// </summary>
    [TextBox("直行夹角"), ToolTip("同前进方向相差在forwardAngle度之内视为直行")]
    public float forwardAngle = 30f;

    /// <summary>
    /// 人行横道与停止线距离
    /// </summary>
    [TextBox("停止线距离")]
    public float crosswalkLineDis = 1f;


    /// <summary>
    /// 停止线长
    /// </summary>
    [TextBox("停止线长")]
    public float carStopDis = 20f;

    /// <summary>
    /// 停车线宽
    /// </summary>
    [TextBox("停车线宽")]
    public float stopLineWidth = 0.4f;

    /// <summary>
    /// 道路转向标识与停车点距离
    /// </summary>
    [TextBox("道路转向标识距离")]
    public float carMoveSignDis = 2.5f;

    /// <summary>
    /// 黄白线宽
    /// </summary>
    [TextBox("黄白线宽")]
    public float lineWidth = 0.15f;

    //[TextBox("公共路面颜色深度")]
    //public float common_toLinear = 1.4f;

    //[TextBox("普通路面颜色深度")]
    //public float normal_toLinear = 1.14f;

    //[Color("公共道路行车标线颜色")]
    //public Color CommonWhiteCol = Color.white;

    //[Color("公共道路道路中心标线颜色")]
    //public Color CommonYellowCol = Color.yellow;

    //[Color("他人道路行车标线颜色")]
    //public Color OtherWhiteCol = Color.white;

    //[Color("他人道路中心标线颜色")]
    //public Color OtherYellowCol = Color.yellow;

    /// <summary>
    /// 单条人行横道线长
    /// </summary>
    [TextBox("单条人行横道线长")]
    public float crosswalk_len = 5f;

    /// <summary>
    /// 道路黄实线最小长度
    /// </summary>
    [TextBox("道路黄实线最小长度"), ToolTip("道路需要容纳最长的车,且道路标识能正常显示")]
    public float minYellowLineDis = 15f;

    /// <summary>
    /// 马路牙子宽度
    /// </summary>
    [TextBox("马路牙子宽度")]
    public float curbWidth = 0.2f;

    /// <summary>
    /// 可建造范围厚度
    /// </summary>
    [TextBox("可建造范围厚度")]
    public float allowBuildHeight = 0.4f;

    [ComBox("可建造范围生成方式", ComBoxStyle.RadioBox)]
    public MakeAllowBuildStyle makeStyle;

    /// <summary>
    /// 人行横道高度
    /// </summary>
    [TextBox("人行横道高度")]
    public float crosswalkHeight = 0.1f;

    /// <summary>
    /// 路面高度
    /// </summary>
    [TextBox("路面高度")]
    public float roadHeight = 0f;

}