using CqCore;
using System;
using System.Collections;
using UnityCore;
using UnityEngine;
using UnityEngine.UI;

public enum MoveState
{
    [EnumLabel("直线")]
    StraightLine,
    [EnumLabel("曲线")]
    CurveLine,
}

/// <summary>
/// 在固定路径上移动的控制组件
/// </summary>
public class CarMoveControl : MonoBehaviourExtended
{
    [ComBox("移动状态", ComBoxStyle.RadioBox), IsEnabled(false)]
    public MoveState state;

    [TextBox("轴距"), ToolTip("主要用于在道路行驶上计算前轮方向")]
    public float Wheelbase=2.5f;
    CarPath path;
    public CqCurve Curve
    {
        get
        {
            if (path == null) path = GetComponent<CarPath>();
            return path.curve;
        }
    }

    [Button("暂停"), Click("Pause")]
    public string _1;

    public void Pause()
    {
        if (cc != null) cc.Stop();
    }
    CancelHandle handle;

    public Text tempText;
    public int id;
    private void Awake()
    {
        handle = new CancelHandle();
        var renders=GetComponentsInChildren<Renderer>(true);
        foreach(var it in renders)
        {
            it.enabled = false;
            handle.CancelAct += () => it.enabled = true;
        }
        handle.CancelAct += () =>
        {
            var obj = GameObjectUtil.Clone(tempText.gameObject);
            obj.SetActive(true);
            tempText = obj.GetComponent<Text>();
            tempText.text = id.ToString();
            StartCoroutine(LookCamera(obj),DestroyHandle);
            DestroyHandle.CancelAct += () => Destroy(obj);
        };
    }

    IEnumerator LookCamera(GameObject obj)
    {
        while(true)
        {
            if (tempText.gameObject.activeInHierarchy)
            {
                tempText.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            }
            yield return null;
        }
    }

    [Button("继续"), Click("Continue")]
    public string _2;

    public void Continue()
    {
        if (cc != null) cc.Start(null);
    }
    CqCoroutine cc;

    private void OnEnable()
    {
        cc =StartCoroutine(LoopMove(Curve),DisabledHandle);
    }
    
    [TextBox("移动速度")]
    public float speed = 0f;

    [TextBox("真实移动速度"),IsEnabled(false)]
    public float realSpeed;

    [TextBox("前轮方向"), IsEnabled(false)]
    public float realDir;


    public Vector3 Pos
    {
        set
        {
            handle.CancelAll();
            realSpeed = Vector3.Distance(value, transform.position)*Mathf.Sign(speed) / GlobalCoroutine.deltaTime;
            transform.position = value;
        }
        get
        {
            return transform.position;
        }
    }
    Vector3 mDir;
    public Vector3 Dir
    {
        set
        {

            mDir = value;
            
            transform.rotation = Quaternion.LookRotation(mDir);
        }
        get
        {
            return mDir;
        }
    }
    [CheckBox("自动删除行驶过的路")]
    public bool autoRemovePathad=true;

    [CheckBox("自动添加路")]
    public bool autoAddePathad = true;

    IEnumerator LoopMove(CqCurve curve)
    {
        var index = 0;

        Pos = curve.points[index].point;

        float t=0;
        while (true)
        {
            if (autoAddePathad && curve.points.Count < index + 3)
            {
                path.InputTurnToNextWay();
            }

            //只保留两条历史道路
            while (autoRemovePathad && !curve.close && index>2)
            {
                curve.points.RemoveAt(0);
                index--;
            }
            path.ReDraw();

            var current = curve.points[index];
            path.movePathId = current.data;
            var next = curve.points.GetItemByRound(index + 1);

            if (!current.IsLine(next))
            {
                state = MoveState.CurveLine;
                //var startDis = Vector3.Distance(current.point, Pos);
                //var endDis = Vector3.Distance(next.point, Pos);
                //if (startDis>endDis)
                //{
                //    t = 1-Vector3.Distance(next.point, Pos) * current.GetDeltaT_Dis_K(next, 1);
                //}
                //else
                //{
                //    t = Vector3.Distance(current.point, Pos) * current.GetDeltaT_Dis_K(next, 0);
                //}

                while (t<= 1f && t>=0f)
                {
                    //通过计算1帧位移的距离,迭代计算当前帧对应的差值系数
                    var deltaT = current.GetDeltaT_Dis_K(next, t);
                    if (float.IsInfinity(deltaT))
                    {
                        var k= current.GetDeltaT_Dis_K(next, t);
                    }
                    t += deltaT * GlobalCoroutine.deltaTime * speed;
                    if (float.IsInfinity(t))
                    {

                    }
                    Pos = current.LerpUnclamped(next, t);
                    Dir = current.GetTangent(next, t);

                    //计算前轮
                    var frontT = Mathf.Clamp(t + deltaT * Wheelbase / 2, 0,1);
                    var backT = Mathf.Clamp(t - deltaT * Wheelbase / 2, 0, 1);
                    var frontDir = current.GetTangent(next, frontT);
                    var backDir = current.GetTangent(next, backT);

                    realDir = Vector2Util.GetRoundAngleX(frontDir.ToVector2(), backDir.ToVector2()) * Mathf.Rad2Deg;

                    yield return null;
                }
            }
            else
            {
                state = MoveState.StraightLine;
                var a = current.point;
                var b = next.point;
                var dis = Vector3.Distance(a, b);
                Dir = b - a;
                realDir = 0;
                //t = (Pos-a).op_Division(b-a);
                while (t <= 1f && t >= 0f)
                {
                    t += GlobalCoroutine.deltaTime * speed/ dis;
                    Pos = Vector3.LerpUnclamped(a, b, t);
                    yield return null;
                }
            }
            if (t > 1f)
            {
                if (!curve.close && index == curve.points.Count - 1)
                {
                    speed = 0;
                    realSpeed = 0;
                    t = 1;
                }
                else
                {
                    index = (index + 1) % curve.points.Count;
                    t = 0;
                }
                
            }
            else if(t<0f)
            {
                if (!curve.close && index == 0)
                {
                    speed = 0;
                    realSpeed = 0;
                    t = 0;
                }
                else
                {
                    index=(index-1+ curve.points.Count) % curve.points.Count;
                    t = 1;
                }
            }
            else
            {
                throw new Exception("未知错误" + t);
            }
            
            while(speed==0) yield return null;
        }
    }
    

}
