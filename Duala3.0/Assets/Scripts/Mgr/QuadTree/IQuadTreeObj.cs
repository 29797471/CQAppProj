using UnityEngine;

/// <summary>
/// 四叉树管理对象
/// </summary>
public abstract class IQuadTreeObj
{
    /// <summary>
    /// 用于对碰撞列表排序(优先级按从小到大排列)
    /// 应用场景 - 当与射线碰撞计算时缓存发生碰撞的距离,用于对碰撞列表排序
    /// </summary>
    public float priority;

    /// <summary>
    /// 对象的范围
    /// </summary>
    public Rect rect { set; get; }

    /// <summary>
    /// 检查相交
    /// </summary>
    public virtual bool CheckIntersects(object sharp)
    {
        if(sharp is Vector2)
        {
            return rect.Contains((Vector2)sharp);
        }
        else if (sharp is Rect)
        {
            return rect.Intersects((Rect)sharp);
        }
        return false;
    }

    public virtual void OnAdd(QuadTreeNode node)
    {
    }
}
