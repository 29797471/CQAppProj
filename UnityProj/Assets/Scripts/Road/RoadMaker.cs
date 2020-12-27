using System;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

public class RoadMaker
{

    public virtual void BuildTrafficDirectionMarking(string name, Transform parent, Vector2 pos, Vector2 dir, TurnStyle style)
    {
        Vector3 worldPos = pos.ToVector3();
        worldPos.y = 0.1f;
        worldPos = parent.TransformPoint(worldPos);
        
        Matrix4x4 mtr = Matrix4x4.TRS(worldPos, Quaternion.LookRotation(dir.ToVector3(), Vector3.up), Vector3.one);
        var obj = PathMapRule.instance.Arp.roadArgs_Others.turnSign.Clone(name, parent);
        obj.transform.SetWorldMatrix(mtr);
        var list=obj.GetComponentsInChildren<ShowTurnStyle>(true);
        foreach(var it in list)
        {
            it.SetTurnStyle(style);
        }
    }

    /// <summary>
    /// 构造一个曲面将一个线条平滑连接到另一个线条的算法.
    /// </summary>
    public static CqCurve LineSmoothConnection(Ray2D a, Ray2D b, float aLeftWidth, float bLeftWidth)
    {
        if (a.origin.EqualsByEpsilon(b.origin)) return null;
        var curve = new CqCurve() { close = true };
        curve.points = new List<CqCurvePoint>();

        var a2 = new Ray2D(a.origin + a.direction.Rot90() * aLeftWidth, a.direction);
        var b2 = new Ray2D(b.origin + b.direction.Rot90() * bLeftWidth, b.direction);

        if (a.direction.EqualsByEpsilon(b.direction))
        {
            var center = (a.origin + b.origin) / 2;
            var center2 = (a2.origin + b2.origin) / 2;
            curve.points.Add(new CqCurvePoint() { point = a2.origin });
            curve.points.Add(new CqCurvePoint() { point = center2 });
            curve.points.Add(new CqCurvePoint() { point = b2.origin });
            curve.points.Add(new CqCurvePoint() { point = b.origin });
            curve.points.Add(new CqCurvePoint() { point = center });
            curve.points.Add(new CqCurvePoint() { point = a.origin });

            var disByDir = Vector3.Project(a.origin - b.origin, a.direction).magnitude;
            if(!a2.GetPoint(disByDir/2).EqualsByEpsilon(center2))
            {
                curve.SetOutInTangent(0, a2.GetPoint(disByDir / 4));
                curve.SetOutInTangent(1, b2.GetPoint(-disByDir / 4));
            }

            if (!a.GetPoint(disByDir / 2).EqualsByEpsilon(center))
            {
                curve.SetOutInTangent(3, b.GetPoint(-disByDir / 4));
                curve.SetOutInTangent(4, a.GetPoint(disByDir / 4));
            }
        }
        else
        {
            if(a2.origin.EqualsByEpsilon(b2.origin))//曲边三角形
            {
                var cross = (Vector2)a.TryIntersect(b);
                curve.points.Add(new CqCurvePoint() { point = a2.origin });
                curve.points.Add(new CqCurvePoint() { point = b.origin });
                curve.points.Add(new CqCurvePoint() { point = a.origin });
                curve.SetOutInTangent(1, cross);
            }
            else//弯曲管道
            {
                var cross = (Vector2)a.TryIntersect(b,0);
                var cross2 = (Vector2)a2.TryIntersect(b2,0);

                curve.points.Add(new CqCurvePoint() { point = a2.origin });
                curve.points.Add(new CqCurvePoint() { point = b2.origin });
                curve.points.Add(new CqCurvePoint() { point = b.origin });
                curve.points.Add(new CqCurvePoint() { point = a.origin });
                curve.SetOutInTangent(0, cross2);
                curve.SetOutInTangent(2, cross);
            }
        }
        return curve;
    }


    void BaseMake(string name,Transform parent,CqCurve curve, RoadType type,Action<GameObject> callBack=null)
    {
        if (curve == null)
        {
            return;
        }
        var makeElement = PathMapRule.instance.Arp.GetMakeElement(type);
        //ToY_0_Plane
        foreach (var it in curve.points)
        {
            it.point = ((Vector2)it.point).ToVector3();
            it.inVec = ((Vector2)it.inVec).ToVector3();
            it.outVec = ((Vector2)it.outVec).ToVector3();
        }
        var obj = new GameObject(name);
        obj.transform.SetParent(parent);
        var mr = obj.AddComponent<MeshRenderer>();
        var mf = obj.AddComponent<MeshFilter>();
        
        mf.mesh = MeshUtil.Create3DByPipeline(curve.ExportPolygon());
        var curveMono = obj.AddComponent<CqCurveMono>();
        curveMono.curve = curve;
        mr.material = makeElement.mat;
        if(callBack!=null) callBack(obj);
        obj.transform.position = Vector3.up * makeElement.y;
        
    }
    /// <summary>
    /// 构造一个曲面将一个线条平滑连接到另一个线条,生成弯道
    /// </summary>
    public void MakeCurveRoadLeft( string name, Transform parent, Vector2 a, Vector2 aDir, Vector2 c, Vector2 cDir,
        float aWidth, float cWidth, RoadType roadType)
    {
        /*
        if(false)
        {
            var xx = new GameObject();
            xx.transform.SetParent(parent);
            xx.name = "args";
            var mono = xx.AddComponent<CqCurveMono>();
            mono.curve = new CqCurve() { close = true, points = new List<CqCurvePoint>() };

            var aVec = aDir.Rot90().normalized * aWidth;
            var cvec = cDir.Rot90().normalized * cWidth;
            mono.curve.points.Add(new CqCurvePoint() { point = a });
            mono.curve.points.Add(new CqCurvePoint() { point = c });
            mono.curve.points.Add(new CqCurvePoint() { point = c + cvec });
            mono.curve.points.Add(new CqCurvePoint() { point = a + aVec });
            foreach (var it in mono.curve.points)
            {
                it.point = ((Vector2)it.point).ToVector3();
                it.inVec = ((Vector2)it.inVec).ToVector3();
                it.outVec = ((Vector2)it.outVec).ToVector3();
            }
        }
        */
        

        var curve=LineSmoothConnection(
            new Ray2D() { origin = a, direction = aDir },
            new Ray2D() { origin = c, direction = cDir },
            aWidth,cWidth);
        
        BaseMake(name, parent, curve, roadType);
    }

    /// <summary>
    /// 由a->b沿顺时针拓一定宽度,组成的矩形生成直道
    /// </summary>
    public virtual void BuildRoadElementRight( string name, Transform parent,
        Vector2 a, Vector2 b, float rightWidth,
        RoadType roadType)
    {
        var xx = new GameObject();
        xx.transform.SetParent(parent);
        xx.transform.position = a.ToVector3();
        xx.name = "a";
        xx = new GameObject();
        xx.transform.SetParent(parent);
        xx.transform.position = b.ToVector3();
        xx.name = "b";
        CqCurve outCurve = new CqCurve();
        outCurve.close = true;
        outCurve.points = new List<CqCurvePoint>();

        var outPoints = outCurve.points;

        var abDir = (b - a);

        var a2 = -abDir.Rot90().normalized * rightWidth + a;
        var b2 = -abDir.Rot90().normalized * rightWidth + b;

        outPoints.Add(new CqCurvePoint() { point = a, });
        outPoints.Add(new CqCurvePoint() { point = b, });
        outPoints.Add(new CqCurvePoint() { point = b2, });
        outPoints.Add(new CqCurvePoint() { point = a2, });

        BaseMake(name, parent, outCurve, roadType);
    }

    /// <summary>
    /// 公路划线接口-直线
    /// </summary>
    public void CreateRoadLine( string name, Transform parent, Vector2 a, Vector2 b, RoadType lineType, float roadLineWidth = 0.2f)
    {
        var xx = new GameObject();
        xx.transform.SetParent(parent);
        xx.name = "line";

        CqCurve outCurve = new CqCurve();
        outCurve.close = true;
        outCurve.points = new List<CqCurvePoint>();

        var outPoints = outCurve.points;

        var abDir = (b - a);
        //roadLineWidth = 3f;
        var halfWidthVec = abDir.Rot90().normalized * roadLineWidth/2;

        var a1 = a + halfWidthVec;
        var a2 = a - halfWidthVec;
        var b2 = b - halfWidthVec;
        var b1 = b + halfWidthVec;

        outPoints.Add(new CqCurvePoint() { point = a1, });
        outPoints.Add(new CqCurvePoint() { point = b1, });
        outPoints.Add(new CqCurvePoint() { point = b2, });
        outPoints.Add(new CqCurvePoint() { point = a2, });
        
        BaseMake(name, parent, outCurve, lineType, obj =>
        {
            var mp = obj.AddComponent<MaterialProperty>();
            mp.propertyName = "_LineLength";
            mp.FloatValue = abDir.magnitude;
        });
    }
    /// <summary>
    /// 公路划线接口-曲线
    /// </summary>
    public void CreateRoadLine(string name, Transform parent, Vector2 a, Vector2 b, Vector2 c, RoadType roadType, float roadLineWidth = 0.2f, float partWidth = 2f)
    {
        var curve = LineSmoothConnection(
            new Ray2D() { origin = a, direction = b-a },
            new Ray2D() { origin = c, direction = c-b },
            roadLineWidth, roadLineWidth);
        var dis = BezierUtil.Length(a, b, c, 100);
        BaseMake(name, parent, curve, roadType, obj =>
        {
            var mp = obj.AddComponent<MaterialProperty>();
            mp.propertyName = "_LineLength";
            mp.FloatValue = dis;
        });
    }

    /// <summary>
    /// 绘制胶囊
    /// </summary>
    public virtual void DrawCapsule(string name, Transform parent,
        Vector2 a, Vector2 b, float rad, Color color)
    {

        Vector2 dir = (b - a);

       // 中间方块

        // 半圆1

        // 半圆2
    }
    /// <summary>
    /// 生成3D文字
    /// </summary>
    public virtual void Make3DText( string name, Transform parent, Vector2 point, Vector2 dir, string text)
    {
        var quat = Quaternion.LookRotation(Vector3.down, dir.ToVector3());
        var p = point.ToVector3();
        p.y = 1f;
        var drawMtr = Matrix4x4.TRS(p, quat, Vector3.one);
        var obj = new GameObject();
        obj.name = "位置&Id";
        obj.transform.SetParent(parent);
        obj.transform.SetWorldMatrix(drawMtr);
        var tm=obj.AddComponent<TextMesh>();
        tm.text = text;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.red;
        tm.fontSize = 50;
        var ubds=obj.AddComponent<UpdateByDrawStyle>();
        ubds.style=RoadDrawStyle.RoadPoint;
        
    }
    /// <summary>
    /// 绘制圆圈
    /// </summary>
    public virtual void DrawCirCle(string name, Transform parent,
        Vector2 a, float rad, float lineWidth, Color color)
    {
    }
    /// <summary>
    /// 生成区域3D文字
    /// </summary>
    public virtual void MakeRange3DText(string name, Transform parent, Vector2 point, Vector2 dir, string text)
    {
    }
    /// <summary>
    /// 绘制实线
    /// </summary>
    public virtual void DrawLine( string name, Transform parent,
        Vector2 a, Vector2 b, float lineWidth, Color color)
    {
        DrawAssisLine(name, parent, a, b, lineWidth, color, false);
    }

    public void DrawDottedLine( string name, Transform parent,
        Vector2 a, Vector2 b, float Space, float lineWidth, Color color, float partWidth = 1f)
    {
    }
    /// <summary>
    /// 绘制虚线
    /// </summary>
    public virtual void DrawDottedLine(string name, Transform parent,
        Vector2 a, Vector2 b, float lineWidth, Color color, float partWidth = 1f)
    {
        DrawAssisLine(name, parent, a, b, lineWidth, color, true, partWidth);
    }
    private void DrawAssisLine(string name, Transform parent,
        Vector2 a, Vector2 b, float lineWidth, Color color, bool bDottedLine, float partWidth = 1f)
    {
    }

    /// <summary>
    /// 生成杂物
    /// </summary>
    public virtual void MakeOthers(string name, Transform parent, Vector2 point, Vector2 dir, RoadType roadType, GameObject temp = null)
    {
        var arp = PathMapRule.instance.Arp;
        var quat = Quaternion.LookRotation(dir.ToVector3(), Vector3.up);
        float y = 0f;
        switch (roadType)
        {
            case RoadType.RT_Pavement:
                y = arp.roadArgs_Make.crosswalkHeight;
                break;
            case RoadType.RT_ManholeCover:
                y = arp.roadArgs_Others.manholeCoverHeight;
                break;
            default:
                y = arp.roadArgs_Make.roadHeight;
                break;
        }
        var mat = Matrix4x4.TRS(point.ToVector3() + Vector3.up * y, quat, Vector3.one);
        GameObject obj = null;
        switch (roadType)
        {
            case RoadType.RT_StreetLamp: //路灯
                {
                    obj = PathMapRule.instance.Arp.roadArgs_Others.streetLight.Clone(name, parent);
                    
                    break;
                }
            case RoadType.RT_ManholeCover://井盖
                {
                    obj = PathMapRule.instance.Arp.roadArgs_Others.manholeCover.Clone(name, parent);
                    break;
                }
            case RoadType.RT_TrafficLight_Car://红绿灯
                {
                    obj = PathMapRule.instance.Arp.roadArgs_Others.trafficLight_Car.Clone(name, parent);
                    break;
                }
            case RoadType.RT_TrafficLight_Person:
                {
                    obj = PathMapRule.instance.Arp.roadArgs_Others.trafficLight_Person.Clone(name, parent);
                    break;
                }
        }
        obj.transform.SetWorldMatrix(mat);
    }
}
