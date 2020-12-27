using UnityCore;
using UnityEngine;

/// <summary>
/// 绘制多边形(圆)
/// </summary>
[ExecuteInEditMode]
public class DrawCircleMono : DrawShapeMono
{
    [Vector("中心点"),OnValueChanged("UpdateMax")]
    public Vector3 pos;

    [TextBox("半径"), OnValueChanged("UpdateMax")]
    public float R=10;

    protected override void Init()
    {
        Mat = DrawShapeSetting.Inst.matCircle;
    }
    public override Matrix4x4 GetShapeMatrix()
    {
        return Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(R, 1, R));
    }

}
