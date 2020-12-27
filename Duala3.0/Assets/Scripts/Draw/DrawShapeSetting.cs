using System;
using UnityCore;
using UnityEngine;

/// <summary>
/// 图元绘制类
/// </summary>
[Serializable]
[CreateAssetMenu]
public class DrawShapeSetting : ScriptableObject
{
    static DrawShapeSetting mInst;
    public static DrawShapeSetting Inst
    {
        get
        {
            if (mInst == null)
            {
                mInst = Resources.Load<DrawShapeSetting>("DrawShapeSetting");
            }
            return mInst;
        }
    }

    /// <summary>
    /// xz正方形
    /// </summary>
    public Mesh QuadxzMesh
    {
        get
        {
            if(mQuadxzMesh==null)
            {
                mQuadxzMesh= MeshUtil.MakeXZQuad();
            }
            return mQuadxzMesh;
        }
    }
    Mesh mQuadxzMesh;

    [CqLabel("虚线")]
    public Material matDottedLine;

    [CqLabel("实线")]
    public Material matLine;

    [CqLabel("圆")]
    public Material matCircle;

    [CqLabel("圆环")]
    public Material matRing;

    [CqLabel("多边形")]
    public Material matpolygon;

}
