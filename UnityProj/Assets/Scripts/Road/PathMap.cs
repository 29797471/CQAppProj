using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道路元素(路点,路线)
/// </summary>
public interface PathElement
{

}

public class PathMap
{

    /// <summary>
    /// 路点
    /// </summary>
    public List<PathV> list = new List<PathV>();

    /// <summary>
    /// 道路元素生成器
    /// </summary>
    public RoadMaker roadMaker;

    /// <summary>
    /// 生成对象根节点
    /// </summary>
    Transform root;
    /// <summary>
    /// 路线
    /// </summary>
    public List<PathE> links = new List<PathE>();

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="roadMaker">道路元素生成器</param>
    /// <param name="parent">生成对象根节点</param>
    public void Init(RoadMaker roadMaker, Transform root)
    {
        this.roadMaker = roadMaker;
        this.root = root;
    }

    public void ClearAll()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].Remove();
        }
        for (int i = links.Count - 1; i >= 0; i--)
        {
            links[i].Remove();
        }
        links.Clear();
        list.Clear();
    }
    public void ReCalcAll()
    {
        for (int i = 0; i < list.Count; i++)
        {
            var it = list[i];
            it.ReCalc();
        }
    }
    /// <summary>
    /// 用于场景切换时清除道路生成模型
    /// </summary>
    public void ClearMake()
    {
        for (int i = 0; i < list.Count; i++)
        {
            var it = list[i];
            it.ClearMake();
        }

        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            it.ClearMake();
        }
    }
    public void MakeAll()
    {
        for (int i = 0; i < list.Count; i++)
        {
            var it = list[i];
            it.Make(roadMaker, root);
        }

        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            it.Make(roadMaker, root);
        }
        makeList.Clear();
    }



    /// <summary>
    /// 查询位置符合的路点
    /// </summary>
    public PathV FindPathV(Vector2 a)
    {
        return list.Find(x => x.pos == a);
    }

    /// <summary>
    /// 由任意点拾取一条道路(用于鼠标点选)
    /// 如果没有返回null
    /// </summary>
    public PathE SelectLink(Vector2 p)
    {
        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            if (p.InRangeX(it.Polygon) != PolyPointRelations.Outside )
            {
                return it;
            }
        }
        return null;
    }

    /// <summary>
    /// 由任意点拾取一条道路(用于鼠标点选)(无视道路拥有者是谁)
    /// 如果没有返回null
    /// </summary>
    public PathE SelectLinkX(Vector2 p)
    {
        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            if (p.InRangeX(it.Polygon) != PolyPointRelations.Outside)
            {
                return it;
            }
        }
        return null;
    }

    /// <summary>
    /// 由道路上的一点返回一条道路(用于判定点在道路两端点连线上,用于道路合并等)
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    PathE FindPathE(Vector2 a)
    {
        for (int i = 0; i < links.Count; i++)
        {
            var e = links[i];
            if (e.InSegment(a))
            {
                return e;
            }
        }
        return null;
    }

    /// <summary>
    /// 由任意点,关联一个路点或者一条道路
    /// </summary>
    public PathElement GetPathElement(Vector2 a)
    {
        PathElement aPE = null;
        aPE = FindPathV(a);
        if (aPE == null)
        {
            aPE = FindPathE(a);
        }
        return aPE;
    }




    /// <summary>
    /// 由道路的起点终点选取一条道路
    /// 如果没有返回null
    /// </summary>
    public PathE SelectLinkBy2P(Vector2 p1, Vector2 p2)
    {
        for (int i = 0; i < links.Count; i++)
        {
            var it = links[i];
            if ((it.from.pos == p1 || it.from.pos == p2) && (it.to.pos == p1 || it.to.pos == p2))
            {
                return it;
            }
        }
        return null;
    }

    /// <summary>
    /// 由坐标获取这个点,没有这个点时创建,当在其中一条道路上时,将原道路拆分后分别连接到该点上.
    /// </summary>
    PathV AddOrGet(Vector2 a)
    {
        var Va = FindPathV(a);
        if (Va != null) return Va;

        Va = NewPathV(a);
        var e = FindPathE(a);
        if (e != null)
        {
            RemoveLinkByKeepPoint(e);

            var ee = LinkTo<PathE>(Va, e.from, e.style, true);
            
            ee = LinkTo<PathE>(Va, e.to, e.style, true);
        }
        return Va;
    }

    PathV NewPathV(Vector2 p)
    {
        PathV v = new PathV(p, this);
        list.Add(v);
        return v;
    }
    /// <summary>
    /// 由坐标获取这个点,没有这个点时创建,
    /// </summary>
    PathV AddOrGetWidthOutMerge(Vector2 a)
    {
        var Va = FindPathV(a);
        if (Va != null) return Va;
        Va = NewPathV(a);
        return Va;
    }
    /// <summary>
    /// 提供给地图初始化时添加道路,不检查路点合并
    /// 也提供给道路修建后的合法性检查
    /// </summary>
    public PathE AddLinkWidthOutMerge(Vector2 a, Vector2 b, RoadStyle style)
    {
        var Va = AddOrGetWidthOutMerge(a);

        var Vb = AddOrGetWidthOutMerge(b);

        var e = LinkTo<PathE>(Va, Vb, style, false);
        return e;
    }

    T LinkTo<T>(PathV a, PathV b, RoadStyle style, bool make) where T : PathE
    {
        if (make)
        {
            makeList.Add(a);
            makeList.Add(b);
        }
        var e = a.LinkTo<T>(b, style);
        var re = b.LinkTo<T>(a, style);
        e.reE = re;
        re.reE = e;

        return e;
    }
    /// <summary>
    /// 拆除道路,保留路点
    /// </summary>
    bool RemoveLinkByKeepPoint(PathE e)
    {
        //lock(e)
        //{
        var re = e.reE;
        e.Remove();
        re.Remove();
        makeList.Add(e.from);
        makeList.Add(e.to);
        //}
        return true;
    }
    /// <summary>
    /// 提供给用户添加道路时,检查路点合并
    /// </summary>
    public PathE AddLink(Vector2 a, Vector2 b, RoadStyle style, ulong playerId = 0)
    {
        var Va = AddOrGet(a);

        var Vb = AddOrGet(b);

        var e = LinkTo<PathE>(Va, Vb, style, true);
        Merge(Va);
        Merge(Vb);

        if (roadMaker != null)
        {
            DoMake();
            //EventMgr.RoadChanged.Notify(1, this);
        }
        return e;
    }

    /// <summary>
    /// 如果一个顶点只有两条连线,并且两条连线是同种类型,并且同方向,合并这个顶点
    /// 如果一个顶点没有连线,删除这个顶点
    /// </summary>
    public void Merge(PathV a, bool no_merage = false)
    {
        if (a.links.Count == 2)
        {
            if (no_merage) return;
            var line1 = a.links[0];
            var line2 = a.links[1];
            if (line1.style == line2.style && Vector2Util.EqualZero(line1.Dir + line2.Dir) )
            {
                
                //特殊路点的道路是不作合并的
                //if (!(line1 is PathE_HighSpeedEnter) && !(line2 is PathE_HighSpeedEnter) &&
                //    !(line1 is PathE_Facilities) && !(line2 is PathE_Facilities))
                {
                    RemoveLinkByKeepPoint(line1);
                    RemoveLinkByKeepPoint(line2);
                    RemovePathV(a);
                    var e = LinkTo<PathE>(line1.to, line2.to, line1.style, true);
                }
                
            }
        }
        else if (a.links.Count == 0)
        {
            RemovePathV(a);
        }
    }

    bool RemovePathV(PathV v)
    {
        v.Remove();
        //if (v.pos == PathMapRule.instance.roadEnterPos)
        //{
        //    RoadMgr.instance.SetNpcBornPathV(v);
        //}
        return true;
    }



    HashSet<PathV> makeList = new HashSet<PathV>();

    /// <summary>
    /// 生成道路资源
    /// </summary>
    void DoMake()
    {
        //Debug.LogError("生成" + makeList.Count);
        foreach (var v in makeList)
        {
            v.ReCalc();
        }

        foreach (var v in makeList)
        {
            if (v.Index != -1) v.Make(roadMaker, root);
            for (int i = 0; i < v.links.Count; i++)
            {
                var e = v.links[i];
                e.Make(roadMaker, root);
                e.reE.Make(roadMaker, root);
            }
        }
        makeList.Clear();
    }
    /// <summary>
    /// 拆除道路
    /// 当拆除道路后,路点没有再连接道路时,需要去除路点
    /// </summary>
    public bool RemoveLink(PathE e)
    {
        //lock(e)
        //{
        if (roadMaker != null)
        {
            RemoveLinkByKeepPoint(e);
            Merge(e.from);
            Merge(e.to);
            DoMake();
            //EventMgr.RoadChanged.Notify(2, this);
        }
        else
        {
            RemoveLinkByKeepPoint(e);
            Merge(e.from);
            Merge(e.to);
        }
        //}
        return true;
    }

    /// <summary>
    /// 拆除多段道路
    /// </summary>
    public bool RemoveLinkList(List<PathE> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            RemoveLinkByKeepPoint(list[i]);
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].from.Index != -1) Merge(list[i].from);
            if (list[i].to.Index != -1) Merge(list[i].to);
        }
        DoMake();
        //EventMgr.RoadChanged.Notify(2, this);
        return true;
    }
    /// <summary>
    /// 检查pathE是否还在
    /// </summary>
    /// <returns></returns>
    public bool CheckPathEValid(PathE pathE)
    {
        return pathE != null && links.IndexOf(pathE) >= 0;
    }
    /// <summary>
    /// 检查pathV是否还在
    /// </summary>
    /// <returns></returns>
    public bool CheckPathVValid(PathV pathV)
    {
        return pathV != null && list.IndexOf(pathV) >= 0;
    }
}
