using UnityCore;
using UnityEngine;

/// <summary>
/// 绘制多边形
/// </summary>
public class DrawPolygonMono : DrawShapeMono
{
    [Vector("中心点"),OnValueChanged("UpdateMax")]
    public Vector3 pos;

    [TextBox("半径"), OnValueChanged("UpdateMax")]
    public float R=10;

    [TextBox("边"), OnValueChanged("UpdateSide")]
    public int side = 3;

    int _side = Shader.PropertyToID("_side");

    protected override void Init()
    {
        Mat = DrawShapeSetting.Inst.matpolygon;
        UpdateSide();
    }
    public void UpdateSide()
    {
        Mpb.SetInt(_side, side);
    }
    public override Matrix4x4 GetShapeMatrix()
    {
        return Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(R, 1, R));
    }

}
