using UnityEngine;


/// <summary>
/// 两条路线构成一个转角
/// </summary>
public class PathCorner
{
    public PathE e;
    public PathE e_right;

    /// <summary>
    /// 边缘转弯点
    /// </summary>
    public Vector2 turn;

    /// <summary>
    /// 人行道内侧交点
    /// </summary>
    public Vector2 inside;
    /// <summary>
    /// 人行道外侧交点
    /// </summary>
    public Vector2 outside;

    /// <summary>
    /// 路边地表外侧交点
    /// </summary>
    public Vector2 subface_outside;

    /// <summary>
    /// 内侧转弯半径
    /// </summary>
    public float insideRad;
}