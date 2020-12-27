using UnityEngine;

/// <summary>
/// 添加删除道路
/// </summary>
public partial class PathMapRule : Singleton<PathMapRule>
{

    /// <summary>
    /// 提供给地图初始化时添加道路,不检查路点合并
    /// </summary>
    public PathE AddLinkWidthOutMerge(Vector2 a, Vector2 b, int styleId)
    {
        return PathMapInst.AddLinkWidthOutMerge(a, b, dicStyles[styleId]);
    }

    /// <summary>s
    /// 提供给地图其它玩家拆路,修路
    /// </summary>
    public PathE AddLink(Vector2 a, Vector2 b, int styleId, ulong playerId = 0)
    {
        return PathMapInst.AddLink(a, b, dicStyles[styleId], playerId);
    }
    public bool RemoveLink(Vector2 a, Vector2 b, int styleId, ulong playerId = 0)
    {
        var e = PathMapInst.SelectLinkBy2P(a, b);
        if (e != null) return RemoveRoad(e);
        return false;
    }
    public System.Action OnAddHightSpeed;

    /// <summary>
    /// 提供给地图初始化时添加道路完毕
    /// </summary>
    public void AddLinkWidthOutMergeComplete()
    {
        if (OnAddHightSpeed != null) OnAddHightSpeed();
        
        PathMapInst.ReCalcAll();
        PathMapInst.MakeAll();
    }

    /// <summary>
    /// 清除辅助线和辅助点
    /// 取消选中的道路
    /// </summary>
    public void Clear()
    {
        helpPoints.Clear();
        helpSegments.Clear();
        drawInst.Clear();
        drawError.Clear();

        lastSelect = null;
    }

    /// <summary>
    /// 清除道路数据
    /// </summary>
    public void ClearAll()
    {
        from = Vector2.zero;
        to = Vector2.zero;
        Clear();
        PathMapInst.ClearAll();
    }
}
