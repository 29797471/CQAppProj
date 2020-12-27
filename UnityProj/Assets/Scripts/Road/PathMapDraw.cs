using System;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
///  基本几何图形绘制类
/// </summary>
public class PathMapDraw
{
    /// <summary>
    /// 移除生成对象的回调
    /// </summary>
    System.Action OnClearMake;
    bool debugDraw;

    HelpDraw helpDraw;
    public Color color;
    public float lineWidth;

    public bool isDottedLine;
    public float partWidth = 1f;

    Transform mParent;
    public Transform Parent
    {
        get
        {
            if(mParent==null)
            {
                var obj = new GameObject();
                obj.name = "PathMapDraw";
                mParent = obj.transform;
            }
            return mParent;
        }
        set
        {
            mParent = value;
        }
    }

    public float height
    {
        set
        {
            helpDraw.SetVector2ToVector3(x =>
            {
                return x.ToVector3() + Vector3.up * value;
            });
        }
    }
    public RoadMaker RoadMakerInst
    {
        get
        {
            return PathMapRule.instance.RoadMakerInst;
        }
    }
    public void Init(bool debugDraw)
    {
        this.debugDraw = debugDraw;
        helpDraw = new HelpDraw();
        if(debugDraw)
        {
            helpDraw.HelpDrawStyle = HelpDrawStyle.Debug;
        }
    }

    public void Clear()
    {
        helpDraw.Clear();
        if (OnClearMake != null)
        {
            OnClearMake();
            OnClearMake = null;
        }
        
        if (mParent != null)
        {
            GameObject.DestroyImmediate(mParent.gameObject);
            mParent = null;
        }
    }

    /// <summary>
    /// 绘制多边形
    /// </summary>
    /// <param name="poly"></param>
    public void DrawPolygon(List<Vector2> poly)
    {
        DrawCalc.DrawPolygon(poly, DrawLine);
    }

    /// <summary>
    /// 绘制线条
    /// </summary>
    public void DrawLine(Vector2 a, Vector2 b)
    {
        if (!debugDraw)
        {
            if(isDottedLine)
            {
                RoadMakerInst.DrawDottedLine("DottedLine", Parent, a, b, lineWidth, color, partWidth);
                //var space = lineWidth;
                //OnClearMake += RoadMakerInst.DrawDottedLine("DottedLine", Parent, a, b, space, lineWidth, color, partWidth);

                //var solidWidth = 10f;
                //OnClearMake += RoadMakerInst.DrawDottedLine("DottedLine", Parent, a, b, lineWidth, color, solidWidth, true);
            }
            else
            {
                RoadMakerInst.DrawLine("Line", Parent, a, b, lineWidth, color);
            }
        }
        else
        {
            helpDraw.color = color;
            if (isDottedLine)
            {
                helpDraw.DrawDottedLine(a, b, partWidth);
            }
            else helpDraw.DrawLine(a, b);

        }
    }
    
    /// <summary>
    /// 绘制任意角度的矩形
    /// </summary>
    public void DrawRect(Vector2 a, Vector2 b, float width)
    {
        DrawCalc.DrawRect(a, b, width, DrawLine);
    }

    /// <summary>
    /// 绘制圆
    /// </summary>
    public void DrawCirCle(float rad, Vector2 a, float _lineWidth=1f, int sampling = 21)
    {
        if (!debugDraw)
        {
            RoadMakerInst.DrawCirCle("CirCle", Parent, a, rad, _lineWidth, color);
        }
        else
        {
            helpDraw.color = color;
            helpDraw.DrawCirCle(rad, a,  sampling);
        }
    }
    /// <summary>
    /// 绘制胶囊
    /// </summary>
    public void DrawCapsule(Vector2 a, Vector2 b, float rad)
    {
        if (!debugDraw)
        {
            RoadMakerInst.DrawCapsule("Capsule", Parent, a, b, rad, color);
        }
        else
        {
            helpDraw.color = color;
            helpDraw.DrawCapsule(a, b, rad);
        }
    }
    /// <summary>
    /// 绘制由A指向B的线段并在中间有一个方向箭头
    /// </summary>
    public void DrawArrowLine(Vector2 a, Vector2 b,float arrowSize = 1f, float arrowPos = 0.618f)
    {
        if (a == b) return;
        arrowSize = Math.Min(arrowSize, Vector2.Distance(a, b) / 20);
        var center = Vector2.LerpUnclamped(a, b, arrowPos);
        DrawLine(a, b);
        DrawCalc.DrawArrow(center, b - a, arrowSize, DrawLine);
    }

    /// <summary>
    /// 绘制曲线
    /// </summary>
    public void DrawCurveLine(Vector2 a, Vector2 b, Vector2 c)
    {
        if (!debugDraw)
        {
            RoadMakerInst.CreateRoadLine("Curve", Parent, a, b, c,  RoadType.RT_SignleSolidLine, lineWidth);
        }
        else
        {
            helpDraw.color = color;
            helpDraw.DrawBezier(a, b, c,20);
        }
        
    }

    public void DrawText(string text,Vector2 p)
    {
        RoadMakerInst.Make3DText(text, Parent, p, Vector2.up, text);
    }

    public void DrawRangeText(string text,Vector2 p)
    {
        RoadMakerInst.MakeRange3DText(text, Parent, p, Vector2.up, text);
    }
}
