using CqCore;

public enum RoadType
{
    [EnumLabel("路面")]
    /// <summary>
    /// 自己的路面
    /// </summary>
    RT_MainRoad = 1,

    [EnumLabel("人行道")]
    /// <summary>
    /// 人行道
    /// </summary>
    RT_Pavement,

    [EnumLabel("中心线")]
    RT_CenterLine,

    [EnumLabel("中心虚线")]
    RT_CenterDottedLine,

    [EnumLabel("车道虚线")]
    RT_SignleDottedLine,

    [EnumLabel("车道实线")]
    RT_SignleSolidLine,

    [EnumLabel("斑马线")]
    RT_ZebraCrossingLine,

    [EnumLabel("路灯")]
    RT_StreetLamp,

    [EnumLabel("井盖")]
    RT_ManholeCover,

    [EnumLabel("行人红绿灯")]
    RT_TrafficLight_Person,

    [EnumLabel("交通红绿灯")]
    RT_TrafficLight_Car,
}