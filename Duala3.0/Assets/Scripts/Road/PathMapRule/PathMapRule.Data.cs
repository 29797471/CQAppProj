using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityCore;
using UnityEngine;

public partial class PathMapRule : Singleton<PathMapRule>
{
    /// <summary>
    /// 路点网格间距
    /// </summary>
    public int deltaGrid
    {
        get
        {
            return Arp.roadArgs_Draw.deltaGrid;
        }
    }
    /// <summary>
    /// 允许建造的道路长度
    /// </summary>
    public float allowCreateRoadLength;


    System.Action OnLogoutCall;


    /// <summary>
    /// 道路样式列表
    /// </summary>
    public Dictionary<int, RoadStyle> dicStyles;

    RoadArgs arp;

    public RoadArgs Arp
    {
        get
        {
            if(arp==null)
            {
                arp = Object.FindObjectOfType<RoadArgs>();
            }
            return arp;
        }
    }

    //PathElement fromElement;
    Vector2 from;

    public Vector2 GetFrom()
    {
        return from;
    }
    public Vector2 GetTo()
    {
        return to;
    }
    public float RoadDistance()
    {
        return Vector2.Distance(from, to);
    }

    //PathElement toElement;
    Vector2 to;
    RoadStyle style;

    /// <summary>
    /// 得到这条路的外接矩形
    /// </summary>
    public Vector2[] Polygon
    {
        get
        {
            var mPolygon = new Vector2[4];
            var Dir = (to - from).normalized;
            var delta = Dir.Rot90() * style.road_width / 2;
            mPolygon[0] = from;
            mPolygon[1] = from - delta;
            mPolygon[2] = to - delta;
            mPolygon[3] = to;
            return mPolygon;
        }
    }
    System.Func<float, float> RoadPointColorCalcFunc;

    /// <summary>
    /// 辅助点
    /// </summary>
    List<Vector2> helpPoints;

    /// <summary>
    /// 辅助线
    /// </summary>
    List<Segment> helpSegments;


    /// <summary>
    /// 绘制辅助线
    /// </summary>
    public HelpDrawMono drawInst;

    /// <summary>
    /// 绘制错误提示
    /// </summary>
    public PathMapDraw drawError;

    /// <summary>
    /// 道路图数据结构
    /// </summary>
    public PathMap PathMapInst { get; private set; }

    /// <summary>
    /// 道路生成器
    /// </summary>
    public RoadMaker RoadMakerInst { get; private set; }

    public void Init(RoadArgs arp,RoadStyle[] styles, System.Func<float, float> RoadPointColorCalcFunc)
    {
        this.arp = arp;
        this.dicStyles = new Dictionary<int, RoadStyle>();


        for (int i = 0; i < styles.Length; i++)
        {
            var it = styles[i];
            dicStyles[it.id] = it;
        }
        style = styles[0];

        this.RoadPointColorCalcFunc = RoadPointColorCalcFunc;
        helpPoints = new List<Vector2>();
        helpSegments = new List<Segment>();

        {
            drawInst = Object.FindObjectOfType<HelpDrawMono>();

            //drawInst = new PathMapDraw();
            //drawInst.Init(true);
            //drawInst.lineWidth = arp.roadArgs_Draw.drawLineWidth;
        }
        {
            drawError = new PathMapDraw();
            drawError.Init(true);
            drawError.lineWidth = arp.roadArgs_Draw.drawLineWidth;
        }

        checkPathMap = new PathMap();
        checkPathVs = new HashSet<PathV>();

    }
    Transform _root;

    public List<PathE> Getlinks()
    {
        return PathMapInst.links;
    }

    /// <summary>
    /// 设置生成器
    /// </summary>
    public void SetMake(RoadMaker roadMaker, Transform root)
    {
        _root = root;

        PathMapInst = new PathMap();
        PathMapInst.Init(roadMaker, root);
        
        RoadMakerInst = roadMaker;
    }

    public void ClearData()
    {
        PathMapInst.ClearAll();
    }

    PathMap selfData;
    public void ViewOthers()
    {
        selfData = PathMapInst;
        selfData.ClearMake();
        PathMapInst = new PathMap();
        PathMapInst.Init(RoadMakerInst, _root);
    }
    public void ReturnSelf()
    {
        PathMapInst.ClearAll();
        PathMapInst = selfData;
        var Va = selfData.list.Last();
        
        PathMapInst.MakeAll();
    }

    
}
