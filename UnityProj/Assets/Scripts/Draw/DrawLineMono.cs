using System;
using UnityCore;
using UnityEngine;

public class DrawLineMono : DrawShapeMono
{
    [Vector("起点"),OnValueChanged("UpdateMax")]
    public Vector3 start=Vector3.right;

    [Vector("终点"), OnValueChanged("UpdateMax")]
    public Vector3 end;

    [TextBox("线宽"), OnValueChanged("UpdateMax")]
    public float lineWidth=1f;

    protected override void Init()
    {
        Mat = DrawShapeSetting.Inst.matLine;
    }

    public override Matrix4x4 GetShapeMatrix()
    {
        var dir = end - start;
        return Matrix4x4.TRS((start + end) / 2, Quaternion.LookRotation(dir), new Vector3(lineWidth, 1, dir.magnitude));
    }
    

}
