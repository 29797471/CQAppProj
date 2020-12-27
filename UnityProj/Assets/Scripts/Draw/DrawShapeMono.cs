using UnityCore;
using UnityEngine;

/// <summary>
/// 图元绘制基类
/// </summary>
public abstract class DrawShapeMono : MonoBehaviour
{
    /// <summary>
    /// 颜色
    /// </summary>
    [Color("颜色"),OnValueChanged("UpdateColor")]
    public Color color = Color.white;
    int _shapeColor =Shader.PropertyToID("_shapeColor");

    public void UpdateColor()
    {
        Mpb.SetColor(_shapeColor, color);
    }

    private void Awake()
    {
        UpdateColor();
        Init();
    }

    protected abstract void Init();


    public Material Mat { get; protected set; }
    MaterialPropertyBlock mMpb;

    public abstract Matrix4x4 GetShapeMatrix();

    Matrix4x4 worldMatrix;
    public MaterialPropertyBlock Mpb
    {
        get
        { 
            if (mMpb == null) mMpb = new MaterialPropertyBlock();
            return mMpb;
        }
    }
    
    Matrix4x4 lastMat;
    public void UpdateMax()
    {
        worldMatrix = transform.localToWorldMatrix * GetShapeMatrix();
    }
    private void Update()
    {
        if(lastMat!=transform.localToWorldMatrix)
        {
            lastMat = transform.localToWorldMatrix;
            UpdateMax();
        }
        Graphics.DrawMesh(DrawShapeSetting.Inst.QuadxzMesh, worldMatrix, Mat, 0, null, 0, Mpb);
    }
}
