using UnityCore;
using UnityEngine;

public class CqCurveMono : MonoBehaviourExtended
{
    public CqCurve ToWorldCurve
    {
        get
        {
            var temp = Torsion.Clone(curve);
            var mat = transform.localToWorldMatrix;
            foreach (var it in temp.points)
            {
                it.point = mat.MultiplyPoint(it.point);
            }
            return temp;
        }
    }
    public CqCurve curve;
    public Vector3 this[float k]
    {
        get
        {
            return transform.localToWorldMatrix.MultiplyPoint(curve[k]);
        }
    }
#if UNITY_EDITOR
    [HideInInspector]
    public bool lockDrawInScene;


    //private void OnDrawGizmos()
    //{
    //    return;
    //    var select = UnityEditor.Selection.Contains(gameObject);

    //    var selectParent = (UnityEditor.Selection.activeTransform) != null && transform.IsChildOf(UnityEditor.Selection.activeTransform);
        
    //    if (lockDrawInScene || select || selectParent) 
    //    {
    //        if (curve==null || curve.points == null || curve.points.Count < 2) return;
    //        var config = CqCurveEditorConfig.Inst;
    //        var worldMat = transform.localToWorldMatrix;
    //        System.Func<Vector3, Vector3> ToWorld = (a) => worldMat.MultiplyPoint(a);

    //        var worldList = new List<Vector3>();
    //        for (int i = 0; i < curve.points.Count; i++)
    //        {
    //            var p = ToWorld(curve.points[i].point);
    //            worldList.Add(p);
    //        }
    //        System.Action<int, int> DrawLine = (a, b) =>
    //        {
    //            var p1 = curve.points[a];
    //            var p2 = curve.points[b];
    //            UnityEditor.Handles.DrawBezier(
    //                worldList[a],
    //                worldList[b],
    //                ToWorld(p1.outTangent),
    //                ToWorld(p2.inTangent),
    //                select?config.lineColor:config.lockLineColor, null, (select ? 1:0.25f)*config.lineWidth);
                
    //        };
    //        if (curve.close)
    //        {
    //            DrawLine(curve.points.Count - 1, 0);
    //        }

    //        for (int i = 0; i < curve.points.Count - 1; i++)
    //        {
    //            DrawLine(i, i + 1);
    //        }
    //    }
    //}
    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(transform.position, sphereSize);
    }
#endif
}
