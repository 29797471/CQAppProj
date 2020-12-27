using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PathMapRule : Singleton<PathMapRule>
{
    /// <summary>
    /// 绘制准备删除的道路,返回删除道路的长度
    /// </summary>
    /// <param name="selectPathE"></param>
    public float DrawRemove(PathE selectPathE)
    {
        float removeDis=0;
        if (selectPathE != null)
        {
            drawInst.color = arp.roadArgs_Draw.removeRoad;
            removeDis = selectPathE.Distance;
            drawInst.DrawCapsule(selectPathE.from.pos, selectPathE.to.pos,
                selectPathE.style.road_width / 2);

            
        }
        return removeDis;
    }


    public void DrawCarRoad(PathE fromE)
    {
        if (fromE == null) return;
        var roadArgs_Draw = instance.Arp.roadArgs_Draw;
        drawInst.color = roadArgs_Draw.carRoadLine;
        Vector2 a, b, p;
        var v = fromE.to;
        var fromCount = fromE.style.lanes / 2 - 1;
        for (int j = 0; j < v.links.Count; j++)
        {
            var toE = v.links[j];
            var toCount = toE.style.lanes / 2 - 1;
            if (fromE != toE)
            {
                for (int fromindex = 0; fromindex < fromCount; fromindex++)
                {
                    var list = fromE[fromindex].PathRoadNext();
                    for (int k = 0; k < list.Count; k++)
                    {
                        if (PathMapRule.instance.GetCarTurnCurve(fromE[fromindex], list[k],
                            out a, out b, out p))
                        {
                            drawInst.DrawBezier(a.ToVector3(), p.ToVector3(), b.ToVector3());
                        }
                        else
                        {
                            drawInst.lineWidth = roadArgs_Draw.drawLineWidth / 4;
                            drawInst.DrawArrowLine(a, p, roadArgs_Draw.drawLineWidth / 2);
                            drawInst.DrawArrowLine(p, b, roadArgs_Draw.drawLineWidth / 2);
                            drawInst.lineWidth = roadArgs_Draw.drawLineWidth;
                        }
                    }
                }
            }
        }
    }
    


    /// <summary>
    /// 绘制辅助点和线
    /// </summary>
    public void ReCalcHelpObjs(bool moveSecondPoint)
    {
        var helpLineMaxCount = arp.roadArgs_Draw.helpLineMaxCount;
        var helpLineLength = arp.roadArgs_Draw.helpLineLength;
        var mousePosIn = moveSecondPoint ? to : from;
        if (StateCheck(arp.drawStyle, RoadDrawStyle.RoadPoint))
        {
            //绘制路点
            for (int i = 0; i < PathMapInst.list.Count; i++)
            {
                var it = PathMapInst.list[i];
                var dis = Vector2.Distance(mousePosIn, it.pos);
                var alpha = RoadPointColorCalcFunc(dis);
                drawInst.color = new Color(1, 1, 1, alpha);
                drawInst.DrawCirCle(arp.roadArgs_Draw.drawRoadPointR, it.pos);
            }
        }
        if (StateCheck(arp.align, RoadAlign.AbsorbToHelpPoint) ||
            StateCheck(arp.drawStyle, RoadDrawStyle.Guideline))
        {
            
            var t = PathMapInst.FindPathV(mousePosIn);

            if (t == null)
            {
                ///辅助线的方向和连接的路点
                var dicV = new Dictionary<Vector2, PathV>();
                var dicK = new Dictionary<Vector2, float>();
                

                foreach (var it in PathMapInst.list)
                {
                    //对编辑起始终止点不作辅助线
                    if (it.pos == from || it.pos == to) continue;

                    
                    if (Vector2.Distance(it.pos, mousePosIn) <= helpLineLength * helpLineMaxCount)
                    {
                        foreach (var e in it.links)
                        {
                            var helpDir = e.Dir;
                            //每条道路产生3个垂直方向的辅助线
                            for (int j = 0; j < 3; j++)
                            {
                                helpDir = helpDir.Rot90();
                                var pathE = e.from.FindPathE(helpDir);
                                if (pathE != null) continue;
                                var help3 = e.from.pos + helpDir * helpLineLength * helpLineMaxCount;
                                Vector2 inter;
                                var _dis = mousePosIn.DistanceBySegment(e.from.pos, help3, out inter);
                                
                                var k = _dis / style.road_width;
                                if (k <= 1)
                                {
                                    //当辅助线和鼠标比较接近时比较同方向的辅助线,保留一条,比较规则
                                    //1.鼠标到辅助线的距离近的优先,2,距离相同时,比较鼠标和两辅助线起点的距离近的优先 
                                    if (!dicV.ContainsKey(helpDir) ||
                                        (dicK[helpDir] > k) ||
                                        Vector2.Distance(dicV[helpDir].pos, mousePosIn) > Vector2.Distance(e.from.pos, mousePosIn))
                                    {
                                        dicV[helpDir] = e.from;
                                        dicK[helpDir] = k;
                                    }
                                }
                            }
                        }
                    }
                }
                var listV = dicV.ToList();
                var listK = dicK.ToList();
                for (int i = 0; i < listV.Count; i++)
                {
                    var itV = listV[i];
                    var k = listK[i].Value;


                    Vector2 helpPoint = itV.Value.pos;

                    var xdis = Vector2.Distance(mousePosIn, helpPoint);
                    int drawCount = Mathf.CeilToInt(xdis / helpLineLength);
                    if(k.EqualZero())
                    {
                        drawInst.color = arp.roadArgs_Draw.pickupHelpLine;
                    }
                    else
                    {
                        drawInst.color = new Color(1, 1, 1, 1 - k + 0.2f);//k∈[0,1]
                    }
                    if (StateCheck(arp.drawStyle, RoadDrawStyle.Guideline))
                    {
                        helpPoint = itV.Value.pos;
                        var drawRoadPointR = arp.roadArgs_Draw.drawRoadPointR;
                        drawInst.DrawCirCle(drawRoadPointR, helpPoint);
                        for (int j = 0; j < drawCount; j++)
                        {
                            helpPoint += itV.Key * helpLineLength;
                            drawInst.DrawCirCle(drawRoadPointR, helpPoint);
                        }
                        drawInst.DrawLine(itV.Value.pos, helpPoint);
                    }
                    if (StateCheck(arp.align, RoadAlign.AbsorbToHelpLine))
                    {
                        helpSegments.Add(new CqCore.Segment() { a= itV.Value.pos,b= helpPoint });
                    }
                    if (StateCheck(arp.align, RoadAlign.AbsorbToHelpPoint))
                    {
                        var temp = itV.Value.pos;
                        for (int j = 0; j < drawCount; j++)
                        {
                            temp += itV.Key * helpLineLength;
                            helpPoints.Add(temp);
                        }
                    }
                }
            }
        }

        //绘制基本辅助胶囊
        {
            var rad = style.road_width / 2;

            if (moveSecondPoint)
            {
                if (AllowDrop)
                {
                    if (StateCheck(arp.align, RoadAlign.AbsorbToRoadLength) ||
                       StateCheck(arp.drawStyle, RoadDrawStyle.RoadLength))
                    {
                        var dir = (to - from).normalized;
                        var leftDelta = dir.Rot90() * (style.road_width + style.range);
                        var count = Mathf.RoundToInt(Vector2.Distance(from, to) / helpLineLength);
                        var a = from + leftDelta;
                        var helpPoint = from;
                        var b = from - leftDelta;

                        var forwardDelta = dir * helpLineLength;
                        if (StateCheck(arp.align, RoadAlign.AbsorbToRoadLength))
                        {
                            for (int i = 0; i < count; i++)
                            {
                                helpPoint += forwardDelta;

                                helpPoints.Add(helpPoint);
                            }
                        }
                        if (StateCheck(arp.drawStyle, RoadDrawStyle.RoadLength))
                        {
                            drawInst.color = Color.white;
                            for (int i = 0; i < count; i++)
                            {
                                drawInst.DrawLine(a += forwardDelta, b += forwardDelta);
                            }
                        }
                    }
                    drawInst.color = arp.roadArgs_Draw.addRoad;
                    drawInst.DrawCapsule(from, to, rad);
                    drawInst.DrawRect(from, to, style.range * 2 + style.road_width);
                }
                else
                {
                    if ((from - to).EqualZero())
                    {
                        drawInst.color = arp.roadArgs_Draw.addRoad;
                        drawInst.DrawCapsule(from, to, rad);
                    }
                    else
                    {
                        drawInst.color = arp.roadArgs_Draw.notAllowBuild;
                        drawInst.DrawCapsule(from, to, rad);
                    }
                }

                //绘制网格
                if (StateCheck(arp.drawStyle, RoadDrawStyle.Grid))
                {
                    var deltaGrid = arp.roadArgs_Draw.deltaGrid;
                    drawInst.color = new Color(1, 1, 1, 0.2f);
                    var center = ToGrid(to);
                    var n = helpLineLength / deltaGrid / 2;
                    var leftUp = center + (Vector2.left - Vector2.up) * deltaGrid * n;
                    var rightUp = center + (Vector2.right - Vector2.up) * deltaGrid * n;

                    var upLeft = center + (Vector2.up - Vector2.right) * deltaGrid * n;
                    var downLeft = center + (Vector2.down - Vector2.right) * deltaGrid * n;

                    var downDelta = Vector2.up * deltaGrid;
                    var rightDelta = Vector2.right * deltaGrid;
                    for (int i = 0; i < 2 * n - 1; i++)
                    {
                        drawInst.DrawLine(leftUp += downDelta, rightUp += downDelta);
                        drawInst.DrawLine(upLeft += rightDelta, downLeft += rightDelta);
                    }
                }
            }
            else
            {
                drawInst.color = arp.roadArgs_Draw.addRoad;
                drawInst.lineWidth = 2f;
                //道路宽度
                drawInst.DrawCirCle(rad, from);

                drawInst.lineWidth = 0.25f;
                //辐射范围
                drawInst.DrawCirCle(style.range + rad, from,101);

            }
        }
    }
}

