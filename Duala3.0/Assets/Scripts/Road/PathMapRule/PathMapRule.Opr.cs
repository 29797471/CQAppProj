using System.Collections.Generic;
using UnityEngine;

public partial class PathMapRule : Singleton<PathMapRule>
{
    

    /// <summary>
    /// 添加的道路是否合法(不合法时要绘制非法形状,所以要先清除绘制)<para/>
    /// 1.两点不重合<para/>
    /// 2.45度规则<para/>
    /// 3.是否与任一道路都相离<para/>
    /// 4.道路合法性测试<para/>
    /// </summary>
    public bool IsRightRoad()
    {
        int errorState=0;
        System.Action drawError=null;
        return CheckRoad(from, to, ref errorState,ref drawError);
    }

    /// <summary>
    /// 添加道路
    /// </summary>
    public bool AddRoad()
    {
        if (!IsRightRoad())
        {
            return false;
        }
        
        var e = PathMapInst.AddLink(from, to, style, 0);

        from = to;

        //fromElement = PathMapInst.FindPathV(to);

        AllowDrop = false;
        return true;
    }

    

    /// <summary>
    /// 拆除道路
    /// </summary>
    /// <param name="pathE"></param>
    public bool RemoveRoad(PathE pathE)
    {
        bool bl = false;
        
        {
            bl=PathMapInst.RemoveLink(pathE);
        }
        Clear();
        DrawRemove(null);
        return bl;
    }

    public void SetStyle(int id)
    {
        style = dicStyles[id];
    }
    public RoadStyle CurrentStyle
    {
        get
        {
            return style;
        }
    }
    public RoadStyle GetStyle(int id)
    {
        return dicStyles[id];
    }

    /// <summary>
    /// 可以左移,或者右移
    /// leftOrRight:true左移,false右移
    /// </summary>
    /// <returns></returns>
    public bool CanMove(bool leftOrRight)
    {
        var vec = (to- from).normalized.Rot90()* Arp.roadArgs_Draw.moveDeltaDis;
        if(to.x!=from.x && to.y!=from.y)
        {
            vec *= Mathf.Sqrt(2);
        }
        if (!leftOrRight) vec *= -1;
        return CanBuildRoad(from+vec, to+vec);
    }

    /// <summary>
    /// 获取箭头
    /// </summary>
    public Ray GetArrow(bool leftOrRight)
    {
        var center = (to + from) / 2;
        var vec = (to - from).normalized.Rot90() * style.HalfRoadW;
        if (!leftOrRight) vec *= -1;
        return new Ray((center + vec).ToVector3(), vec.ToVector3());
    }

    /// <summary>
    /// 平移
    /// </summary>
    public void Move(bool leftOrRight)
    {
        var vec = (to - from).normalized.Rot90() * Arp.roadArgs_Draw.moveDeltaDis;
        if (to.x != from.x && to.y != from.y)
        {
            vec *= Mathf.Sqrt(2);
        }
        if (!leftOrRight) vec *= -1;
        from += vec;
        to += vec;
        AllowDrop = true;
        Clear();
        ReCalcHelpObjs(true);
    }

    public Vector2 SetFrom(Vector2 from)
    {
        AbsorbFromPoint(ref from);
        this.to = this.from = from;

        Clear();
        AllowDrop = false;
        ReCalcHelpObjs(false);
        return this.from;
    }
    public bool AllowDrop { get; set; }

    public void FirstDown()
    {
        hasLastRight = false;
    }
    /// <summary>
    /// 有一条正确的道路
    /// </summary>
    bool hasLastRight;
    public Vector2 SetTo(Vector2 inTo)
    {
        

        var _to = to;
        var _AllowDrop = AllowDrop;
        //Debug.Log("前:"+inTo.x + ":" + inTo.y );
        AbsorbToPoint(ref inTo, from);
        //Debug.Log(inTo.x+":"+inTo.y);
        to = inTo;
        int errorState=0;
        System.Action DrawError=null;
        AllowDrop = CheckRoad(from,to,ref errorState,ref DrawError);
        ErrorState = errorState;
        //Debug.Log("ErrorState:" + ErrorState);
        if (ErrorState==1)
        {
            hasLastRight = false;
        }
        else if(ErrorState==0)
        {
            hasLastRight = true;
        }
        //一直都是错误的路
        if(!hasLastRight)
        {
            Clear();
            if (DrawError != null) DrawError();
            ReCalcHelpObjs(true);
        }
        else if(AllowDrop)
        {
            Clear();
            if(DrawError!=null) DrawError();
            ReCalcHelpObjs(true);
        }
        else
        {
            drawError.Clear();
            to = _to;
            AllowDrop = _AllowDrop;
            if (DrawError != null) DrawError();
        }
        return this.to;
    }
    /// <summary>
    /// 放手时(清除错误)
    /// </summary>
    public void OnUp()
    {
        if (hasLastRight) drawError.Clear();
    }
}


