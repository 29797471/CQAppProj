using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PathMapRule : Singleton<PathMapRule>
{
    /// <summary>
    /// 提供给外部系统判定是否可以在两点之间修建道路
    /// 不可建造状态分类<para/>
    /// 1.两点不可重合<para/>
    /// 2.道路只能45度建造<para/>
    /// 3.道路不应交叉<para/>
    /// 4.道路建造最小宽度不够<para/>
    /// 5.道路不应和建筑交叉<para/>
    /// 6.道路太短<para/>
    /// 7.离路口太近<para/>
    /// 8.离道路尽头太近<para/>
    /// 9.超出建造范围(New)<para/>
    /// 10.未连通到公共道路<para/>
    /// 11.不能建造在其他玩家的辐射范围内<para/>
    /// 12.道路修建超出上限<para/>
    /// </summary>
    public int ErrorState { get; private set; }

    /// <summary>
    /// 提供给外部系统判定是否可以在两点之间修建道路
    /// 不可建造状态分类<para/>
    /// 1.两点不可重合<para/>
    /// 2.道路只能45度建造<para/>
    /// 3.道路不应交叉<para/>
    /// 4.道路建造最小宽度不够<para/>
    /// 5.道路不应和建筑交叉<para/>
    /// 6.道路太短<para/>
    /// 7.离路口太近<para/>
    /// 8.离道路尽头太近<para/>
    /// 9.超出建造范围(New)<para/>
    /// 10.未连通到公共道路<para/>
    /// 11.不能建造在其他玩家的辐射范围内<para/>
    /// 12.道路修建超出上限<para/>
    /// </summary>
    public bool CanBuildRoad(Vector2 a, Vector2 b)
    {
        int errorState = 0;
        System.Action DrawError = null;
        var bl= CheckRoad(a, b, ref errorState, ref DrawError);
        return bl;
    }
    /// <summary>
    /// 检查由这两点能否正确生成道路
    /// 参数返回错误信息
    /// </summary>
    bool CheckRoad(Vector2 a, Vector2 b, ref int errorState, ref Action DrawError)
    {
        var bl = CheckRoadX(a, b, ref errorState, ref DrawError);
        if(!bl)
        {
            DrawError += () =>
            {
                drawError.DrawCapsule(a, b, style.road_width / 2);
            };
            //Debug.Log("errorState="+errorState);
        }
        return bl;
    }

    /// <summary>
    /// 检查由这两点能否正确生成道路
    /// 参数返回错误信息
    /// </summary>
    bool CheckRoadX(Vector2 a, Vector2 b, ref int errorState, ref Action DrawError)
    {
        errorState = 0;
        var dir = b - a;
        if (dir == Vector2.zero)
        {
            errorState = 1;
            return false;
        }
        var aElement = PathMapInst.GetPathElement(a);
        
        var bElement = PathMapInst.GetPathElement(b);

        
        if (!Check45Correct(dir))
        {
            errorState = 2;
            return false;
        }
        if (!CheckOverlap(a,b,aElement, bElement))
        {
            errorState = 3;
            return false;
        }

        if (!CheckCorrect(a,b,aElement, bElement,ref DrawError, ref errorState))
        {
            return false;
        }
        if (!CheckCollider(a,b,aElement, bElement,ref DrawError,ref errorState))
        {
            return false;
        }
        //if (commonMap)
        //{
        //    //公共地图要求道路修建有长度限制
        //    var roadLen = Vector2.Distance(a, b);
        //    if (roadLen > allowCreateRoadLength)
        //    {
        //        ErrorLength = roadLen;
        //        errorState = 12;
        //        return false;
        //    }
        //}
        return true;
    }

    /// <summary>
    /// 超标的道路建造长度
    /// </summary>
    public float ErrorLength;


    bool Check45Correct(Vector2 dir)
    {
        if (!arp.roadArgs_Draw.limit45) return true;
        if (dir.x.EqualsByEpsilon(0)) return true;
        if (dir.y.EqualsByEpsilon(0)) return true;
        if (dir.x.EqualsByEpsilon(dir.y)) return true;
        if (dir.x.EqualsByEpsilon(-dir.y)) return true;
        return false;
    }
    /// <summary>
    /// 道路修建的合法性检测,保证显示正常且道路长度足够放至少一辆车.
    /// </summary>
    bool CheckCorrect(Vector2 a, Vector2 b, PathElement aElement, PathElement bElement, ref Action DrawError, ref int errorState)
    {
        drawError.color = Color.red;
        var x = CheckCorrectNew(a,b,aElement, bElement, ref DrawError,ref errorState);

        checkPathVs.Clear();
        checkPathMap.links.Clear();
        checkPathMap.list.Clear();
        return x;
    }
    /// <summary>
    /// 测试添加检测合法性的数据结构
    /// (主要用于,添加道路后道路情况比较复杂,可能是交叉路口相连,中间可能不满足人行横道线最小间距)
    /// </summary>
    PathMap checkPathMap;
    HashSet<PathV> checkPathVs;
    /// <summary>
    /// 道路修建的合法性检测,保证显示正常且道路长度足够(至少一辆车的长度).
    /// </summary>
    bool CheckCorrectNew(Vector2 a,Vector2 b,PathElement aElement, PathElement bElement,ref Action DrawError, ref int errorState)
    {
        var elements = new PathElement[] { aElement, bElement };
        for (int i = 0; i < 2; i++)
        {
            var element = elements[i];
            if (element is PathE)
            {
                var e = element as PathE;
                checkPathVs.Add(e.from);
                checkPathVs.Add(e.to);
            }
            else if (element is PathV)
            {
                var v = element as PathV;
                checkPathVs.Add(v);
                for (int j = 0; j < v.links.Count; j++)
                {
                    checkPathVs.Add(v.links[j].to);
                }
            }
        }
        List<PathE> _addList = new List<PathE>();
        foreach (var v in checkPathVs)
        {
            for (int i = 0; i < v.links.Count; i++)
            {
                var link = v.links[i];
                if (!_addList.Contains(link))
                {
                    checkPathMap.AddLinkWidthOutMerge(link.from.pos, link.to.pos, link.style);
                    _addList.Add(link);
                    _addList.Add(link.reE);
                }
            }
        }
        var ee = checkPathMap.AddLink(a, b, style);
        checkPathMap.ReCalcAll();
        for (int i = 0; i < checkPathMap.links.Count; i++)
        {
            var _link = checkPathMap.links[i];
            if (_link.OneSide)
            {
                if (!_link.CheckCorrect())
                {
                    DrawError+=() =>
                    {
                        drawError.DrawCapsule(_link.from.pos, _link.to.pos, _link.style.road_width / 2);
                    };
                    
                    if (ee == _link || ee == _link.reE) errorState = 6;
                    else
                    {
                        if (_link.from.IsRoadEnd() || _link.to.IsRoadEnd()) errorState = 8;
                        else errorState = 7;
                    }
                    return false;
                }
            }
        }

        return true;
    }



    /// <summary>
    /// 检查道路元素是否与传入道路相关联,相互关联时忽略碰撞检查
    /// </summary>
    System.Func<PathE, bool> CheckRelation(PathElement pathElement)
    {
        System.Func<PathE, bool> checkStart = null;
        if (pathElement == null)
        {
            checkStart = (e) => false;
        }
        else if (pathElement is PathV)
        {
            var v = pathElement as PathV;
            checkStart = (e) => e.from == v || e.to == v;
        }
        else if (pathElement is PathE)
        {
            var selectE = pathElement as PathE;
            checkStart = (e) => e == selectE || e.reE == selectE;
        }
        return checkStart;
    }
    /// <summary>
    /// 检查重叠(重叠返回false)
    /// </summary>
    bool CheckOverlap(Vector2 a, Vector2 b, PathElement fromElement, PathElement toElement)
    {
        var checkStart = CheckRelation(fromElement);
        var checkMouse = CheckRelation(toElement);
        var dir = (a - b).normalized;

        //排除在道路内,方向相同和相反的重叠
        if (fromElement is PathE)
        {
            if (toElement is PathV)
            {
                var fe = fromElement as PathE;
                var toV = toElement as PathV;
                if (fe.to == toV || fe.from == toV)
                {
                    a = b;
                    fromElement = toElement;
                    toElement = null;
                    return false;
                }
            }
            var e = fromElement as PathE;
            var eDir = e.Dir;
            if (eDir == dir || eDir == -dir)
                return false;
            if (checkMouse(fromElement as PathE))
                return false;
        }
        if (toElement is PathE)
        {
            var e = toElement as PathE;
            var eDir = e.Dir;
            if (eDir == dir || eDir == -dir)
                return false;
            if (checkStart(toElement as PathE))
                return false;
        }

        //排除同路点,在相同方向的重叠
        if (fromElement is PathV)
        {
            var v = fromElement as PathV;
            for (int i = 0; i < v.links.Count; i++)
            {
                if (v.links[i].Dir == -dir)
                    return false;
            }
        }

        if (toElement is PathV)
        {
            var v = toElement as PathV;
            for (int i = 0; i < v.links.Count; i++)
            {
                if (v.links[i].Dir == dir)
                    return false;
            }
        }

        //选中一条道路的两端时
        if (fromElement is PathV && toElement is PathV)
        {
            var _a = fromElement as PathV;
            var _b = toElement as PathV;
            for (int i = 0; i < _a.links.Count; i++)
            {
                if (_a.links[i].to == _b)
                    return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 检查是否与已有道路或者建筑相交(相交返回false)
    /// </summary>
    bool CheckCollider(Vector2 a,Vector2 b,PathElement fromElement, PathElement toElement, ref Action DrawError, ref int errorState)
    {
        var width = style.road_width;
        
        var _a = a + (a - b).normalized * width / 2;
        if (fromElement is PathV )
        {
            var v = fromElement as PathV;

            var len = v.links[0].style.HalfRoadW *  Mathf.Sqrt(2) - width/2;
            _a = a + (a - b).normalized * len;
        }
        else if(fromElement is PathE)
        {
            var ee = fromElement as PathE;
            var len = ee.style.HalfRoadW * Mathf.Sqrt(2) - width / 2;
            _a = a + (a - b).normalized * len;
        }

        var _b = b + (b - a).normalized * width / 2;
        if(toElement is PathV)
        {
            var v = toElement as PathV;
            var len = v.links[0].style.HalfRoadW * Mathf.Sqrt(2) - width / 2;
            _b = b + (b - a).normalized * len;
        }
        else if (toElement is PathE)
        {
            var ee = toElement as PathE;
            var len = ee.style.HalfRoadW * Mathf.Sqrt(2) - width / 2;
            _b = b + (b - a).normalized * len;
        }

        
        {
            for (int i = 0; i < PathMapInst.links.Count; i++)
            {
                var e = PathMapInst.links[i];
                //单向
                if (e.from.Index > e.to.Index)
                {
                    ///只有当道路跟两点都不关联时才检查碰撞
                    if (!e.InSegment(a) && !e.InSegment(b))
                    {
                        //还未处理
                        /*
                        if (Utility.OBBOverlap2D_P2P(
                        ref e.from.pos, ref e.to.pos, e.style.road_width,
                        ref _a, ref _b, style.road_width))
                        {
                            DrawError+=() =>
                            {
                                drawError.DrawCapsule(e.from.pos, e.to.pos, e.style.road_width / 2);
                            };
                            errorState = 3;
                            return false;
                        }
                        */
                    }
                }
            }
        }
        

        return true;
    }
}


