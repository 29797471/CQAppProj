using UnityCore;
using UnityEngine;

public class TestBC : MonoBehaviour
{
    public CqCurveMono outMono;
    [Button("差集"),Click("AA")]
    public string _1;
    public void AA()
    {
        
        var mono = GetComponent<CqCurveMono>().curve.points;
        var list=Vector2Util.LineSub(mono[0].point.ToVector2(), mono[1].point.ToVector2(), mono[2].point.ToVector2(), mono[3].point.ToVector2());
        outMono.curve.points.Clear();
        if (list!=null)
        {
            for (var i=0;i<list.Count;i++)
            {
                outMono.curve.points.Add(new CqCurvePoint() { point = list[i].a.ToVector3() });
                outMono.curve.points.Add(new CqCurvePoint() { point = list[i].b.ToVector3() });
            }
        }
        else
        {
            Debug.Log("null");
        }
    }

    [Button("合并"), Click("BB")]
    public string _2;
    public void BB()
    {
        var mono = GetComponent<CqCurveMono>().curve.points;
        Vector2 a, b;
        var bl = Vector2Util.LineUnion(mono[0].point.ToVector2(), mono[1].point.ToVector2(), mono[2].point.ToVector2(), mono[3].point.ToVector2(),
            out a,out b);
        outMono.curve.points.Clear();
        if (bl)
        {
            outMono.curve.points.Add(new CqCurvePoint() { point = a.ToVector3() });
            outMono.curve.points.Add(new CqCurvePoint() { point = b.ToVector3() });
        }
        else
        {
            Debug.Log("null");
        }
    }
}