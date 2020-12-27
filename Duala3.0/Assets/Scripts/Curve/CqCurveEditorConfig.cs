using UnityEngine;

/// <summary>
/// 曲线编辑器配置数据
/// </summary>
public class CqCurveEditorConfig
{
    public float lineWidth = 1.5f;

    public Color lineColor = Color.green;

    public Color lockLineColor = Color.white;

    //对称切线颜色
    public Color symmetricTangentColor = Color.green;

    //进
    public Color inTangentColor = Color.black;
    //出
    public Color outTangentColor = Color.white;


    public Color noneColor = Color.yellow;
    public Color selectColor = Color.blue;
    public Color nearColor = Color.white;

    /// <summary>
    /// 基于基准球的缩放大小
    /// </summary>
    public float sphereSizeScale = 1f;

    /// <summary>
    /// 顶点坐标栅格化单位
    /// </summary>
    public float closeGrid = 0.1f;


    /// <summary>
    /// 贝塞尔平滑系数
    /// </summary>
    public float smoothK = 0.25f;

    static CqCurveEditorConfig mData;
    public static CqCurveEditorConfig Inst
    {
        get
        {
            if (mData == null)
            {
                if (PlayerPrefs.HasKey(key))
                {
                    mData = Torsion.Deserialize<CqCurveEditorConfig>(PlayerPrefs.GetString(key));
                }
                else
                {
                    mData = new CqCurveEditorConfig();
                    PlayerPrefs.SetString(key, Torsion.Serialize(mData));
                }
            }
            return mData;
        }
        set
        {
            mData = value;
            PlayerPrefs.SetString(key, Torsion.Serialize(mData));
        }
    }
    const string key = "CurveConfig";
}
