using System.Collections.Generic;
using UnityCore;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    [Button("取曲线生成面"), Click("DrawPolygon1")]
    public string _1;

    public bool makeRealMesh;
    public void DrawPolygon1()
    {
        var targetFilter = GetComponent<MeshFilter>();
        var curve = GetComponent<CqCurveMono>();
        var points = curve.curve.ExportPolygon();
        if(makeRealMesh)
        {
            var x = this.gameObject.AddComponent<CqCurveMono>();
            x.curve = new CqCurve();
            x.curve.close = true;
            x.curve.points = points.ConvertAll(y => new CqCurvePoint() { point = y });
        }
        
        targetFilter.sharedMesh = MeshUtil.Create3DByPipeline(points);
        
        Debug.Log(Torsion.Serialize(targetFilter.sharedMesh.triangles));
        Debug.Log(Torsion.Serialize(targetFilter.sharedMesh.uv));
        Debug.Log(Torsion.Serialize(targetFilter.sharedMesh.normals));
    }
    
}