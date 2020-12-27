/// <summary>
/// 绘制自己的所有的道路的辅助线
/// </summary>
public partial class PathMapRule : Singleton<PathMapRule>
{
    /// <summary>
    /// 绘制辅助线
    /// </summary>
    PathMapDraw mDrawMyself;
    public PathMapDraw DrawMyself
    {
        get
        {
            if(mDrawMyself==null)
            {
                mDrawMyself = new PathMapDraw();
                mDrawMyself.Init(true);
                mDrawMyself.lineWidth = arp.roadArgs_Draw.drawLineWidth;
            }
            return mDrawMyself;
        }
    }
    
    public void ShowAllMyself(bool hide=false)
    {
        if(hide)
        {
            DrawMyself.Clear();
        }
        
    }
}

