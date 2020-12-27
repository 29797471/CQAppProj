using CqCore;
using UnityCore;
using UnityEngine;

public class CqCurveObj : MonoBehaviour
{
    public CqCurveMono mono;
    [Slider("曲线位置系数", 0, 1), OnValueChanged("K")]
    public float mK;
    public float K
    {
        set
        {
            mK = value;
            var pos = mono[mK];
            speed=Vector3.Distance(transform.position, pos)/ GlobalCoroutine.deltaTime;
            if (pos != transform.position) transform.rotation = Quaternion.LookRotation(transform.position - pos);
            transform.position = pos;
        }
        get
        {
            return mK;
        }
    }
    [TextBox("移动距离"), IsEnabled(false), OnValueChanged("Length")]
    public float mLength;
    public float Length
    {
        set
        {
            if (mLength != value)
            {
                mLength = value;
                LengthK=(mLength / totalLength) % 1;
            }
        }
        get
        {
            return mLength;
        }
    }
    float totalLength;

    float mLengthK;
    public float LengthK
    {
        set
        {
            mLengthK = value;
            K = curve.Evaluate(mLengthK);
        }
        get
        {
            return mLengthK;
        }
    }
    [TextBox("当前速度"),IsEnabled(false)]
    public float speed;

    [Button("更新匀速曲线"),Click("AA")]
    public string _1;

    [Curve("匀速曲线"),Height(100)]
    public AnimationCurve curve;

    
    [TextBox("每段曲线采样段数")]
    public int calcCount = 20;

    public void AA()
    {
        var realCount = calcCount * (mono.curve.points.Count + (mono.curve.close ? 0 : -1));
        curve = AnimationCurveUtil.GetLengthCurve(t => mono.curve[t], realCount);
        totalLength = mono.curve.Length;
    }
}
