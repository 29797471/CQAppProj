using CqCore;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// 道路配置参数
/// </summary>
[InpectorDrawStyle(150)]
public class RoadArgs : MonoBehaviourExtended
{
    /// <summary>
    /// 对齐方式
    /// </summary>
    [ComBox("对齐", ComBoxStyle.CheckBox)]
    public RoadAlign align;

    /// <summary>
    /// 绘制方式
    /// </summary>
    [ComBox("绘制", ComBoxStyle.CheckBox), OnValueChanged("OnDrawStyleChanged")]
    public RoadDrawStyle drawStyle;

    /// <summary>
    /// 道路样式列表
    /// </summary>
    public RoadStyle[] styles;

    [Header("道路生成参数")]
    public RoadArgs_Make roadArgs_Make;

    [Header("辅助绘制参数")]
    public RoadArgs_Draw roadArgs_Draw;

    [Header("其它装饰物参数")]
    public RoadArgs_Others roadArgs_Others;
    public float partWidth = 1f;

    [System.Serializable]
    public class MakeElement
    {
        public string name;
        [ComBox("样式",ComBoxStyle.RadioBox)]
        public RoadType type;
        public Material mat;
        public float y;
    }
    public MakeElement[] makeElements;
    Dictionary<RoadType, MakeElement> dic;

    public MakeElement GetMakeElement(RoadType type)
    {
        if (dic == null)
        {
            dic = new Dictionary<RoadType, MakeElement>();
            foreach(var it in makeElements)
            {
                dic[it.type] = it;
            }
        }
        return dic[type];
    }
    //测试绘制实例
    PathMapDraw drawInst;

    void Awake()
    {
        PathMapRule.instance.SetMake(new RoadMaker(), transform);
        PathMapRule.instance.Init(this, styles, x => 1 / (1 + x * x / 10000));

        drawInst = new PathMapDraw();
        drawInst.Init(true);
    }

    public void OnDrawStyleChanged()
    {
        if (drawInst == null) return;
        drawInst.lineWidth = roadArgs_Draw.drawLineWidth;
        drawInst.Clear();
        var pathMap = PathMapRule.instance.PathMapInst;
        //绘制转弯顶点
        if (MathUtil.StateCheck(drawStyle, RoadDrawStyle.Corner))
        {
            drawInst.color = Color.blue;
            for (int i = 0; i < pathMap.list.Count; i++)
            {
                var p = pathMap.list[i];
                foreach (var it in p.corners)
                {
                    drawInst.DrawCirCle(roadArgs_Draw.drawCorPointR, it.inside);
                    drawInst.DrawCirCle(roadArgs_Draw.drawCorPointR, it.outside);
                    drawInst.DrawCirCle(roadArgs_Draw.drawCorPointR, it.subface_outside);
                }
            }
        }

        //绘制回退点
        if (MathUtil.StateCheck(drawStyle, RoadDrawStyle.CenterStart))
        {
            drawInst.color = Color.green;
            for (int i = 0; i < pathMap.links.Count; i++)
            {
                var e = pathMap.links[i];
                drawInst.DrawCirCle(roadArgs_Draw.drawCorPointR, e.Start);
            }
        }

        //绘制车道
        if (MathUtil.StateCheck(drawStyle, RoadDrawStyle.CarPath))
        {
            drawInst.color = Color.green;
            for (int i = 0; i < pathMap.links.Count; i++)
            {
                var it = pathMap.links[i];
                for (int j = 0; j < it.style.lanes / 2; j++)
                {
                    var b = it[j].EnterPoint;
                    var c = it[j].ExitPoint;
                    drawInst.lineWidth = roadArgs_Draw.drawLineWidth / 4;
                    drawInst.DrawArrowLine(b, c, roadArgs_Draw.drawLineWidth / 2);
                    drawInst.lineWidth = roadArgs_Draw.drawLineWidth;
                }
            }
        }

        EventMgr.DrawStyleChanged.Notify(drawStyle);
        //Shader.PropertyToID(ShaderParameters.RoadOwnerColor)
    }
}
