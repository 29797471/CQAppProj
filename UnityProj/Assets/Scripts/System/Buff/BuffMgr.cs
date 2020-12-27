//using CqCore;
//using System.Collections.Generic;
//using System;

///// <summary>
///// Buff管理器
///// </summary>
//public class BuffMgr : Singleton<BuffMgr>
//{
//    Dictionary<ulong ,BuffObject> dic;
//    public void Init()
//    {
//        dic = new Dictionary<ulong, BuffObject>();
//    }
//    /// <summary>
//    /// 退出关卡清除所有buff
//    /// </summary>
//    public void ClearAllBuff()
//    {
//        foreach (var it in dic.Values)
//        {
//            it.Destory();
//        }
//        dic.Clear();
//    }
//    /// <summary>
//    /// 外部系统回调单位死亡
//    /// </summary>
//    public void OnUnitDead(uint unitId)
//    {
//        if(dic.ContainsKey(unitId))
//        {
//            var buffObj = dic[unitId];
//            buffObj.Destory();
//            dic.Remove(unitId);
//        }
//    }

//    public void Play(uint buffId, UnitStateCtrl usc,uint buffLv=1, UnitStateCtrl attackUnit = null)
//    {
//        if (usc.IsUnitDead())
//            return;

//        if(dic == null)
//        {
//            Init();
//        }
//        if (!dic.ContainsKey(usc.GetId()))
//        {
//            dic[usc.GetId()] = new BuffObject(usc);
//        }
//        dic[usc.GetId()].attackUnit = attackUnit;
//        //读buff表得到缓动参数,叠加方式
//        var buff = dic[usc.GetId()].Play(buffId, buffLv);
        
//        if (attackUnit != null && buff!=null)
//        {
//            Action action = null;
//            action = () =>
//            {
//                attackUnit.OnDestroyAction -= action;
//                buff.NoKeepAliveExit();
//            };

//            //读buff表得到缓动参数,叠加方式
//            attackUnit.OnDestroyAction += action;

//            buff.OnExit +=()=> attackUnit.OnDestroyAction -= action;
//        }
//    }

//}



