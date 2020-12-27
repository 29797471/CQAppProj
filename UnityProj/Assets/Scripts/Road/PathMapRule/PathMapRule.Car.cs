using UnityEngine;

public partial class PathMapRule : Singleton<PathMapRule>
{
    /// <summary>
    /// 获得从一条车道转向到另一条车道的路线点
    /// 分成两种转弯算法,向左转时以人行横道中点开始,向右以交点回退内弯半径和开始.
    /// 有交点返回true(曲线),否则false(折线)
    /// </summary>
    public bool GetCarTurnCurve(PathERoad fromE, PathERoad toE,
        out Vector2 fromTurnPos, out Vector2 toTurnPos, out Vector2 midPos)
    {
        var len = Arp.roadArgs_Make.crosswalkLineDis + Arp.roadArgs_Make.crosswalk_len / 2;
        var from_a = fromE.ExitPoint;
        var from_b = from_a + fromE.e.Dir * len;
        var to_a = toE.EnterPoint;
        var to_b = to_a - toE.e.Dir * len;

        var style = GetTurnStyle(fromE.e, toE.e);

        if (style == TurnStyle.Straight)
        {
            midPos = (from_b + to_b) / 2;
            fromTurnPos = from_b;
            toTurnPos = to_b;
            return false;
        }
        else if(style== TurnStyle.Around)
        {
            var dis = Vector2.Distance(from_b, to_b);
            midPos = BezierUtil.GetControlPos(from_b, (from_b + to_b) / 2+fromE.e.Dir* dis/2, to_b);
            fromTurnPos = from_b;
            toTurnPos = to_b;
            return true;
        }
        else
        {
            var p = Vector2Util.TryIntersect(from_a, from_b, to_a, to_b);
            
            var control = (Vector2)p;
            fromTurnPos = from_b;
            toTurnPos = to_b;

            ///判定是否可绕过道路转角
            if (style == TurnStyle.Right)
            {
                {
                    var corner = toE.e.RightPathCorner.turn;
                    var corner_control= BezierUtil.GetControlPos(from_b, corner, to_b);
                    if(corner_control.DistanceByLine(from_b, control)<0)
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

            midPos = control;
            return true;
        }
    }

    /// <summary>
    /// 一条道路和另一条道路判定转向方向
    /// </summary>
    public TurnStyle GetTurnStyle(PathE from, PathE to)
    {
        if(from==to.reE)
        {
            return TurnStyle.Around;
        }
        var dir1 = from.Dir;
        var dir2 = to.Dir;
        if (Vector2.Angle(dir1, dir2) <= arp.roadArgs_Make.forwardAngle)
        {
            return TurnStyle.Straight;
        }
        else
        {
            return Vector2Util.Cross(dir1, dir2) > 0 ? TurnStyle.Left : TurnStyle.Right;
        }
    }

    /// <summary>
    /// 车道转向规则<para/>
    /// 传入这条路所有可行方向,以及对应车道,获取这条车道可以转弯的方向
    /// </summary>
    public TurnStyle GetAllowTurnStyle(TurnStyle turnStyle, int lanes, int carPathIndex)
    {
        //只在超车道调头
        if (StateCheck(turnStyle,TurnStyle.Around))
        {
            turnStyle = StateDel(turnStyle, TurnStyle.Around);

            if (carPathIndex == 0)
            {
                return StateAdd(GetAllowTurnStyle(turnStyle, lanes, carPathIndex),TurnStyle.Around);
            }
            else
            {
                return GetAllowTurnStyle(turnStyle, lanes, carPathIndex);
            }
        }
        //只有一个方向的转向,不用判定道路直接返回
        switch (turnStyle)
        {
            case TurnStyle.Left:
            case TurnStyle.Right:
            case TurnStyle.Straight:
            case TurnStyle.Around:
                return turnStyle;
        }
        
        //停车道转向标识
        if (lanes / 2 == carPathIndex + 1)
        {
            if (StateCheck(turnStyle, TurnStyle.Right))
            {
                return TurnStyle.Right;
            }
            else if (StateCheck(turnStyle, TurnStyle.Straight))
            {
                return TurnStyle.Straight;
            }
            else
            {
                return TurnStyle.Left;
            }
        }

        switch (lanes / 2)
        {
            case 2:
                {
                    break;
                }
            case 3:
                {
                    switch (carPathIndex)
                    {
                        case 0:
                            {
                                turnStyle= StateDel(turnStyle, TurnStyle.Right);
                                break;
                            }
                        case 1:
                            {
                                turnStyle= StateDel(turnStyle, TurnStyle.Left);
                                break;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (carPathIndex)
                    {
                        case 0:
                            {
                                turnStyle= StateDel(turnStyle, TurnStyle.Right);
                                break;
                            }
                        case 1:
                            {
                                if (StateCheck(turnStyle, TurnStyle.Straight))
                                {
                                    turnStyle= TurnStyle.Straight;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (StateCheck(turnStyle, TurnStyle.Right))
                                {
                                    turnStyle = TurnStyle.Right;
                                }
                                else
                                {
                                    turnStyle=StateDel(turnStyle, TurnStyle.Left);
                                }
                                break;
                            }
                    }
                    break;
                }
        }

        
        return turnStyle;
    }
}
