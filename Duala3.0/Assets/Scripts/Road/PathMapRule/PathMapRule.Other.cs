using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PathMapRule : Singleton<PathMapRule>
{
    #region 状态检查和判定
    public bool StateCheck(TurnStyle a, TurnStyle b)
    {
        return MathUtil.StateCheck((int)a, (int)b);
    }
    public TurnStyle StateAdd(TurnStyle a, TurnStyle b)
    {
        return (TurnStyle)MathUtil.StateAdd((int)a, (int)b);
    }
    TurnStyle StateDel(TurnStyle a, TurnStyle b)
    {
        return (TurnStyle)MathUtil.StateDel((int)a, (int)b);
    }
    bool StateCheck(RoadDrawStyle a, RoadDrawStyle b)
    {
        return MathUtil.StateCheck((int)a, (int)b);
    }
    bool StateCheck(RoadAlign a, RoadAlign b)
    {
        return MathUtil.StateCheck((int)a, (int)b);
    }
    #endregion
}
