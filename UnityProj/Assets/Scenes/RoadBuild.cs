using CqCore;
using UnityCore;
using UnityEngine;
using UnityEngine.Events;

public class RoadBuild : MonoBehaviourExtended
{
    Camera cam;
    RoadArgs arp;
    PathE selectPathE;

    bool moveSecondPoint;

    string[] oprs = new string[] { "添加", "拆除", "寻路" };

    public bool drawGui;
    /// <summary>
    /// 添加/拆除/寻找道路
    /// </summary>
    public int add_remove_find;

    bool allowDrop;
    string[] roadDrawStyleNames;
    string[] roadAlignNames;
    public UnityEvent Save;

    public int RoadStyleId
    {
        set
        {
            PathMapRule.instance.SetStyle(value);
        }
        get
        {

            return PathMapRule.instance.CurrentStyle.id;
        }
    }
    void Awake()
    {
        //InputMgr.instance.Init();

        cam = GetComponent<Camera>();
        roadDrawStyleNames = EnumUtil.GetEnumNames<RoadDrawStyle>();
        roadAlignNames = EnumUtil.GetEnumNames<RoadAlign>();
    }
    private void Start()
    {
        arp = FindObjectOfType<RoadArgs>();
    }

    private void OnEnable()
    {
        InputMgr.instance.MouseLeft.Click_CallBack(MouseLeft_OnClick, DisabledHandle);

        InputMgr.instance.MouseLeft.Move_CallBack(MouseLeft_OnMove, DisabledHandle);

        InputMgr.instance.MouseRight.Click_CallBack(MouseRight_OnClick, DisabledHandle);
        DisabledHandle.CancelAct+= PathMapRule.instance.Clear;
    }
    
    private void MouseLeft_OnClick()
    {
        switch (add_remove_find)
        {
            case 0:
                {
                    if (moveSecondPoint)
                    {
                        var bl = PathMapRule.instance.AddRoad();
                        if (bl)
                        {
                            PathMapRule.instance.Clear();
                            PathMapRule.instance.ReCalcHelpObjs(false);
                            Save.Invoke();
                        }
                    }
                    else
                    {
                        moveSecondPoint = true;
                        PathMapRule.instance.FirstDown();
                        PathMapRule.instance.Clear();
                        PathMapRule.instance.ReCalcHelpObjs(true);
                    }
                    break;
                }
            case 1:
                {
                    if (selectPathE != null)
                    {
                        PathMapRule.instance.RemoveRoad(selectPathE);
                        selectPathE = null;
                        Save.Invoke();
                    }
                }
                break;
            case 2://定起始寻路车道
                {
                    
                }
                break;
        }
    }

    private void MouseLeft_OnMove(Vector2 obj)
    {
        var temp = Camera.main.MouseRayCrossYPlane();
        if (temp == null) return;
        var mousePosIn = ((Vector3)temp).ToVector2();
        switch (add_remove_find)
        {
            case 0:
                {
                    if (moveSecondPoint)
                    {
                        mousePosIn = PathMapRule.instance.SetTo(mousePosIn);
                        //Debug.Log(PathMapRule.instance.ErrorState);
                    }
                    else
                    {
                        mousePosIn = PathMapRule.instance.SetFrom(mousePosIn);
                    }
                    break;
                }
            case 1:
            case 2:
                {
                    var t = PathMapRule.instance.SelectLink(mousePosIn);
                    if (t != selectPathE)
                    {
                        selectPathE = t;
                    }
                    break;
                }
        }
    }

    private void MouseRight_OnClick()
    {
        var temp = Camera.main.MouseRayCrossYPlane();
        if (temp == null) return;
        var mousePosIn = ((Vector3)temp).ToVector2();
        switch (add_remove_find)
        {
            case 0:
                {
                    moveSecondPoint = false;
                    PathMapRule.instance.SetFrom(mousePosIn);
                    break;
                }
            case 1:
                {
                    var t = PathMapRule.instance.SelectLink(mousePosIn);
                    if (t != selectPathE)
                    {
                        selectPathE = t;
                    }
                    break;
                }
            case 2://定结束寻路车道
                {
                }
                break;
        }
    }

    void OnGUI()
    {
        if (!drawGui) return;
        GUILayout.BeginHorizontal();

        for (int i = 0; i < oprs.Length; i++)
        {
            var bl = (add_remove_find == i);
            var _add_remove_find = GUILayout.Toggle(bl, oprs[i]);
            if (_add_remove_find != bl)
            {
                add_remove_find = i;
                switch (add_remove_find)
                {
                    case 0:
                        {
                            break;
                        }
                    default:
                        break;
                }
                PathMapRule.instance.Clear();
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < arp.styles.Length; i++)
        {
            var bl = (PathMapRule.instance.CurrentStyle == arp.styles[i]);
            var result = GUILayout.Toggle(bl, arp.styles[i].name);
            if (result != bl)
            {
                PathMapRule.instance.SetStyle(arp.styles[i].id);
            }
        }

        GUILayout.EndHorizontal();

        arp.roadArgs_Draw.limit45 = GUILayout.Toggle(arp.roadArgs_Draw.limit45, "建造45度限制");

        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("对齐");

            var state = arp.align;
            for (int i = 0; i < roadAlignNames.Length; i++)
            {
                var e = (RoadAlign)(1 << i);
                var s = MathUtil.StateCheck(state, e);
                var ss = GUILayout.Toggle(s, roadAlignNames[i]);
                if (ss != s)
                {
                    if (ss)
                    {
                        arp.align = MathUtil.StateAdd(state, e);
                    }
                    else
                    {
                        arp.align = MathUtil.StateDel(state, e);
                    }
                }
            }

            GUILayout.EndHorizontal();
        }
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("绘制");

            var state = arp.drawStyle;
            for (int i = 0; i < roadDrawStyleNames.Length; i++)
            {
                var e = (RoadDrawStyle)(1 << i);
                var s = MathUtil.StateCheck(state, e);
                var ss = GUILayout.Toggle(s, roadDrawStyleNames[i]);
                if (ss != s)
                {
                    if (ss)
                    {
                        arp.drawStyle = MathUtil.StateAdd(state, e);
                    }
                    else
                    {
                        arp.drawStyle = MathUtil.StateDel(state, e);
                    }
                    arp.OnDrawStyleChanged();
                }
            }

            GUILayout.EndHorizontal();
        }
        


        //if (GUILayout.Button("清除生成和数据"))
        //{
        //    cp.ClearAndData();
        //}
    }
}
