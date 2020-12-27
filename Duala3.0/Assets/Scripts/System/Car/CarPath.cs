using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityCore;
using UnityEngine;

public class CarPath : MonoBehaviourExtended
{
    [CheckBox("绘制路线")]
    public bool drawPath;

    [Button("Click"), Click("InputTurnToNextWay")]
    public string _1;

    public CqCurve curve;

    HelpDraw draw = new HelpDraw();

    PathERoad road;

    [TextBox("当前行驶道路id")]
    public int _movePathId;

    public int movePathId
    {
        set
        {
            if(_movePathId!=value)
            {
                if(_movePathId!=0)
                {
                    if (!dic.ContainsKey(_movePathId)) dic[_movePathId] = new List<CarCollider>();
                    dic[_movePathId].Remove(col);
                }
                _movePathId = value;

                if (!dic.ContainsKey(_movePathId)) dic[_movePathId] = new List<CarCollider>();
                dic[_movePathId].Add(col);
                DestroyHandle.CancelAct+=()=> dic[_movePathId].Remove(col);
            }
        }
        get
        {
            return _movePathId;
        }
    }
    List_GC<CarCollider> list = new List_GC<CarCollider>();
    public List_GC<CarCollider> GetCarsInSameRoad()
    {
        if (!dic.ContainsKey(_movePathId)) dic[_movePathId] = new List<CarCollider>();

        list.Clear();
        foreach (var it in dic[_movePathId])
        {
            list.Add(it);
        }
        if (PathERoad.dic.ContainsKey(_movePathId))
        {
            var nextId = PathERoad.dic[_movePathId].e.to.GetHashCode();
            if(dic.ContainsKey(nextId))
            {
                foreach (var it in dic[nextId])
                {
                    list.Add(it);
                }
            }
        }
        return list;
    }
    CarCollider col;
    /// <summary>
    /// 收集在同一道路的车辆列表
    /// </summary>
    static Dictionary<int, List<CarCollider>> dic = new Dictionary<int, List<CarCollider>>();

    private void Awake()
    {
        col = GetComponent<CarCollider>();
        var e = RandomUtil.RandomIt_T(PathMapRule.instance.PathMapInst.links);
        road = RandomUtil.RandomIt_T(e.roads);
        curve = new CqCurve() { points = new List<CqCurvePoint>() };

        draw.color = Color.green;
        draw.HelpDrawStyle = HelpDrawStyle.Debug;
        DestroyHandle.CancelAct += draw.Clear;


        AddStartRoadway();
    }
    
    /// <summary>
    /// 添加车道
    /// </summary>
    void AddStartRoadway()
    {
        AddPoint(road.EnterPoint,road.GetHashCode());
        AddPoint(road.ExitPoint);
        ReDraw();
    }
    /// <summary>
    /// 添加转弯到另一条路
    /// </summary>
    public void InputTurnToNextWay()
    {
        if (road.e.IsRemoved)
        {
            Destroy(gameObject);
            return;
        }
        var list = road.PathRoadNext();
        var index=RandomUtil.Random(0, list.Count);
        var nextRoad = list[index];

        GetCarTurnCurve(road, nextRoad);

        curve.points.Last().data = nextRoad.GetHashCode();

        road = nextRoad;

        AddPoint(nextRoad.ExitPoint);

    }

    public void ReDraw()
    {
        draw.Clear();
        if (!drawPath) return;
        for (int i = 0; i < curve.points.Count-1; i++)
        {
            var p = curve.points[i];
            var next = curve.points[i+1];
            if (p.IsLine(next))
            {
                draw.DrawLine(p.point, next.point);
            }
            else
            {
                draw.DrawBezier(p.point, p.outTangent, next.inTangent, next.point);
            }
            draw.DrawCirCle(0.5f, p.point.ToVector2());
        }
    }

    /// <summary>
    /// 汽车从等红绿灯的起点开到人行道中心点的长度
    /// </summary>
    public static float len = PathMapRule.instance.Arp.roadArgs_Make.crosswalkLineDis + PathMapRule.instance.Arp.roadArgs_Make.crosswalk_len / 2;

    void LineTo(Vector2 target)
    {
        AddPoint(target);
    }
    void BezierTo(Vector2 control, Vector2 target)
    {
        var lastP = curve.points.Last();
        var ary=BezierUtil.DegreeElevation(lastP.point,control.ToVector3(),target.ToVector3());

        lastP.outTangent = ary[1];
        AddPoint(target).inTangent=ary[2];
    }
    CqCurvePoint AddPoint(Vector2 p,int? id=null)
    {
        return AddPoint(p.ToVector3(),id);
    }
    CqCurvePoint AddPoint(Vector3 p, int? id = null)
    {
        if(curve.points.Count>0)
        {
            var last = curve.points.Last();
            if (p.EqualsByEpsilon(last.point)) return last;
        }
        
        var cp = new CqCurvePoint() { point = p};
        if (id == null)
        {
            cp.data = road.e.to.GetHashCode();
        }
        else
        {
            cp.data = (int)id;
        }
        curve.points.Add(cp);
        return cp;
    }
    /// <summary>
    /// 获得从一条车道转向到另一条车道的路线点
    /// 分成两种转弯算法,向左转时以人行横道中点开始,向右以交点回退内弯半径和开始.
    /// 有交点返回true(曲线),否则false(折线)
    /// </summary>
    public void GetCarTurnCurve(PathERoad fromE, PathERoad toE)
    {
        var from_a = fromE.ExitPoint;

        var from_b = from_a;
        var to_a = toE.EnterPoint;
        var to_b = to_a;
        if (!fromE.e.to.IsRoadEnd())
        {
            from_b += fromE.e.Dir * len;
            to_b -= toE.e.Dir * len;
        }

        var style = PathMapRule.instance.GetTurnStyle(fromE.e, toE.e);

        if (style == TurnStyle.Straight)
        {
            var mid = (from_b + to_b) / 2;

            var inControl = mid.GetPedal(fromE.ExitPoint, from_b);
            var outControl = mid.GetPedal(toE.EnterPoint, to_b);
            //侧方转到另一条平行的车道.需要构造两个贝塞尔曲线
            if (!inControl.EqualsByEpsilon(mid))
            {
                inControl = (from_b + inControl) / 2;
                outControl = (to_b + outControl) / 2;

                LineTo(from_b);
                BezierTo(inControl, mid);
                BezierTo(outControl, to_b);
            }
            else
            {
                LineTo(to_b);
            }
        }
        else if (style == TurnStyle.Around)
        {
            var dis = Vector2.Distance(from_b, to_b);
            var delta = fromE.e.Dir * dis / 2;
            var fromTurnPos = from_b;
            var toTurnPos = to_b;

            LineTo(from_b);
            BezierTo(from_b + delta, (from_b + to_b) / 2 + delta);
            BezierTo(to_b + delta, to_b);
        }
        else
        {

            var p = Vector2Util.TryIntersect(from_a, from_b, to_a, to_b);

            var control = (Vector2)p;

            ///判定是否可绕过道路转角
            if (style == TurnStyle.Right)
            {
                {
                    var corner = toE.e.RightPathCorner.turn;
                    var corner_control = BezierUtil.GetControlPos(from_b, corner, to_b);
                    if (corner_control.DistanceByLine(from_b, control) < 0)
                    {
                        control = corner_control;
                    }
                }

                {
                    var corner = fromE.e.reE.LeftPathCorner.turn;
                    var corner_control = BezierUtil.GetControlPos(from_b, corner, to_b);
                    if (corner_control.DistanceByLine(from_b, control) < 0)
                    {
                        control = corner_control;
                    }
                }
            }
            else if (style == TurnStyle.Left)
            {
                {
                    var corner = toE.e.LeftPathCorner.turn;
                    var corner_control = BezierUtil.GetControlPos(from_b, corner, to_b);
                    if (corner_control.DistanceByLine(from_b, control) > 0)
                    {
                        control = corner_control;
                    }
                }
                {
                    var corner = fromE.e.reE.RightPathCorner.turn;
                    var corner_control = BezierUtil.GetControlPos(from_b, corner, to_b);
                    if (corner_control.DistanceByLine(from_b, control) > 0)
                    {
                        control = corner_control;
                    }
                }
            }
            LineTo(from_b);
            BezierTo(control, to_b);
        }
    }

}
