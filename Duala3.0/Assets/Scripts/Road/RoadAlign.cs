using CqCore;

/// <summary>
/// 操作点吸附基础规则
/// </summary>
public enum RoadAlign
{
    /// <summary>
    /// 吸附到八个固定方向
    /// </summary>
    [EnumLabel("吸附到八个固定方向")]
    AbsorbToAngle = 1,

    /// <summary>
    /// 吸附到固定道路长度
    /// </summary>
    [EnumLabel("吸附到固定道路长度")]
    AbsorbToRoadLength = 2,

    /// <summary>
    /// 吸附到路点
    /// </summary>
    [EnumLabel("吸附到路点")]
    AbsorbToRoadPoint = 4,

    /// <summary>
    /// 吸附到网格
    /// </summary>
    [EnumLabel("吸附到网格")]
    AbsorbToGrid = 8,

    /// <summary>
    /// 吸附到辅助点
    /// </summary>
    [EnumLabel("吸附到辅助点")]
    AbsorbToHelpPoint = 16,

    /// <summary>
    /// 吸附到辅助线
    /// </summary>
    [EnumLabel("吸附到垂直的辅助线")]
    AbsorbToHelpLine = 32,
}


