using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一条跑道
/// </summary>
public class PathERoad
{
    public static Dictionary<int, PathERoad> dic = new Dictionary<int, PathERoad>();
    public PathERoad()
    {
        dic[this.GetHashCode()] = this;
    }
    ~PathERoad()
    {
        dic.Remove(GetHashCode());
    }
    public override string ToString()
    {
        return e.ToString() + "_" + index;
    }
    public PathE e;
    public int index;

    FastStack<PathERoad> nextWays = new FastStack<PathERoad>();
    FastStack<PathERoad> prevWays = new FastStack<PathERoad>();
    /// <summary>
    /// 进入点
    /// </summary>
    public Vector2 EnterPoint
    {
        get
        {
            //lock(e)
            //{
            return e.GetLinePoint(0, -e.style.GetLaneDis(index));
            //}
        }
    }

    /// <summary>
    /// 离开点
    /// </summary>
    public Vector2 ExitPoint
    {
        get
        {
            //lock(e)
            //{
            return e.reE.GetLinePoint(0, -e.style.GetLaneDis(-index - 1));
            //}
        }
    }

    /// <summary>
    /// 虚实交汇点
    /// </summary>
    public Vector2 ConfluencePoint
    {
        get
        {
            return e.reE.GetLinePoint(PathMapRule.instance.Arp.roadArgs_Make.carStopDis, -e.style.GetLaneDis(-index - 1));
        }
    }
    public void Calc()
    {

    }

    /// <summary>
    /// 从这条道开到另一条道的规则,包含:
    /// 1.道路尽头原车道调头
    /// 2.道路转弯原车道转向
    /// 3.大小道路转弯换道
    /// 4.交叉路口换道通行
    /// </summary>
    public FastStack<PathERoad> PathRoadNext()
    {
        //lock (e)
        //{
        nextWays.Clear();
        if (e.to.IsRoadEnd())
        {
            nextWays.Add(e.reE[index]);
        }
        else if (e.to.IsSameDirectRoad())
        {
            nextWays.Add(e.reE.right[index]);
        }
        else
        {
            TurnStyle turnStyle = AllowTurnStyle;
            for (int i = 0; i < e.to.links.Count; i++)
            {
                var it = e.to.links[i];

                if (PathMapRule.instance.StateCheck(turnStyle, PathMapRule.instance.GetTurnStyle(e, it)))
                {
                    //去除停车道
                    for (int j = 0; j < it.roads.Length - 1; j++)
                    {
                        nextWays.Add(it[j]);
                    }
                }
            }
        }
        return nextWays;
        //}
    }
    /// <summary>
    /// 从另一条道开到这条道的规则,包含:
    /// 1.道路尽头原车道调头
    /// 2.道路转弯原车道转向
    /// 3.大小道路转弯换道
    /// 4.交叉路口换道通行
    /// </summary>
    public FastStack<PathERoad> PathRoadPrev()
    {
        //lock(e)
        //{
        prevWays.Clear();
        if (e.from.IsRoadEnd())
        {
            prevWays.Add(e.reE[index]);
        }
        else if (e.from.IsSameDirectRoad())
        {
            prevWays.Add(e.right.reE[index]);
        }
        else
        {
            for (int i = 0; i < e.from.links.Count; i++)
            {
                var it = e.from.links[i].reE;
                //去除停车道
                for (int j = 0; j < it.roads.Length - 1; j++)
                {
                    if (PathMapRule.instance.StateCheck(it[j].AllowTurnStyle, PathMapRule.instance.GetTurnStyle(it, e)))
                    {
                        prevWays.Add(it[j]);
                    }
                }
            }
        }
        return prevWays;
        //}
    }

    /// <summary>
    /// 获取从这条道路可以转弯的方向
    /// </summary>
    public TurnStyle AllowTurnStyle
    {
        get
        {
            //lock(e)
            //{
            TurnStyle turnStyle = 0;
            for (int i = 0; i < e.to.links.Count; i++)
            {
                var it = e.to.links[i];
                turnStyle = PathMapRule.instance.StateAdd(turnStyle, PathMapRule.instance.GetTurnStyle(e, it));
            }
            return PathMapRule.instance.GetAllowTurnStyle(turnStyle, e.style.lanes, index);
            //}
        }
    }
}