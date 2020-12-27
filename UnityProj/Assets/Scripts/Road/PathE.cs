

using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 路线
/// 两个路点构成一条有方向的道路
/// </summary>
public class PathE : PathElement
{
    public PathERoad[] roads;

    /// <summary>
    /// 获取这条道路的其中一条机动车道,0开始为超车道
    /// </summary>
    public PathERoad this[int i]
    {
        get
        {
            return roads[i];
        }
    }
    /// <summary>
    /// 排序优先级
    /// 用于对多条连接到同一个点的道路按逆时针排序
    /// </summary>
    public float priority;

    /// <summary>
    /// 产生起点到终点的方向向量
    /// </summary>
    public Vector2 Dir { get; private set; }

    /// <summary>
    /// 路点间距离
    /// </summary>
    public float Distance { get; private set; }


    Segment segment;

    /// <summary>
    /// 来回道路的一边
    /// </summary>
    public bool OneSide
    {
        get { return from.Index > to.Index; }
    }
    /// <summary>
    /// 点在道路上或者路点上
    /// </summary>
    public bool InSegment(Vector2 p)
    {
        return segment.InSegment(p, 0.01f);
    }

    public PathE(PathV from, PathV to, RoadStyle style)
    {
        this.from = from;
        this.to = to;
        this.style = style;

        Dir = (to.pos - from.pos).normalized;
        Distance = Vector2.Distance(from.pos, to.pos);
        priority = Dir.x;
        if (Dir.y > 0)
        {
            priority += 5;
        }
        else
        {
            priority = -Dir.x;
        }
        segment = new Segment(from.pos, to.pos);

        roads = new PathERoad[style.lanes / 2];
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i] = new PathERoad() { e = this, index = i };
        }
    }

    public override string ToString()
    {
        return string.Format("PathE({0} -> {1})", from.Index, to.Index);
    }

    Vector2[] mPolygon;

    /// <summary>
    /// 得到这条路的外接矩形
    /// </summary>
    public Vector2[] Polygon
    {
        get
        {
            if (mPolygon == null)
            {
                mPolygon = new Vector2[4];
                var delta = Dir.Rot90() * style.road_width / 2;
                mPolygon[0] = from.pos;
                mPolygon[1] = from.pos - delta;
                mPolygon[2] = to.pos - delta;
                mPolygon[3] = to.pos;
            }
            return mPolygon;
        }
    }

    /// <summary>
    /// 道路样式
    /// </summary>
    public RoadStyle style;

    /// <summary>
    /// 起点
    /// </summary>
    public PathV from;

    /// <summary>
    /// 终点
    /// </summary>
    public PathV to;

    /// <summary>
    /// 移除生成对象的回调
    /// </summary>
    protected CancelHandle OnClearMake=new CancelHandle();

    /// <summary>
    /// 反向
    /// </summary>
    public PathE reE;

    /// <summary>
    /// 去除弯角后的有效起点
    /// </summary>
    public Vector2 Start
    {
        get
        {
            return GetPoint(0, 0);
        }
    }

    /// <summary>
    /// 道路有效长度(实际中心实线长度)
    /// </summary>
    public float RealLength
    {
        get
        {
            return Distance - (lineDisDelta + reE.lineDisDelta);
        }
    }

    /// <summary>
    /// 检查有效道路的长度是否满足最小宽度限制
    /// 有效长度以道路绘制实线的长度来计算
    /// </summary>
    public bool CheckCorrect()
    {
        return RealLength > PathMapRule.instance.Arp.roadArgs_Make.minYellowLineDis;
    }

    /// <summary>
    /// 这条道路右侧的道路
    /// </summary>
    public PathE right { get; private set; }

    /// <summary>
    /// 这条道路左侧的道路
    /// </summary>
    public PathE left { get; private set; }



    /// <summary>
    /// 斑马线中心点与起点的距离
    /// </summary>
    float centerDis;

    /// <summary>
    /// 道路右侧转弯点与起点的正向距离
    /// </summary>
    float rightDisDelta;

    /// <summary>
    /// 道路左侧转弯点与起点的正向距离
    /// </summary>
    float leftDisDelta;

    /// <summary>
    /// 实线起点与起点的距离
    /// </summary>
    float lineDisDelta;

    /// <summary>
    /// 转角交汇处回退距离
    /// </summary>
    float rightDis;
    /// <summary>
    /// 转角交汇处回退距离
    /// </summary>
    float leftDis;

    /// <summary>
    /// 道路起点左侧转弯角
    /// </summary>
    public PathCorner LeftPathCorner { get; private set; }

    /// <summary>
    /// 道路起点右侧转弯角
    /// </summary>
    public PathCorner RightPathCorner { get; private set; }

    /// <summary>
    /// 基于起点左右转角点,计算道路实际起点,左右转弯起点,实线起点距离
    /// </summary>
    public void Calc()
    {
        ClearMake();

        var _from_index = from.links.IndexOf(this);
        LeftPathCorner = from.corners.GetItemByRound(_from_index - 1);
        RightPathCorner = from.corners.GetItemByRound(_from_index);
        left = from.links.GetItemByRound(_from_index - 1);
        right = from.links.GetItemByRound(_from_index + 1);

        if (from.IsRoadEnd())
        {
            centerDis = 0f;
            leftDisDelta = 0f;
            rightDisDelta = 0f;
            lineDisDelta = 0f;
        }
        else
        {
            rightDis = Vector2.Dot(RightPathCorner.outside - from.pos, Dir);
            leftDis = Vector2.Dot(LeftPathCorner.outside - from.pos, Dir);

            var insideRad = style.insideRad;

            if (leftDis.EqualZero())
            {
                leftDisDelta = Mathf.Abs(style.road_width - left.style.road_width);
            }
            else
            {
                leftDisDelta = Mathf.Abs(leftDis) + insideRad;
            }
            if (rightDis.EqualZero())
            {
                rightDisDelta = Mathf.Abs(style.road_width - right.style.road_width);
            }
            else
            {
                rightDisDelta = Mathf.Abs(rightDis) + insideRad;
            }

            centerDis = Mathf.Max(leftDisDelta, rightDisDelta);

            lineDisDelta = centerDis;
            if (from.HasZebraCrossing())
            {
                var roadArgs_Make = PathMapRule.instance.Arp.roadArgs_Make;
                lineDisDelta += roadArgs_Make.crosswalk_len / 2 + roadArgs_Make.crosswalkLineDis;
            }
        }
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i].Calc();
        }
    }
    


    /// <summary>
    /// 基于路点,往道路方向和垂直方向偏移得到相对点
    /// </summary>
    Vector2 GetPos(float dirWidth, float dir90Width)
    {
        return from.pos + dirWidth * Dir + dir90Width * Dir.Rot90();
    }

    public Vector2 GetRightConer(float dir90Width)
    {
        return GetPos(rightDis, dir90Width);
    }
    public Vector2 GetLeftConer(float dir90Width)
    {
        return GetPos(leftDis, dir90Width);
    }

    /// <summary>
    /// 人行横道中心点为起点,求相对点
    /// </summary>
    public Vector2 GetPoint(float dirWidth, float dir90Width)
    {
        return GetPos(centerDis + dirWidth, dir90Width);
    }

    /// <summary>
    /// 道路起始实线端点为起点,求相对点
    /// </summary>
    public Vector2 GetLinePoint(float dirWidth, float dir90Width)
    {
        return GetPos(lineDisDelta + dirWidth, dir90Width);
    }

    /// <summary>
    /// 道路方向右边的转角投影在道路上,加上转弯半径后作为起点,求相对点
    /// </summary>
    public Vector2 GetRight(float dir90Width)
    {
        return GetPos(rightDisDelta, dir90Width);
    }

    /// <summary>
    /// 道路方向左边的转角投影在道路上,加上转弯半径后作为起点,求相对点
    /// </summary>
    public Vector2 GetLeft(float dir90Width)
    {
        return GetPos(leftDisDelta, dir90Width);
    }

    public bool IsRemoved { get; private set; }
    public void Remove()
    {
        IsRemoved = true;
        from.links.Remove(this);
        from.parent.links.Remove(this);
        ClearMake();
    }
    public void ClearMake()
    {
        OnClearMake.CancelAll();
    }

    public void Make(RoadMaker roadMaker, Transform root)
    {
        //保证不重复生成
        OnClearMake.CancelAll();
        var obj = new GameObject();
        obj.name = ToString();
        obj.transform.SetParent(root, false);

        {
            var test = new GameObject();
            test.name = "位置";
            test.transform.SetParent(obj.transform);
            test.transform.position = ((from.pos + to.pos) / 2).ToVector3();
        }

        var parent = obj.transform;

        
        {
            MakeLine(roadMaker, parent);
            MakeSign(roadMaker, parent);
            MakeRoadSurface(roadMaker, parent);
            MakeRoadCollider(roadMaker, parent);
            MakeOthers(roadMaker, parent);
        }
        OnClearMake.CancelAct += () => GameObject.Destroy(obj);
    }
    
    /// <summary>
    /// 生成路面和人行道
    /// </summary>
    protected virtual void MakeRoadSurface(RoadMaker roadMaker, Transform parent)
    {
        roadMaker.BuildRoadElementRight("人行直道", parent, GetRight(-style.HalfCarRoadW),
        reE.GetLeft(style.HalfCarRoadW),
        style.crossing_width, RoadType.RT_Pavement);


        var p1 = GetRight(0);
        var p2 = reE.GetLeft(0);
        if (from.IsCrossroad())
        {
            p1 = from.pos;
        }
        if (to.IsCrossroad())
        {
            p2 = to.pos;
        }
        
        {
            roadMaker.BuildRoadElementRight("路面", parent, p1, p2,
            style.CarRoadW / 2, RoadType.RT_MainRoad);
        }
    }

    /// <summary>
    /// 获取道路碰撞起点
    /// </summary>
    Vector2 GetColliderFromPoint()
    {
        Vector2 point = from.pos;
        var arp = PathMapRule.instance.Arp;
        var roadArgs_Make = arp.roadArgs_Make;
        //特殊处理,部分道路碰撞插入可建筑范围内的情况
        if (from.BigSamllLink())
        {
            if (roadArgs_Make.makeStyle == MakeAllowBuildStyle.AlignmentPoint) return point;
        }

        //当不是交叉路口时,要外延到最远的转角
        if (from.links.Count == 2)
        {
            if (centerDis > style.road_width / 2)
            {
                point = GetPos(-(style.insideRad + right.style.insideRad) / 2, 0);
            }
            else
            {
                point = GetPos(-centerDis + style.insideRad, 0);
            }
        }
        else if (from.links.Count == 1)
        {
            point = GetPos(-style.road_width / 2, 0);
        }
        //特殊处理道路碰撞占到辐射范围的情况
        if (from.IsCrossroad() && style.id >= 2)
        {
            point += Dir * style.insideRad;
        }
        return point;
    }

    /// <summary>
    /// 添加道路碰撞
    /// </summary>
    protected virtual void MakeRoadCollider(RoadMaker roadMaker, Transform parent)
    {
        if (from.Index > to.Index)
        {
            /*
             *  var collider = new BoxCollider() { name = ToString()};
            collider.width = style.road_width;
            collider.from = GetColliderFromPoint();
            collider.to = reE.GetColliderFromPoint();
            collider.ToBounds();
            OnClearMake += ColliderTreeMgr.instance.Add(collider);
             */

        }
    }
    public List<Vector2> allowRange;


    protected virtual void MakeLine(RoadMaker roadMaker, Transform parent)
    {
        var arp = PathMapRule.instance.Arp;
        var roadArgs_Make = arp.roadArgs_Make;
        var crosswalkLineDis = roadArgs_Make.crosswalkLineDis;
        var crosswalk = roadArgs_Make.crosswalk_len;
        var carStopDis = roadArgs_Make.carStopDis;
        var lineWidth = roadArgs_Make.lineWidth;
        var stopLineWidth = roadArgs_Make.stopLineWidth;

        //if (PathMapRule.instance.commonMap)
        //{
        //    if(playerId==0)
        //    {
        //        yellowCol = roadArgs_Make.CommonYellowCol;
        //        whiteCol = roadArgs_Make.CommonWhiteCol;
        //    }
        //    else if(playerId!=PathMapRule.instance.playerId)
        //    {
        //        yellowCol = roadArgs_Make.OtherYellowCol;
        //        whiteCol = roadArgs_Make.OtherWhiteCol;
        //    }
        //}


        //单向
        //if (from.Index > to.Index)
        //{
        //    双向四车道(含)以上的划双黄线
        //    var w = style.centerWidth / 2;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        w *= -1;
        //        OnClearMake += roadMaker.CreateRoadLine("黄实线", parent,
        //            GetLinePoint(from.IsCrossroad() ? -stopLineWidth / 2 : 0, -w),
        //            reE.GetLinePoint(to.IsCrossroad() ? -stopLineWidth / 2 : 0, w),
        //            yellowCol,
        //            RoadLineType.SignleSolid, lineWidth);
        //    }
        //}


        //if (from.Index > to.Index)
        {
            var w = style.centerWidth / 2;
            var pFrom = GetLinePoint(from.IsCrossroad() ? -stopLineWidth / 2 : 0, -w);
            var pTo = reE.GetLinePoint(to.IsCrossroad() ? -stopLineWidth / 2 : 0, w);

            //if (to.IsCrossroad())
            //{
            //    if (Vector2.Distance(pFrom, pTo) <= arp.roadArgs_Make.carMoveSignDis * 2)
            //    {
            //        OnClearMake += roadMaker.CreateRoadLine("黄虚线", parent, pFrom, pTo,
            //        RoadType.RT_SignleDottedLine, lineWidth);
            //    }
            //    else
            //    {
            //        var pToM = reE.GetLinePoint(-stopLineWidth / 2 + arp.roadArgs_Make.carMoveSignDis * 2, w);

            //        OnClearMake += roadMaker.CreateRoadLine("黄实线", parent, pFrom, pToM, 
            //        RoadType.RT_CenterLine, lineWidth);
            //        OnClearMake += roadMaker.CreateRoadLine("黄虚线", parent, pToM, pTo, 
            //        RoadType.RT_CenterDottedLine, lineWidth, 0.5f);
            //    }
            //}
            //else
            {

                roadMaker.CreateRoadLine("黄实线", parent, pFrom, pTo, 
                RoadType.RT_CenterLine, lineWidth);
            }

        }


        if (!to.IsCrossroad())
        {
            for (int i = 0; i < style.lanes / 2 - 1; i++)
            {
                var t = (i + 1.0f) * style.car_road_width;
                roadMaker.CreateRoadLine("白虚线", parent,
                    GetLinePoint(0, -t), reE.GetLinePoint(0, t),
                    RoadType.RT_SignleDottedLine, lineWidth);
            }
            //{
            //    var w = style.HalfCarRoadW - style.sideDis;
            //    OnClearMake += roadMaker.CreateRoadLine("白边线", parent,
            //            GetLinePoint(0, -w), reE.GetLinePoint(0, w),
            //            whiteCol,
            //            RoadType.RT_SignleSolidLine, lineWidth);
            //}
        }
        else
        {
            if (Distance > lineDisDelta + reE.lineDisDelta + carStopDis)
            {
                for (int i = 0; i < style.lanes / 2 - 1; i++)
                {
                    var w = (i + 1.0f) * style.car_road_width;

                    var midPos = reE.GetLinePoint(carStopDis, w);
                    roadMaker.CreateRoadLine("白虚线", parent,
                        GetLinePoint(0, -w), midPos,
                        RoadType.RT_SignleDottedLine, lineWidth);
                    roadMaker.CreateRoadLine("白实线", parent,
                        midPos, reE.GetLinePoint(stopLineWidth / 2, w),
                        RoadType.RT_SignleSolidLine, lineWidth);
                }
                //{
                //    var w = style.HalfCarRoadW - style.sideDis;
                //    OnClearMake += roadMaker.CreateRoadLine("白边线", parent,
                //            GetLinePoint(0, -w), reE.GetLinePoint(stopLineWidth / 2, w),
                //            whiteCol,
                //            RoadType.RT_SignleSolidLine, lineWidth);
                //}
            }
            else
            {
                for (int i = 0; i < style.lanes / 2 - 1; i++)
                {
                    var w = (i + 1.0f) * style.car_road_width;
                    roadMaker.CreateRoadLine("白实线", parent,
                        GetLinePoint(0, -w), reE.GetLinePoint(stopLineWidth / 2, w),
                        RoadType.RT_SignleSolidLine, lineWidth);
                }
                //{
                //    var w = style.HalfCarRoadW - style.sideDis;
                //    OnClearMake += roadMaker.CreateRoadLine("白边线", parent,
                //            GetLinePoint(0, -w), reE.GetLinePoint(stopLineWidth / 2, w),
                //            whiteCol,
                //            RoadType.RT_SignleSolidLine, lineWidth);
                //}
            }
        }

    }
    /// <summary>
    /// 包含虚实线
    /// </summary>
    public bool ContainSolid
    {
        get
        {
            return to.IsCrossroad() && (Distance > lineDisDelta + reE.lineDisDelta + PathMapRule.instance.Arp.roadArgs_Make.carStopDis);
        }
    }
    protected virtual void MakeSign(RoadMaker roadMaker, Transform parent)
    {
        var carMoveSignDis = PathMapRule.instance.Arp.roadArgs_Make.carMoveSignDis;
        var carMoveSignVec = Dir * carMoveSignDis;
        if (to.IsRoadEnd())
        {
            for (int i = 0; i < style.lanes / 2; i++)
            {
                var p = this[i].ExitPoint;
                p -= carMoveSignVec;
                roadMaker.BuildTrafficDirectionMarking("调头标线", parent, p, Dir, TurnStyle.Around);
            }
        }
        else if (to.IsCrossroad())
        {
            //当道路有效长度过小,不够显示道路转向标识时,不绘制道路转向标识
            if (RealLength > 2 * carMoveSignDis)
            {
                for (int i = 0; i < style.lanes / 2; i++)
                {
                    var s = this[i].AllowTurnStyle;
                    if (s != 0)
                    {
                        var p = this[i].ExitPoint;
                        p -= carMoveSignVec;
                        roadMaker.BuildTrafficDirectionMarking("行车方向标线", parent, p, Dir, s);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 生成路灯,杂物,井盖
    /// 杂物固定间距生成算法
    /// count*spacing+2*left(中间固定间隔,两头补余)
    /// </summary>
    protected virtual void MakeOthers(RoadMaker roadMaker, Transform parent)
    {
        var arp = PathMapRule.instance.Arp;

        if (!arp.roadArgs_Others.makeOthers) return;
        var lightSpacing = arp.roadArgs_Others.lightSpacing;
        var manholeCoverSpacing = arp.roadArgs_Others.manholeCoverSpacing;
        var manholeCoverK = arp.roadArgs_Others.manholeCoverK;
        var roadSideDis = arp.roadArgs_Others.roadSideDis;
        //路边杂物起点
        var p1 = GetLinePoint(0, -style.HalfCarRoadW - roadSideDis);
        //路边杂物终点
        var p2 = reE.GetLinePoint(0, style.HalfCarRoadW + roadSideDis);
        //得到黄实线的长度,路边对应生成杂物
        var dis = Vector2.Distance(p1, p2);
        if (dis > 0)
        {
            var Dir90 = Dir.Rot90();
            //将路灯基于线段(p1,p2)的中心点对称,均匀分布线段上,间隔spacing
            if (dis > lightSpacing)
            {
                var spacing = lightSpacing;
                var spacingVec = Dir * spacing;
                int count = Mathf.FloorToInt(dis / spacing);
                var left = (dis - count * spacing) / 2;
                var p = p1 + Dir * left;
                for (int i = 0; i <= count; i++)
                {
                    roadMaker.MakeOthers( "路灯", parent, p, Dir90, RoadType.RT_StreetLamp);

                    p += spacingVec;
                }

            }

            //单向
            if (from.Index > to.Index)
            {
                //转向标识的长度
                var roadSignLen = arp.roadArgs_Make.carMoveSignDis * 2;
                //由于井盖不能和道路方向标线重叠,起始终止点要调整
                p1 = GetLinePoint(roadSignLen, -style.HalfCarRoadW);
                p2 = reE.GetLinePoint(roadSignLen, style.HalfCarRoadW);
                dis -= roadSignLen * 2;

                if (dis > 0)
                {
                    ///垂直道路一个车道单位向量
                    var deltaManholeCover = Dir.Rot90() * style.car_road_width;
                    {
                        var spacing = manholeCoverSpacing;
                        var spacingVec = Dir * spacing;
                        int count = Mathf.FloorToInt(dis / spacing);
                        var left = (dis - count * spacing) / 2;
                        var p = p1 + Dir * left;

                        for (int i = 0; i <= count; i++)
                        {
                            //随机决定是否生成井盖
                            if (CqRandom.NextDouble() <= manholeCoverK)
                            {
                                //随机决定井盖生成在哪条车道
                                var index = RandomUtil.Random(0, style.lanes);
                                roadMaker.MakeOthers( "井盖", parent, p + (index + 1f / 2) * deltaManholeCover, Dir, RoadType.RT_ManholeCover);
                            }
                            p += spacingVec;
                        }
                    }

                }


                
            }
        }
    }
}