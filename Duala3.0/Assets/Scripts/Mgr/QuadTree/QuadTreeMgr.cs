using CqCore;
using System;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// 场景管理四叉树
/// <para>通过按物件包围盒,来分类存放物体到四叉树节点,
/// 达到摄像机改变时确定要在屏幕内显示的四叉树节点,
/// 然后显示节点包含的物件
/// </para>
/// </summary>
public class QuadTreeMgr : Singleton<QuadTreeMgr>
{
    public TreeNode<QuadTreeNode> root;

    /// <summary>
    /// 最大深度
    /// (当地图是400*400时 最小4叉是 400/2^5=12.5,即12.5*12.5)
    /// </summary>
    int maxLayer;

    /// <summary>
    /// 传入y=0平面上的两个点,建立渲染区域盒子
    /// </summary>
    /// <param name="size"></param>
    public void Init(Rect rect,int maxLayer=5)
    {
        root = new TreeNode<QuadTreeNode>();
        root.Data = new QuadTreeNode(rect);
        this.maxLayer = maxLayer;

        drawNode = new HelpDraw();
        drawNode.color = Color.black;
        drawNode.HelpDrawStyle = HelpDrawStyle.Debug;

        drawObj = new HelpDraw();
        drawObj.color = Color.green;
        drawObj.HelpDrawStyle = HelpDrawStyle.Debug;

        drawCheck = new HelpDraw();
        drawCheck.color = Color.red;
        drawCheck.HelpDrawStyle = HelpDrawStyle.Debug;
    }

    /// <summary>
    /// 添加到树中
    /// </summary>
    public void Input(IQuadTreeObj obj,ICancelHandle cancelHandle=null)
    {
        root.Data.Input(obj,maxLayer, cancelHandle);
    }
    HelpDraw drawNode;
    HelpDraw drawObj;
    public HelpDraw drawCheck { get; private set; }
    public void Draw()
    {
        drawNode.Clear();
        drawObj.Clear();
        root.PreorderTraversal(x =>
        {
            if(x.Data.objList!=null)
            {
                foreach (var it in x.Data.objList)
                {
                    drawObj.DrawRect(it.rect);
                }
            }
            drawNode.DrawRect(x.Data.rect);
        });
    }

    /// <summary>
    /// 返回相交(相互不外离)的所有碰撞单位
    /// </summary>
    List<TData> GetIntersects<TData, TSharp>(
        TSharp rect,
        Predicate<QuadTreeNode> Intersects)
        where TData : IQuadTreeObj
    {
        var list = new List<IQuadTreeObj>();
        root.Data.CheckIntersects(rect, list, Intersects, x => x is TData && x.CheckIntersects(rect));
        
        return list.ConvertAll(x=>(TData)x);
    }

    /// <summary>
    /// 返回包含这个点的所有单位
    /// </summary>
    public List<T> GetIntersects<T>(Rect rect) where T : IQuadTreeObj
    {
        return GetIntersects<T, Rect>(rect,node => node.rect.Intersects(rect));
    }

    /// <summary>
    /// 返回包含这个点的所有单位
    /// </summary>
    public List<T> GetIntersects<T>(Vector2 p) where T:IQuadTreeObj
    {
        return GetIntersects<T, Vector2>(p,node => node.rect.Contains(p));
    }

    /// <summary>
    /// 返回包围盒相交的所有单位
    /// </summary>
    public List<T> GetIntersects<T>(Bounds bounds) where T : IQuadTreeObj
    {
        return GetIntersects<T, Bounds>(bounds,node => node.bounds.Intersects(bounds));
    }

    /// <summary>
    /// 返回与射线相交的所有单位
    /// </summary>
    public List<T> GetIntersects<T>(Ray ray) where T : IQuadTreeObj
    {
        return GetIntersects<T, Ray>(ray, node => node.bounds.IntersectRay(ray));
    }

    /// <summary>
    /// 将y=0平面上的一个矩形,在y方向上拓展到3D空间最大的包围盒
    /// </summary>
    public Bounds ToMaxBounds(Rect rect)
    {
        return new Bounds(rect.center.ToVector3(), rect.size.ToVector3() + Vector3.up * float.MaxValue);
    }
}

