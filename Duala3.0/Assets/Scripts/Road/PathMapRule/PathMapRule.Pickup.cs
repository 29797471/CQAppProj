using CqCore;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 吸附:添加道路时将操作点吸附到指定位置,辅助正确生成道路
/// 拾取:删除道路时,由操作点拾取一条道路来删除
/// </summary>
public partial class PathMapRule : Singleton<PathMapRule>
{
    /// <summary>
    /// 重新拾取一个终点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool SelectBuildPos(Vector2 pos, float delta)
    {
        Vector2 xx;
        if ((pos - to).EqualZero(delta))
        {
            return true;
        }
        else if ((pos - from).EqualZero(delta))
        {
            var temp = to;
            to = from; from = temp;
            return true;
        }
        else if (pos.DistanceBySegment(from, to, out xx) < delta)
        {
            to = pos;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 起点吸附
    /// 1.吸附到路点
    /// 2.吸附到路线
    /// 3.转网格点
    /// </summary>
    void AbsorbFromPoint(ref Vector2 frist)
    {
        Vector2? p;
        if ((p = PickupPoint(frist)) != null)
        {
            frist = (Vector2)p;
        }
        else
        {
            if (StateCheck(arp.align, RoadAlign.AbsorbToGrid))
            {
                frist = ToGrid(frist);
            }
            if ((p = PickupLineByFrom(frist)) != null)
            {
                frist = (Vector2)p;
            }
        }
    }
    /// <summary>
    /// 终点吸附
    /// 1.吸附到路点
    /// 2.吸附到路线
    /// 3.45度直线吸附
    /// 4.带方向的网格吸附
    /// 5.辅助线吸附(如果吸附后不可建造时不作吸附)
    /// </summary>
    void AbsorbToPoint(ref Vector2 second, Vector2 frist)
    {
        Vector2? p;
        if ((p = PickupPoint(second)) != null)
        {
            second = (Vector2)p;
        }
        else if ((p = PickupLineByTo(second, frist)) != null)
        {
            second = (Vector2)p;
            if (StateCheck(arp.align, RoadAlign.AbsorbToAngle))
            {
                p = Close45(frist, second, style);
                if (p != null)
                {
                    second = (Vector2)p;
                }
            }
            if (StateCheck(arp.align, RoadAlign.AbsorbToGrid))
            {
                second = ToGrid(second);
            }
        }
        else
        {
            if (StateCheck(arp.align, RoadAlign.AbsorbToAngle))
            {
                p = Close45(frist, second, style);
                if (p != null)
                {
                    second = (Vector2)p;
                }
            }
            if (StateCheck(arp.align, RoadAlign.AbsorbToGrid))
            {
                second = ToGrid(second);
            }
            if (StateCheck(arp.align, RoadAlign.AbsorbToHelpLine))
            {
                if ((p = AbsorbToHelpLineByEnd(second, frist, arp.roadArgs_Draw.helpLineAbsorbDis)) != null)
                {
                    int errorState = 0; System.Action act = null;
                    if (CheckRoad(frist, (Vector2)p, ref errorState, ref act))
                    {
                        second = (Vector2)p;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 添加道路时,按道路样式将路点拾取到
    /// 1.路点上,或者
    /// 2.一条最近的路线上
    /// 3.辅助点上
    /// </summary>
    PathElement CloseTo(ref Vector2 point, RoadStyle style, Vector2? dir = null)
    {
        //路点拾取
        for (int i = 0; i < PathMapInst.list.Count; i++)
        {
            var it = PathMapInst.list[i];
            if (Vector2.Distance(point, it.pos) < style.road_width)
            {
                point = it.pos;
                return it;
            }
        }

        if (dir == null)
        {
            //路线拾取
            for (int i = 0; i < PathMapInst.links.Count; i++)
            {
                var it = PathMapInst.links[i];
                Vector2 inter;
                var width = it.style.road_width / 2 + style.road_width / 2;
                var tt = Mathf.Abs(point.DistanceBySegment(it.from.pos, it.to.pos, out inter));
                if (tt < width)
                {
                    point = inter;
                    return it;
                }
            }
        }
        else
        {
            Vector2 a = point;
            Vector2 b = a + (Vector2)dir;
            //路线拾取
            for (int i = 0; i < PathMapInst.links.Count; i++)
            {
                var it = PathMapInst.links[i];
                Vector2 inter;
                var width = it.style.road_width / 2 + style.road_width / 2;
                var tt = Mathf.Abs(point.DistanceBySegment(it.from.pos, it.to.pos, out inter));
                if (tt < width)
                {
                    var temp = Vector2Util.TryIntersect(a, b, it.from.pos, it.to.pos, 0);
                    if (temp != null)
                    {
                        point = ((Vector2)temp).Round(3);
                        return it;
                    }
                }
            }

        }


        //辅助点拾取
        for (int i = 0; i < helpPoints.Count; i++)
        {
            var pos = helpPoints[i];
            if (Vector2.Distance(point, pos) < style.road_width / 2)
            {
                point = pos;
                return null;
            }
        }
        return null;
    }

    /// <summary>
    /// 吸附路点
    /// </summary>
    Vector2? PickupPoint(Vector2 point)
    {
        for (int i = 0; i < PathMapInst.list.Count; i++)
        {
            var it = PathMapInst.list[i];
            var maxRoadWidth = 0f;
            for (int j = 0; j < it.links.Count; j++)
            {
                maxRoadWidth = Mathf.Max(maxRoadWidth, it.links[j].style.road_width);
            }
            if (Vector2.Distance(point, it.pos) < maxRoadWidth)
            {
                return it.pos;
            }
        }
        return null;
    }

    /// <summary>
    /// 起点吸附到路线
    /// </summary>
    Vector2? PickupLineByFrom(Vector2 point)
    {
        for (int i = 0; i < PathMapInst.links.Count; i++)
        {
            var it = PathMapInst.links[i];
            Vector2 inter;
            var width = it.style.road_width;
            var tt = Mathf.Abs(point.DistanceBySegment(it.from.pos, it.to.pos, out inter));
            if (tt < width)
            {
                return inter;
            }
        }
        return null;
    }
    /// <summary>
    /// 终点拾取到路线
    /// </summary>
    Vector2? PickupLineByTo(Vector2 _second, Vector2 _frist)
    {
        //路线拾取
        for (int i = 0; i < PathMapInst.links.Count; i++)
        {
            var it = PathMapInst.links[i];
            Vector2 inter;
            var width = it.style.road_width;
            var tt = Mathf.Abs(_second.DistanceBySegment(it.from.pos, it.to.pos, out inter));
            if (tt < width)
            {
                var temp = Vector2Util.TryIntersect(_second, _frist, it.from.pos, it.to.pos, 0);

                if (temp != null)
                {
                    //return ((Vector2)temp).Round(3);
                    var seg = new Segment() { a = it.from.pos, b = it.to.pos };
                    if (seg.InSegment((Vector2)temp))
                    {
                        return ((Vector2)temp).Round(3);
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 终点吸附到垂直辅助线
    /// </summary>
    Vector2? AbsorbToHelpLineByEnd(Vector2 a, Vector2 b, float width = 15f)
    {
        for (int i = 0; i < helpSegments.Count; i++)
        {
            var it = helpSegments[i];
            Vector2 inter;

            if (Vector2.Angle(a - b, it.a - it.b) != 90) continue;
            var tt = it.Distance(a, out inter);
            if (tt < width)
            {
                var temp = Vector2Util.TryIntersect(a, b, it.a, it.b, 0);
                if (temp != null)
                {
                    return ((Vector2)temp).Round(3);
                }
            }
        }
        return null;
    }


    /// <summary>
    /// 4个靠近的方向
    /// </summary>
    Vector2[] closeDirs = new Vector2[] { Vector2.up, Vector2.right, Vector2.one, new Vector2(1, -1) };

    /// <summary>
    /// 45度靠近算法(a起点,b终点)返回修改后的终点
    /// </summary>
    Vector2? Close45(Vector2 a, Vector2 b, RoadStyle mCurrentStyle)
    {
        var toDis = mCurrentStyle.toDis;
        if (toDis <= 0) return null;
        var dir = b - a;
        float dis = float.MaxValue;
        int index = -1;
        for (int i = 0; i < closeDirs.Length; i++)
        {
            var closeDir = closeDirs[i];
            var d = Mathf.Abs(b.DistanceByLine(a, a + closeDir));
            if (dis > d)
            {
                dis = d;
                index = i;
            }
        }
        if (index != -1 && dis < toDis)
        {
            return b.GetPedal(a, a + closeDirs[index]);
        }
        return null;
    }

    /// <summary>
    /// 对齐到网格坐标
    /// </summary>
    Vector2 ToGrid(Vector2 temp)
    {
        var deltaGrid = arp.roadArgs_Draw.deltaGrid;
        if (deltaGrid > 0)
        {
            temp /= deltaGrid;
            temp = temp.Round();
            temp *= deltaGrid;
        }
        return temp;
    }

    /// <summary>
    /// 对齐到网格坐标
    /// </summary>
    Vector2 ToGrid(Vector2 p, Vector2 start)
    {
        var deltaGrid = arp.roadArgs_Draw.deltaGrid;
        if (deltaGrid > 0)
        {
            if (p.x.EqualsByEpsilon(start.x))
            {
                var temp = p.y;
                temp /= deltaGrid;
                temp = Mathf.Round(temp);
                temp *= deltaGrid;
                return new Vector2(p.x, temp);
            }
            else if (p.y.EqualsByEpsilon(start.y))
            {
                var temp = p.x;
                temp /= deltaGrid;
                temp = Mathf.Round(temp);
                temp *= deltaGrid;
                return new Vector2(temp, p.y);
            }
            else
            {
                var temp = p;
                temp /= deltaGrid;
                temp = temp.Round();
                temp *= deltaGrid;
                return temp;
            }
        }
        return p;
    }

    PathE lastSelect;


    /// <summary>
    /// 由任意点选取一条道路,主要用于拆除道路
    /// 如果没有返回null
    /// </summary>
    public PathE SelectLink(Vector2 input)
    {
        PathE select = null;
        if (true)
        {
            select = PathMapInst.SelectLink(input);
        }
        
        if (select != lastSelect)
        {
            Clear();

            if (MathUtil.StateCheck(PathMapRule.instance.Arp.drawStyle, RoadDrawStyle.CrossRoadCarPath))
            {
                DrawCarRoad(select);
            }
            else
            {
                DrawRemove(select);
            }
            lastSelect = select;
        }
        return select;
    }

    
}
