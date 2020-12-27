using UnityCore;
using UnityEngine;

public class MakeLineToPolygon : MonoBehaviour
{
    [Button("生成曲边多边形"), Click("DrawPolygon2")]
    public string _2;

    [TextBox("a宽度")]
    public float aWidth = 10f;

    [TextBox("c宽度")]
    public float cWidth = 10f;
    public void DrawPolygon2()
    {
        transform.RemoveAllChildren();
        var mono=GetComponent<CqCurveMono>();
        var points = mono.curve.points;

        var rayA = new Ray2D((points[0].point).ToVector2(), (points[1].point - points[0].point).ToVector2());
        var rayB = new Ray2D((points[2].point).ToVector2(), (points[2].point - points[1].point).ToVector2());
        var curve= RoadMaker.LineSmoothConnection(rayA,rayB,aWidth, cWidth);
        foreach (var it in curve.points)
        {
            it.point = ((Vector2)it.point).ToVector3();
            it.inVec = ((Vector2)it.inVec).ToVector3();
            it.outVec = ((Vector2)it.outVec).ToVector3();
        }
        var Xmono = gameObject.AddComponent<CqCurveMono>();
        Xmono.curve = curve;
    }

    
}
