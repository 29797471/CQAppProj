using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路点
/// 分成5大类
/// 1.交叉路口(有人行横道线)
/// 2.道路尽头
/// 3.大小道相连 a.直线(有人行横道线) b.弯道(有人行横道线)
/// 5.同类型弯道相连
/// </summary>
public class PathV : PathElement
{
    /// <summary>
    /// 连接自己的道路数量
    /// </summary>
    public int myselfPathECount;

    /// <summary>
    /// 道路图
    /// </summary>
    public PathMap parent;

    /// <summary>
    /// 路点坐标
    /// </summary>
    public Vector2 pos;

    /// <summary>
    /// 与之相连的从这个路口出发的路线
    /// (顺时针排列)
    /// </summary>
    public List<PathE> links;


    /// <summary>
    /// 道路弯角列表
    /// </summary>
    public List<PathCorner> corners;

    /// <summary>
    /// 移除生成对象的回调
    /// </summary>
    protected Action OnClearMake;

    
    public PathV(Vector2 p, PathMap parent)
    {
        this.parent = parent;
        this.pos = p;
    }
    /// <summary>
    /// 通过道路方向查找道路
    /// </summary>
    public PathE FindPathE(Vector2 dir)
    {
        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            if (it.Dir == dir)
            {
                return it;
            }
        }
        return null;
    }


    public override string ToString()
    {
        return base.ToString() + Index;
    }

    public int Index
    {
        get
        {
            return parent.list.IndexOf(this);
        }
    }
    public void Remove()
    {
        parent.list.Remove(this);
        ClearMake();
    }
    public void ClearMake()
    {
        if (OnClearMake != null)
        {
            OnClearMake();
            OnClearMake = null;
        }
    }

    public T LinkTo<T>(PathV b, RoadStyle style) where T : PathE
    {
        if (links == null)
        {
            links = new List<PathE>();
        }

        var pe = AssemblyUtil.CreateInstance<T>(typeof(T), this, b, style);

        links.Add(pe);
        parent.links.Add(pe);


        ClearMake();
        return pe;
    }

    /// <summary>
    /// 顺时针排列连接的路点
    /// </summary>
    void Sort()
    {
        if (links.Count < 2)
            return;
        if (links.Count == 2)
        {
            if (!Vector2Util.IsPolyClockwise(links[0].to.pos, links[1].to.pos, pos))
            {
                links.Reverse();
                return;
            }
        }
        links.Sort((a) =>
        {
            return a.priority;
        });
    }


    /// <summary>
    /// 计算道路交汇处的拐点
    /// </summary>
    public void ReCalc()
    {

        ClearMake();
        Sort();
        corners = new List<PathCorner>();
        if (links.Count == 1)
        {
            var e = links[0];
            var vec = e.Dir.Rot90() * e.style.HalfRoadW;
            var vec_outside = e.Dir.Rot90() * e.style.HalfRoadAndSide;
            var vec_inside = e.Dir.Rot90() * e.style.HalfCarRoadW;

            var pc1 = new PathCorner();
            var pc2 = new PathCorner();
            pc1.outside = e.from.pos - vec;
            pc2.outside = e.from.pos + vec;

            pc1.subface_outside = e.from.pos - vec_outside;
            pc2.subface_outside = e.from.pos + vec_outside;

            pc1.inside = e.from.pos - vec_inside;
            pc2.inside = e.from.pos + vec_inside;

            pc1.turn = Vector2.zero;
            pc2.turn = Vector2.zero;

            corners.Add(pc1);
            corners.Add(pc2);
        }
        else
        {
            for (int i = 0; i < links.Count; i++)
            {
                var e = links.GetItemByRound(i);
                var e_right = links.GetItemByRound(i + 1);
                var pc = new PathCorner();
                pc.inside = GetCorner(e.to.pos, pos, e_right.to.pos,
                    e.style.HalfCarRoadW, e_right.style.HalfCarRoadW);

                //获取和这个顶点相连的两条边的夹角顶点
                // 从prev开始,顺时针之后到next点之间的夹角顶点
                pc.outside = GetCorner(e.to.pos, pos, e_right.to.pos,
                    e.style.HalfRoadW, e_right.style.HalfRoadW);

                pc.subface_outside = GetCorner(e.to.pos, pos, e_right.to.pos,
                    e.style.HalfRoadAndSide, e_right.style.HalfRoadAndSide);

                pc.turn = Vector2.zero;
                corners.Add(pc);
            }
        }

        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            it.Calc();
            it.reE.ClearMake();
        }
    }

    /// <summary>
    /// 当3个点构成一个角时,每边如果有宽度,返回角中的交点
    /// </summary>
    Vector2 GetCorner(Vector2 prevPos, Vector2 _pos, Vector2 nextPos, float prev_width, float next_width)
    {
        var prevVec = (prevPos - _pos).Rot90().normalized * prev_width;
        var p1 = prevPos - prevVec;
        var p2 = _pos - prevVec;

        var nextVec = (nextPos - _pos).Rot90().normalized * next_width;
        var p3 = nextPos + nextVec;
        var p4 = _pos + nextVec;

        var x = Vector2Util.TryIntersect(p1, p2, p3, p4);
        //平行时取两条平行线交汇的中点
        if (x == null)
        {
            return (p2 + p4) / 2;
        }
        return (Vector2)x;
    }

    /// <summary>
    /// 生成人行横道线,停车线和道路转弯标线
    /// </summary>
    protected virtual void MakeLine(RoadMaker roadMaker, Transform parent)
    {
        var arp = PathMapRule.instance.Arp;
        var roadArgs_Make = arp.roadArgs_Make;
        var halfCrosswalk = roadArgs_Make.crosswalk_len / 2;
        if (HasZebraCrossing())
        {
            //生成人行横道
            for (int i = 0; i < links.Count; i++)//交叉路口
            {
                var e = links[i];
                var halfRoad = e.style.HalfCarRoadW;
                //a,b,c,d从道路右侧起点,逆时针排列人行横道顶点
                {
                    var a = e.GetPoint(-halfCrosswalk, -halfRoad);
                    var b = e.GetPoint(halfCrosswalk, -halfRoad);
                    var c = e.GetPoint(halfCrosswalk, halfRoad);
                    var d = e.GetPoint(-halfCrosswalk, halfRoad);
                    //OnClearMake += roadMaker.CreateZebraCrossing("人行横道", parent, d, c, a, b);
                    roadMaker.CreateRoadLine("人行横道", parent,
                        e.GetPoint(0, halfRoad), e.GetPoint(0, -halfRoad),
                        RoadType.RT_ZebraCrossingLine, roadArgs_Make.crosswalk_len);
                };
            }
        }

        if (IsCrossroad())//交叉路口
        {
            var stopLineWidth = roadArgs_Make.stopLineWidth;
            for (int i = 0; i < links.Count; i++)
            {
                var e = links[i];

                var w1 = e.GetLinePoint(0, arp.roadArgs_Make.lineWidth / 2 + e.style.centerWidth / 2);
                var w2 = e.GetLinePoint(0, e.style.HalfCarRoadW);

                roadMaker.CreateRoadLine("白停车线", parent, w1, w2, RoadType.RT_SignleSolidLine, stopLineWidth);
            }
        }

        if (IsCurveRoad())
        {
            var lineWidth = roadArgs_Make.lineWidth;
            var e = links[0];
            var f = links[1];
            var style = e.style;

            //if (PathMapRule.instance.commonMap)
            //{
            //    if (e.playerId == 0)
            //    {
            //        yellowCol = roadArgs_Make.CommonYellowCol;
            //        whiteCol = roadArgs_Make.CommonWhiteCol;
            //    }
            //    else if (e.playerId != PathMapRule.instance.playerId)
            //    {
            //        yellowCol = roadArgs_Make.OtherYellowCol;
            //        whiteCol = roadArgs_Make.OtherWhiteCol;
            //    }
            //}

            {
                var w = style.centerWidth / 2;

                var w1 = style.HalfCarRoadW - style.sideDis;
                for (int j = 0; j < 2; j++)
                {
                    w *= -1;
                    roadMaker.CreateRoadLine(
                                "拐弯实线",
                               parent,
                               e.GetPoint(0, w),
                               Vector2.LerpUnclamped(pos, corners[1].outside, w * 2f / style.road_width),
                               f.GetPoint(0, -w),
                               RoadType.RT_CenterLine,
                               lineWidth);
                }
            }

            for (int i = 1; i < style.SideOfLanes; i++)
            {
                var w = i * style.car_road_width;
                for (int j = 0; j < 2; j++)
                {
                    w *= -1;
                    roadMaker.CreateRoadLine(
                               "拐弯虚线",
                               parent,
                               e.GetPoint(0, w),
                               Vector2.LerpUnclamped(pos, corners[1].outside, w * 2f / style.road_width),
                               f.GetPoint(0, -w),
                               RoadType.RT_SignleDottedLine,
                               lineWidth);
                }
            }
        }
    }
    /// <summary>
    /// 生成路面和人行道
    /// </summary>
    protected virtual void MakeRoadSurface(RoadMaker roadMaker, Transform parent)
    {
        var arp = PathMapRule.instance.Arp;

        var roadType = RoadType.RT_MainRoad;

        if (IsRoadEnd())
        {
            var e = links[0];
            var halfCarRoad = e.style.HalfCarRoadW;
            var crossing_width = e.style.crossing_width;
            {
                var delta = e.Dir * halfCarRoad;
                var delta90 = e.Dir.Rot90() * halfCarRoad;

                var a = pos + delta90;
                var b = pos + delta90 - delta;
                var c = pos - delta;

                roadMaker.MakeCurveRoadLeft("路面尽头", parent, a, b-a, c,c-b,
                   halfCarRoad, halfCarRoad, roadType);

                roadMaker.MakeCurveRoadLeft("人行尽头", parent,
                                c, b-c, a, a-b,
                                crossing_width, crossing_width, RoadType.RT_Pavement);
                var a2 = pos - delta90;
                var b2 = a2 - delta;

                roadMaker.MakeCurveRoadLeft("路面尽头", parent, c, b2-c, a2, a2-b2,
                    halfCarRoad, halfCarRoad, roadType);

                roadMaker.MakeCurveRoadLeft("人行尽头", parent,
                    a2, b2-a2, c, c-b2,
                    crossing_width, crossing_width, RoadType.RT_Pavement);
            }

        }
        else
        {
            for (int i = 0; i < links.Count; i++)
            {
                var e = links[i];
                var e_right = e.right;//右侧
                var dirA = e.Dir;
                var dirB = e_right.Dir;

                var prexName = e + "-" + e_right;
                {
                    {
                        var a = e.GetRight(0);
                        var b = e_right.GetLeft(0);
                        roadMaker.MakeCurveRoadLeft(
                            "路面弯道_" + prexName, parent,
                            a, -e.Dir, b, e_right.Dir,
                            e.style.HalfCarRoadW,
                            e_right.style.HalfCarRoadW,
                            roadType);
                        var corner = BezierUtil.LerpUnclamped(
                            e.GetRight(-e.style.CarRoadW / 2),
                            corners[i].inside,
                            e_right.GetLeft(e_right.style.CarRoadW / 2),
                            0.5f
                            );
                        corners[i].turn = Vector2.LerpUnclamped(corner, e.from.pos, 2f / Vector2.Distance(corner, e.from.pos));

                    }
                    {
                        var a = e.GetRight(-e.style.HalfCarRoadW);
                        var b = e_right.GetLeft(e_right.style.HalfCarRoadW);
                        roadMaker.MakeCurveRoadLeft(
                            "人行弯道" + prexName,
                            parent,
                            a, -e.Dir, b, e_right.Dir,
                             e.style.crossing_width,
                            e_right.style.crossing_width,
                            RoadType.RT_Pavement);
                    }
                }
            }
        }
    }
    
    public virtual void Make(RoadMaker roadMaker, Transform root)
    {
        if (OnClearMake != null) return;
        var obj = new GameObject();
        obj.name = ToString();
        obj.transform.SetParent(root, false);

        var parentTran = obj.transform;

        {
            roadMaker.Make3DText("位置&Id", parentTran, pos, Vector2.up,
            string.Format("{0} <color=#00ff00>{1}</color>", Index, pos));


            MakeLine(roadMaker, parentTran);

            MakeRoadSurface(roadMaker, parentTran);

            MakeOthers(roadMaker, parentTran);
        }

        OnClearMake += () => GameObject.Destroy(obj);
    }
    /// <summary>
    /// 限速牌,红绿灯
    /// </summary>
    protected virtual void MakeOthers(RoadMaker roadMaker, Transform parent)
    {
        var arp = PathMapRule.instance.Arp;
        if (!arp.roadArgs_Others.makeOthers) return;
        var roadSideDis = arp.roadArgs_Others.roadSideDis;
        if (HasZebraCrossing())
        {
            var halfCrosswalk = arp.roadArgs_Make.crosswalk_len / 2;
            //生成人行横道
            for (int i = 0; i < links.Count; i++)//交叉路口
            {
                var e = links[i];
                var halfRoad = e.style.HalfCarRoadW;
                //a,b,c,d从道路右侧起点,逆时针排列人行横道顶点
                {
                    roadMaker.MakeOthers( "红绿灯", parent,
                        e.GetPoint(halfCrosswalk, halfRoad + roadSideDis),
                        -e.Dir.Rot90(), RoadType.RT_TrafficLight_Car);
                    roadMaker.MakeOthers( "行人红绿灯", parent,
                        e.GetPoint(halfCrosswalk, -halfRoad - roadSideDis),
                        e.Dir.Rot90(), RoadType.RT_TrafficLight_Person);
                };
            }
        }
    }

    /// <summary>
    /// 路点包含人行横道线(交叉路口或者大小道相连)
    /// </summary>
    public bool HasZebraCrossing()
    {
        return (IsCrossroad() || BigSamllLink());
    }

    /// <summary>
    /// 交叉路口
    /// </summary>
    public bool IsCrossroad()
    {
        return links.Count > 2;
    }

    /// <summary>
    /// 大小道相连
    /// </summary>
    /// <returns></returns>
    public bool BigSamllLink()
    {
        return links.Count == 2 && links[0].style != links[1].style;
    }

    /// <summary>
    /// 弯道
    /// </summary>
    /// <returns></returns>
    public bool IsCurveRoad()
    {
        return links.Count == 2 && links[0].style == links[1].style && !(links[0].Dir + links[1].Dir).EqualZero();
    }

    /// <summary>
    /// 两条同类型的道路相连(不用换车道)
    /// </summary>
    /// <returns></returns>
    public bool IsSameDirectRoad()
    {
        return links.Count == 2 && links[0].style == links[1].style;
    }

    /// <summary>
    /// 道路的尽头
    /// </summary>
    /// <returns></returns>
    public bool IsRoadEnd()
    {
        return links.Count == 1;
    }

    /// <summary>
    /// 不同类型的道路组成的路口
    /// </summary>
    /// <returns></returns>
    public bool HasDiffStyle()
    {
        if (links.Count > 1)
        {
            var temp = links[0].style;
            for (int i = 1; i < links.Count; i++)
            {
                if (temp != links[i].style) return true;
            }
        }
        return false;
    }

}