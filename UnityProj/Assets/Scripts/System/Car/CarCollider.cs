using CqCore;
using System.Collections;
using UnityCore;
using UnityEngine;

public class CarCollider : MonoBehaviourExtended
{
    [CheckBox("绘制包围盒")]
    public bool drawBounds;

    [CheckBox("绘制运动碰撞盒")]
    public bool drawRunBounds;

    [CheckBox("绘制车距控制碰撞盒")]
    public bool drawStopBounds;

    [TextBox("曲线行驶检测间隔")]
    public float CurveCheckTime = 0.1f;
    [TextBox("直线行驶检测间隔")]
    public float LineCheckTime = 0.3f;
    /// <summary>
    /// 模型包围盒
    /// </summary>
    Bounds bounds;

    public float carLength { get=>bounds.size.z; }

    Bounds stopBounds;
    Bounds runBounds;
    CarAiControl ca;

    private void Awake()
    {
        bounds = BoundsUtil.GetBounds(gameObject);
        
        ca = GetComponent<CarAiControl>();
    }

    private void OnEnable()
    {
        StartCoroutine(Loop());
    }
    private void Update()
    {
        runBounds = bounds;
        var doTime = (ca.mc.state == MoveState.CurveLine ? CurveCheckTime : LineCheckTime);
        runBounds.max += Vector3.forward * (ca.StopDis + ca.followDis+(Mathf.Abs(ca.mc.speed)* doTime));

        stopBounds = bounds;
        stopBounds.max+=Vector3.forward * ca.followDis;

        if (drawRunBounds)
        {
            DebugUtil.DrawBounds(runBounds, transform.localToWorldMatrix, Color.yellow);
        }
        if(drawStopBounds)
        {
            DebugUtil.DrawBounds(stopBounds, transform.localToWorldMatrix, Color.white);
        }
        if (drawBounds)
        {
            DebugUtil.DrawBounds(bounds, transform.localToWorldMatrix, Color.red);
        }
    }

    IEnumerator Loop()
    {
        while (true)
        {
            if (ca.yieldTarget != null)
            {
                if (!IntersectsRun(this, ca.yieldTarget,out Vector3 p))
                {
                    ca.yieldTarget = null;
                }
            }
            else
            {
                if (ca.path.movePathId != 0)
                {
                    ca.yieldTarget = CheckRunCollider();
                }
            }
            yield return GlobalCoroutine.Sleep( ca.mc.state == MoveState.CurveLine ? CurveCheckTime : LineCheckTime);
        }
    }
    CarCollider CheckRunCollider()
    {
        var cars = ca.path.GetCarsInSameRoad();
        cars.Sort(x => Vector3.Distance(x.transform.position,transform.position));
        foreach (var it in cars)
        {
            if (it == this)
            {
                continue;
            }
            
            if (IntersectsRun(this,it,out Vector3 p))
            {
                if(it.ca.yieldTarget!=null)//碰撞相交的车也有让行目标
                {
                    //确定自己不在让行队列.
                    var temp = it;
                    while(temp.ca.yieldTarget!=null)
                    {
                        if (temp.ca.yieldTarget == this) break;
                        temp = temp.ca.yieldTarget;
                    }

                    if (temp.ca.yieldTarget != this)
                    {
                        //插入队列
                        return it;
                    }
                    //自己是队列首
                    continue;
                }
                //两车探测范围相交,需要确定谁让谁.
                
                var aRay = baseRay.Multiply(transform.localToWorldMatrix);
                var bRay = baseRay.Multiply(it.transform.localToWorldMatrix);
                
                //行径方向上远离碰撞点的单位先行
                if (aRay.ProjectDistance(p) > bRay.ProjectDistance(p))
                {
                    return it;
                }
                else
                {
                    it.ca.yieldTarget = this;
                }
            }
        }

        return null;
    }

    static Ray baseRay = new Ray(Vector3.zero, Vector3.forward);
    /// <summary>
    /// 两车作碰撞检测,参数返回相交范围中心点
    /// </summary>
    public static bool IntersectsRun(CarCollider a, CarCollider b,out Vector3 pos)
    {
        a.runBounds.ToWorldVertexs(a.transform.localToWorldMatrix, ref temp1);
        b.runBounds.ToWorldVertexs(b.transform.localToWorldMatrix, ref temp2);
        for (int i = 0; i < 4; i++)
        {
            temp1_2[i] = temp1[i].ToVector2();
            temp2_2[i] = temp2[i].ToVector2();
        }
        var poly=Vector2Util.PolygonIntersection(temp1_2, temp2_2);
        pos = Vector3.zero;
        if (poly.Count == 0) return false;
        pos = ((Vector2)Vector2Util.Average(poly)).ToVector3();
        return true;
    }

    public static float CalcArea(CarCollider a, CarCollider b,float calcPri)
    {
        var checkBounds = a.runBounds;
        checkBounds.max += Vector3.forward * calcPri;
        checkBounds.ToWorldVertexs(a.transform.localToWorldMatrix, ref temp1);
        b.runBounds.ToWorldVertexs(b.transform.localToWorldMatrix, ref temp2);
        for (int i = 0; i < 4; i++)
        {
            temp1_2[i] = temp1[i].ToVector2();
            temp2_2[i] = temp2[i].ToVector2();
        }
        return Vector2Util.IntersectionArea(temp1_2, temp2_2);
    }
    static Vector3[] temp1 = new Vector3[4];
    static Vector3[] temp2 = new Vector3[4];

    static Vector2[] temp1_2 = new Vector2[4];
    static Vector2[] temp2_2 = new Vector2[4];
}
