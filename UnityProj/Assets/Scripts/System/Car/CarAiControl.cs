using CqCore;
using System.Collections;
using UnityCore;
using UnityEngine;

public enum PlayerCarState
{
    /// <summary>
    /// 正常行驶
    /// </summary>
    [EnumLabel("正常行驶")]
    Normal,

    [EnumLabel("立即停止")]
    StopImmediately,

    [EnumLabel("停止")]
    StopYield,

    /// <summary>
    /// 警戒行驶<para/>
    /// 运动碰撞内有其它车辆
    /// </summary>
    [EnumLabel("警戒行驶")]
    Warn,

}

public class CarAiControl : MonoBehaviourExtended
{
    [ComBox("状态", ComBoxStyle.RadioBox),IsEnabled(false)]
    public PlayerCarState state;

    [TextBox("过弯移动速度")]
    public float turnSpeed=5f;

    [TextBox("直线行驶速度")]
    public float normalSpeed=10f;

    [TextBox("警戒行驶速度")]
    public float warnSpeed = 2f;

    [TextBox("跟车距(米)"), ToolTip("和前车小于这个距离触发停车")]
    public float followDis = 3f;

    [TextBox("油门加速度(米/秒^2)")]
    public float accAdd = 5f;

    [TextBox("刹车减速度(米/秒^2)")]
    public float accSub = -10f;

    /// <summary>
    /// 制动距离 = 平均速度 * 制动时间
    /// </summary>
    public float StopDis
    {
        get
        {
            return Mathf.Abs((mc.speed / 2) * (mc.speed / accSub));
        }
    }
    /// <summary>
    /// 反应时间
    /// </summary>
    public float checkTime;

    /// <summary>
    /// 让行单位
    /// </summary>
    public CarCollider yieldTarget;

    public CarMoveControl mc;
    public CarPath path;
    public CarCollider col;
    private void Awake()
    {
        mc=GetComponent<CarMoveControl>();
        path = GetComponent<CarPath>();
        col = GetComponent<CarCollider>();
    }
    private void OnEnable()
    {
        StartCoroutine(LoopAi(), DisabledHandle);
    }
    
    IEnumerator LoopAi()
    {
        state = PlayerCarState.Normal;
        while(true)
        {
            var TargetSpeed = 0f;

            if (yieldTarget != null)
            {
                if (state == PlayerCarState.Normal)
                {
                    state = PlayerCarState.Warn;
                    mc.tempText.color = Color.yellow;
                }

                var dis = Vector3.Distance(transform.position, yieldTarget.transform.position);
                var _follow = col.carLength / 2 + yieldTarget.carLength / 2 + followDis;

                if(dis<_follow)
                {
                    if (state != PlayerCarState.StopImmediately)
                    {
                        state = PlayerCarState.StopImmediately;
                        mc.tempText.color = Color.black;
                    }
                }
                else if (dis< _follow + StopDis)
                {
                    if(state != PlayerCarState.StopYield)
                    {
                        state=PlayerCarState.StopYield;
                        mc.tempText.color = Color.red;
                    }
                }
                else
                {
                    if (state != PlayerCarState.Warn)
                    {
                        state = PlayerCarState.Warn;
                        mc.tempText.color = Color.yellow;
                    }
                }
                
            }
            else
            {
                state = PlayerCarState.Normal;
                mc.tempText.color = Color.white;
            }

            switch (state)
            {
                case PlayerCarState.StopImmediately:
                    TargetSpeed = 0f;
                    mc.speed = 0;
                    break;
                case PlayerCarState.StopYield:
                    TargetSpeed = 0f;
                    break;
                case PlayerCarState.Normal:
                    {
                        switch (mc.state)
                        {
                            case MoveState.StraightLine:
                                {
                                    TargetSpeed = normalSpeed;
                                    break;
                                }
                            case MoveState.CurveLine:
                                {
                                    TargetSpeed = turnSpeed;
                                    break;
                                }
                        }
                    }
                    break;
                case PlayerCarState.Warn:
                    TargetSpeed = warnSpeed;
                    break;
                default:
                    break;
            }
            
            
            if (mc.speed > TargetSpeed)
            {
                mc.speed += accSub * GlobalCoroutine.deltaTime;
            }
            else
            {
                mc.speed += accAdd * GlobalCoroutine.deltaTime;
            }
            yield return null;
        }
    }

}
