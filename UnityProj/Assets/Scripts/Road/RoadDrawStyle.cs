using CqCore;

/// <summary>
/// 道路绘制
/// </summary>
public enum RoadDrawStyle
{
    [EnumLabel("路长")]
    RoadLength = 1,

    [EnumLabel("路点(资源接入点为黄色)")]
    RoadPoint = 2,

    [EnumLabel("网格")]
    Grid = 4,

    [EnumLabel("车道线")]
    CarPath = 8,

    [EnumLabel("辅助线(含辅助点)")]
    Guideline = 16,

    [EnumLabel("弯角(蓝)")]
    Corner = 32,

    /// <summary>
    /// 去除弯道的道路起止中心点
    /// </summary>
    [EnumLabel("道路起止点(绿)")]
    CenterStart = 64,

    [EnumLabel("交叉路口行车路线")]
    CrossRoadCarPath = 128,

}

