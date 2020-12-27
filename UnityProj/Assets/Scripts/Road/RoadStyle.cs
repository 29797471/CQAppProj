using UnityCore;
using UnityEngine;

/// <summary>
/// 道路样式(4,6,8车道)
/// </summary>
[System.Serializable]
public class RoadStyle
{
    [TextBox("名称")]
    public string name;

    /// <summary>
    /// 样式id
    /// </summary>
    public int id;

    /// <summary>
    /// 道路的来回车道数量
    /// </summary>
    [TextBox("来回车道数量")]
    public int lanes;

    /// <summary>
    /// 一边的车道数
    /// </summary>
    public int SideOfLanes { get { return lanes / 2; } }

    /// <summary>
    /// 人行道宽度
    /// </summary>
    [TextBox("人行道宽度")]
    public float crossing_width;

    /// <summary>
    /// 单条车道宽度
    /// </summary>
    [TextBox("单车道宽度")]
    public float car_road_width;

    /// <summary>
    /// 中央隔离带宽度
    /// 城市道路设计规范-第三节 机动车车道与路面宽度：线宽 15cm,两线间距 20cm
    /// </summary>
    [TextBox("中央隔离带宽度"), ToolTip("暂时只用于双黄线间距")]
    public float centerWidth = 0.275f;


    /// <summary>
    /// 边线间距
    /// </summary>
    [TextBox("边线间距")]
    public float sideDis = 0.2f;


    /// <summary>
    /// 路边去杂草宽度
    /// </summary>
    [TextBox("路边杂草宽度")]
    public float roadSideWidth = 0f;

    /// <summary>
    /// 辐射范围(半径)
    /// </summary>
    [TextBox("辐射范围"), ToolTip("垂直于道路方向的宽度,不计算道路本身宽度")]
    public float range;

    /// <summary>
    /// 内弯半径
    /// </summary>
    [TextBox("内弯半径")]
    public float insideRad;

    /// <summary>
    /// 人行道和除草的宽度
    /// </summary>
    public float CrossingAndSideWidth
    {
        get
        {
            return crossing_width + roadSideWidth;
        }
    }

    /// <summary>
    /// 当添加道路的辅助线接近45度时,当距离小于这个值,端点会吸附过去,
    /// </summary>
    [TextBox("45度吸附距离")]
    public float toDis = 20f;

    /// <summary>
    /// 侧边人行道中点与中线的距离
    /// </summary>
    public float crossing_center
    {
        get
        {
            return lanes * car_road_width / 2 + crossing_width / 2;
        }
    }

    /// <summary>
    /// 侧边人行道和除草范围的中心点到中线的距离
    /// </summary>
    public float side_center
    {
        get
        {
            return lanes * car_road_width / 2 + crossing_width + roadSideWidth / 2;
        }
    }

    /// <summary>
    /// 道路宽度(人行道+车道)
    /// </summary>
    public float road_width
    {
        get
        {
            return car_road_width * lanes + crossing_width * 2;
        }
    }

    /// <summary>
    /// 半道路宽度(人行道+车道)
    /// </summary>
    public float HalfRoadW
    {
        get
        {
            return car_road_width * lanes / 2 + crossing_width;
        }
    }

    /// <summary>
    /// 半道路宽度(人行道+车道+除草)
    /// </summary>
    public float HalfRoadAndSide
    {
        get
        {
            return car_road_width * lanes / 2 + crossing_width + roadSideWidth;
        }
    }

    /// <summary>
    /// 车道宽度
    /// </summary>
    public float CarRoadW
    {
        get
        {
            return car_road_width * lanes;
        }
    }

    /// <summary>
    /// 半总车道宽度
    /// </summary>
    public float HalfCarRoadW
    {
        get
        {
            return car_road_width * lanes / 2f;
        }
    }

    /// <summary>
    /// 由车道索引计算这条车道中心点和中心线的距离
    /// </summary>
    public float GetLaneDis(int index)
    {
        return car_road_width * (index + 0.5f);
    }
}
