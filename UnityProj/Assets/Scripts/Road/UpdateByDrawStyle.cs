using CqCore;
using UnityCore;

public class UpdateByDrawStyle : MonoBehaviourExtended
{
    [ComBox("绘制", ComBoxStyle.RadioBox)]
    public RoadDrawStyle style;

    // Use this for initialization
    void Start ()
    {
        EventMgr.DrawStyleChanged.CallBack(DrawStyleChanged_EventHandler, DestroyHandle);
        DrawStyleChanged_EventHandler(null,null);
    }

    private void DrawStyleChanged_EventHandler(object sender, EventMgr.DrawStyleChanged._EventArgs e)
    {
        gameObject.SetActive(MathUtil.StateCheck(PathMapRule.instance.Arp.drawStyle, style));
    }
}
