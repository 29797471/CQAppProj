using System.Collections.Generic;

public partial class PathMapRule : Singleton<PathMapRule>
{
    

    /// <summary>
    /// 提供给新手引导,检查这个点是否连通高速路
    /// </summary>
    public bool CheckLinkToHighSpeed(UnityEngine.Vector2 a)
    {
        var element = PathMapInst.GetPathElement(a);
        if (element == null) return false;

        var checkedList = new List<PathV>();
        System.Predicate<PathE> CheckLinkPathE = null;
        System.Predicate<PathV> CheckLinkPathV = null;
        CheckLinkPathE = e => 
        {
            if (CheckLinkPathV(e.from) )return true;
            if (CheckLinkPathV(e.to)) return true;
            return false;
        };
        CheckLinkPathV = v =>
        {
            if (checkedList.Contains(v)) return false;
            checkedList.Add(v);
            
            foreach (var e in v.links)
            {
                if (CheckLinkPathE(e)) return true;
            }
            return false;
        };
        

        if (element is PathV)
        {
            if (CheckLinkPathV(element as PathV)) return true;
        }
        else if (element is PathE)
        {
            if (CheckLinkPathE(element as PathE)) return true;
        }
        return false;
    }
}


