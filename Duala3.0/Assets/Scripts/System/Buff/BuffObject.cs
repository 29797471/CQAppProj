//using UnityEngine;
//using System.Collections.Generic;
//using System;


///// <summary>
///// 承载了Buff的对象
///// 触发Buff分两种情况
///// 1.添加(和已有的BUFF是兼容的)(添加到同一分组队列里面)
///// 2.替换(和已有的BUFF不兼容)(通过优先级决定是否替换)
///// 
///// 当BUFF分组被销毁时,产生CD,CD时间到期后才能再次添加这种分组的BUFF
///// </summary>
//public class BuffObject
//{
//    public ulong id
//    {
//        get
//        {
//            return usc.GetId();
//        }
//    }
//    UnitStateCtrl usc;
//    public UnitStateCtrl attackUnit;
//    float lastData;

//    public List<BuffQueue> buffList;

//    public List<uint> CdGroupIds;

//    Dictionary<uint, Dictionary<uint, BuffAttrChange>> dicBuffAttrChange;
//    public BuffObject(UnitStateCtrl usc)
//    {
//        this.usc = usc;
//        buffList = new List<BuffQueue>();
//        CdGroupIds = new List<uint>();
//        dicBuffAttrChange = new Dictionary<uint, Dictionary<uint, BuffAttrChange>>();
//    }

//    public event Action<int , int , float > OnSetAttritube
//    {
//        add
//        {
//            usc.OnSetAttritube += value;
//        }
//        remove
//        {
//            usc.OnSetAttritube -= value;
//        }
//    }


//    /// <summary>
//    /// 播放角色特效
//    /// </summary>
//    public System.Action PlayEffect(string resEff, uint bindPoint)
//    {
//        return usc.SetEffect(resEff, bindPoint);
//    }

//    /// <summary>
//    /// 获取角色的变化属性(角色的变化属性通常不仅仅由BUFF控制,比如:当前生命值,当前护盾值)
//    /// </summary>
//    public float GetAttr(uint unitType, uint attrKey)
//    {
//        return usc.Get(unitType,attrKey);
//    }

//    /// <summary>
//    /// 设置角色的变化属性(角色的变化属性通常不仅仅由BUFF控制,比如:当前生命值,当前护盾值)
//    /// </summary>
//    public void SetAttr(uint unitType, uint attrKey, float value)
//    {
//        usc.Set(unitType, attrKey, value);
//    }

//    /// <summary>
//    /// 改变的是角色的变化属性,角色的变化属性通常不仅仅由BUFF控制,比如:当前生命值,当前护盾值
//    /// 这种改变一般是永久性的,不会在BUFF移除时消失
//    /// 当属性百分比加成按施放者的属性作计算时,当施放者不存在时按上一次数据计算.
//    /// </summary>
//    public void ChangeAttr(uint unitType, uint attrKey, uint fromUnitType, uint fromAttrKey, bool isSelf,float deltaOrPercent,bool isPercent)
//    {
//        var baseData= usc.Get(unitType,attrKey);
//        if(isPercent)
//        {
//            var target = isSelf ? usc: attackUnit;
//            var xx = (target == null) ? lastData : target.Get(fromUnitType, fromAttrKey);
            
//            usc.Set(unitType, attrKey, xx * (deltaOrPercent / 10000) + baseData);
//            //FloatWindow.PlayFloat("永久改变 武器:" + unitType + " 属性:" + attrKey + "百分比加成:" + delta);
//            if (!isSelf)
//            {
//                lastData = xx;
//            }
//        }
//        else
//        {
//            usc.Set(unitType, attrKey, deltaOrPercent + baseData);
//            //FloatWindow.PlayFloat("永久改变 武器:" + unitType + " 属性:" + attrKey + "加成:" + delta);
//        }
//    }
//    /// <summary>
//    /// 改变的是属性计算函数,当战斗计算时由公式返回实际值
//    /// 这种改变一般是临时性的,在移除时增加的值会被减去
//    /// 这个属性的最终值只会由BUFF提供,当外部获取时公式返回结果,当buff不存在时调用函数直接返回原始值
//    /// </summary>
//    public void ChangeBuffAttr(uint unitType, uint attrKey, float data, bool isPercent = false)
//    {
//        if (!dicBuffAttrChange.ContainsKey(unitType)) dicBuffAttrChange[unitType] = new Dictionary<uint, BuffAttrChange>();
//        if (!dicBuffAttrChange[unitType].ContainsKey(attrKey))
//        {
//            var bac= new BuffAttrChange();
//            dicBuffAttrChange[unitType][attrKey] = bac;
//            usc.Set(unitType,attrKey, bac.Calculate);
//        }
//        var it = dicBuffAttrChange[unitType][attrKey];
//        if (isPercent)
//        {
//            it.percent += data;
//            //FloatWindow.PlayFloat("武器:"+ unitType + " 属性:" + attrKey + "百分比加成:"+data);
//        }
//        else
//        {
//            it.delta += data;
//            //FloatWindow.PlayFloat("武器:" + unitType + " 属性:" + attrKey + "加成:" + data);
//        }
//    }

//    public Buff Play(uint buffId,uint buffLv)
//    {
//        var type = BuffConfigTableManager.instance.Find(buffId).type;
//        var groupTbl = BuffGroupTableManager.instance.Find(type);
//        var typeList = groupTbl.incompatibleList.list;
//        System.Action removeGroups = null;

//        //正在CD
//        if(CdGroupIds.Contains(type))
//        {
//            return null;
//        }

//        /// 检测Buff不并存的分类,存在时比较优先级
//        for (int i = 0; i < buffList.Count; i++)
//        {
//            //是否已有该分组
//            var buffGroup = buffList[i];
//            if (buffGroup.buffType==type)
//            {
//                return buffList[i].PushBuff(buffLv, buffId);
//            }
//            //是否和已有的分组不兼容
//            if (typeList.Contains(buffGroup.buffType))
//            {
//                //比较优先级,添加buff的分组优先级较低,直接返回
//                if(groupTbl.priority<buffGroup.groupTbl.priority)
//                {
//                    return null;
//                }
//                else
//                {
//                    removeGroups += ()=> RemoveGroup(buffGroup);
//                }
//            }
//        }
//        if(removeGroups!=null)removeGroups();
//        var newGroup = AddGroup(type);
//        return newGroup.PushBuff(buffLv, buffId);
//    }
    
//    void UpdateState()
//    {
//        uint state = 0;
//        for(var i=0;i< buffList.Count;i++)
//        {
//            var it = buffList[i];
//            if (!it.IsEmpty())
//            {
//                state |= buffList[i].State;
//            }else
//            {
//                RemoveGroup(it);
//            }
//        }
//        usc.CurrentState = state;
//    }

//    public BuffQueue AddGroup(uint type)
//    {
//        var buffQueue = new BuffQueue(type, this);
//        buffQueue.OnUpdateCount += UpdateState;
//        buffList.Add(buffQueue);
//        return buffQueue;
//    }
//    public bool RemoveGroup(BuffQueue group)
//    {
//        group.Destory();
//        CdGroupIds.Add(group.buffType);
//        UnityDelay.Call((float)group.groupTbl.appendCd, () => { CdGroupIds.Remove(group.buffType); });
//        return buffList.Remove(group);
//    }
//    public void Destory()
//    {
//        for(int i=0;i<buffList.Count;i++)
//        {
//            var it = buffList[i];
//            it.Destory();
//        }
//        buffList.Clear();
//    }

    
//}
    
