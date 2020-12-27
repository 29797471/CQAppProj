using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 四叉树数据节点
/// </summary>
public class QuadTreeNode : ITreeDataNode<QuadTreeNode>
{
    public QuadTreeNode(Rect rect)
    {
        this.rect = rect;
        this.bounds = QuadTreeMgr.instance.ToMaxBounds(rect);
    }
    public List<IQuadTreeObj> objList { get; private set; }
    void AddObj(IQuadTreeObj obj, ICancelHandle cancelHandle = null)
    {
        if (objList == null) objList = new List<IQuadTreeObj>();
        objList.Add(obj);
        obj.OnAdd(this);
        if (cancelHandle != null)
        {
            cancelHandle.CancelAct += () => objList.Remove(obj);
        }
    }

    /// <summary>
    /// 范围
    /// </summary>
    public Rect rect { get; private set; }
    public Bounds bounds { get; private set; }

    public override string Name
    {
        get
        {
            return string.Format("{0}({1})", bounds.ToString(), Node.Layer);
        }
        set
        {
        }
    }

    /// <summary>
    /// 四叉树一分四算法
    /// </summary>
    public void Split()
    {
        var childWidth = rect.width / 2;
        var childHeight= rect.height / 2;
        var center = rect.center;
        System.Action<float,float> AddChild = (x,y) =>
        {
            var child = new TreeNode<QuadTreeNode>();
            child.Data = new QuadTreeNode(RectUtil.GetRect(x,y, childWidth, childHeight));
            Node.AddChildren(child);
        };
        AddChild(center.x-rect.width/4, center.y - rect.height / 4);
        AddChild(center.x+rect.width/4, center.y - rect.height / 4);
        AddChild(center.x+rect.width/4, center.y + rect.height / 4);
        AddChild(center.x-rect.width/4, center.y + rect.height / 4);
    }
    /// <summary>
    /// 添加到树中
    /// </summary>
    public bool Input(IQuadTreeObj obj,int maxLayer, ICancelHandle cancelHandle = null)
    {
        if (maxLayer == 0)
        {
            Node.Data.AddObj(obj, cancelHandle);
            return true;
        } 

        if (Node.IsLeaf()) Split();

        QuadTreeNode interChild =null;
        
        foreach(var child in Node.mChildren)
        {
            var inChildArea = child.Data.rect.Intersects(obj.rect);
            if (inChildArea)
            {
                if (interChild != null)
                {
                    //当物体范围同时与2个子节点相交时,放在这个节点上
                    Node.Data.AddObj(obj, cancelHandle);
                    return true;
                }
                else
                {
                    interChild = child.Data;
                }
            }
        }
        if (interChild != null)
        {
            return interChild.Input(obj,maxLayer-1, cancelHandle);
        }

        Debug.LogError("不在有限的范围定义内"+obj);
        return false;
    }

    public void CheckIntersects<T>(
        T p,
        List<IQuadTreeObj> list,
        Predicate<QuadTreeNode> Intersects,
        Predicate<IQuadTreeObj> Filter
        )
    {
        if (!Intersects(this)) return;
        
        QuadTreeMgr.instance.drawCheck.DrawLine(rect.position, rect.position + rect.size);
        QuadTreeMgr.instance.drawCheck.DrawLine(rect.position+ Vector2.Scale(rect.size,Vector2.right), rect.position + Vector2.Scale(rect.size, Vector2.up));
        if (objList != null)
        {
            foreach (var it in objList)
            {
                if (!list.Contains(it) && it.CheckIntersects(p))
                {
                    var index = list.FindIndex(x => x.priority >= it.priority);
                    if(index==-1)
                    {
                        list.Add(it);
                    }
                    else
                    {
                        list.Insert(index, it);
                    }

                }
            }
        }
        if (Node.mChildren != null)
        {
            foreach (var child in Node.mChildren)
            {
                child.Data.CheckIntersects(p, list, Intersects, Filter);
            }
        }
    }

}