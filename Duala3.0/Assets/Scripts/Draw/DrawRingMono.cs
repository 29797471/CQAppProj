using UnityCore;
using UnityEngine;

/// <summary>
/// 绘制圆环
/// </summary>
public class DrawRingMono : DrawShapeMono
{
    [Vector("中心点"),OnValueChanged("UpdateMax")]
    public Vector3 pos;

    [TextBox("外径"), OnValueChanged("UpdateMax")]
    public float R = 50;

    [TextBox("内径"), OnValueChanged("UpdateR")]
    public float r = 10;

    int _r = Shader.PropertyToID("_r");

    public void UpdateR()
    {
        Mpb.SetFloat(_r, r/R);
    }

    protected override void Init()
    {
        Mat = DrawShapeSetting.Inst.matRing;
        UpdateR();
    }
    public override Matrix4x4 GetShapeMatrix()
    {
        return Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(R, 1, R));
    }

}
