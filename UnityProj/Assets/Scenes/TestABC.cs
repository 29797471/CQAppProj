using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityCore;
using UnityEngine;

public class TestABC : MonoBehaviour
{
    public enum CalcStyle
    {
        [EnumLabel("交集")]
        Intersection,
        [EnumLabel("并集")]
        Union,
        [EnumLabel("差集")]
        Sub,
    }
    List<CqCurveMono> list;

    [ComBox("样式", ComBoxStyle.RadioBox)]
    public CalcStyle style;

    [Button("测试"),Click("AA")]
    public string _1;

    public string tName;
    public float grid = 10f;
    public void AA()
    {
        var ary = GetComponents<CqCurveMono>();
        foreach(var it in ary)
        {

            DestroyImmediate(it);
        }
        list = this.GetComponentsInChildrenNoDeep<CqCurveMono>();

        var a = list[0].curve.points.ConvertAll(x => (Vector2)list[0].transform.localToWorldMatrix.MultiplyPoint(x.point));
        var b = list[1].curve.points.ConvertAll(x => (Vector2)list[1].transform.localToWorldMatrix.MultiplyPoint(x.point));
        //foreach(var it in list)
        //{
        //    foreach(var a in it.curve.points)
        //    {
        //        a.point = a.point.Rasterize(grid);
        //    }
        //}
        switch (style)
        {
            case CalcStyle.Intersection:
                {
                    SetCurve(Vector2Util.PolygonIntersection(a, b));
                    break;
                }
            case CalcStyle.Union:
                {
                    SetCurve(Vector2Util.PolygonUnion(a, b));
                    break;
                }
            case CalcStyle.Sub:
                {
                    var re=Vector2Util.PolygonSub(a, b);
                    foreach(var it in re)
                    {
                        SetCurve(it);
                    }
                    break;
                }
        }
    }

    void SetCurve(IList<Vector2> list)
    {
        var mono = this.gameObject.AddComponent<CqCurveMono>();
        mono.curve = new CqCurve();
        mono.curve.close = true;
        mono.curve.points = new List<CqCurvePoint>();
        var mat = mono.transform.worldToLocalMatrix;
        if(list!=null)
        {
            foreach (var it in list)
            {
                mono.curve.points.Add(new CqCurvePoint() { point = mat.MultiplyPoint(it) });
            }
        }
    }
    
}