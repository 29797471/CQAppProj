using CqCore;

/// <summary>
/// 可建造范围生成方式
/// </summary>
public enum MakeAllowBuildStyle
{
    /// <summary>
    /// 对齐路点生成
    /// </summary>
    [EnumLabel("对齐路点生成")]
    AlignmentPoint,

    /// <summary>
    /// 对齐转弯处
    /// </summary>
    [EnumLabel("对齐转弯处")]
    AlignmentCorner,
}

