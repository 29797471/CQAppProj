//using CqCore;
//using System;
//using System.Collections.Generic;

///// <summary>
///// buff同类队列
///// </summary>
//public class BuffQueue
//{
//    BuffObject buffObj;

//    public BuffGroupTable groupTbl;

//    public event System.Action OnExit;
//    public BuffQueue(uint type, BuffObject buffObj)
//    {
//        this.buffObj = buffObj;
//        groupTbl = BuffGroupTableManager.instance.Find(type);

//        list = new List<Buff>();

//        //OnEnter += () => target.CurrentState |= tableData.state;
//        //OnExit += () => target.CurrentState ^= ~tableData.state;
//        OnExit+=()=> { while (list.Count > 0) { var it = list[0];it.Exit();if (list.Contains(it)) list.Remove(it); } };
//    }
//    /// <summary>
//    /// Buff分类
//    /// </summary>
//    public uint buffType
//    {
//        get
//        {
//            return groupTbl.id;
//        }
//    }

//    /// <summary>
//    /// 当存在时阻止的Buff分类表
//    /// </summary>
//    public List<uint> disabledTypeList
//    {
//        get
//        {
//            return groupTbl.incompatibleList.list;
//        }
//    }
//    public uint State
//    {
//        get
//        {
//            uint state = 0;
//            for(int i=0;i<list.Count;i++)
//            {
//                state |= list[i].buffState;
//            }
//            return state;
//        }
//    }
//    public bool IsEmpty()
//    {
//        return list.Count == 0;
//    }
//    List<Buff> list;
    
//    /// <summary>
//    /// Buff的叠加调用
//    /// </summary>
//    public Buff PushBuff(uint buffLv,uint buffId)
//    {
//        Buff buff = new Buff(buffId, buffObj,buffLv); 
//        list.Add(buff);if (OnUpdateCount != null) OnUpdateCount();
//        buff.OnExit += () => { list.Remove(buff); if(OnUpdateCount!=null) OnUpdateCount(); };
//        if (list.Count > groupTbl.maxCount)
//        {
//            PopBuff(buff);
//        }
//        if(list.Contains(buff))
//        {
//            buff.Enter();
//            return buff;
//        }
//        return null;
//    }
//    DelayHandle handle;
    
//    public event System.Action OnUpdateCount;
//    /// <summary>
//    /// 先按等级从低到高,再按剩余时间从少到多排列,移除第一个
//    /// </summary>
//    public void PopBuff(Buff newBuff)
//    {
//        switch(groupTbl.change)
//        {
//            case 0:
//                list.Sort((x, y) => { if (x.buffLv != y.buffLv) return x.buffLv - y.buffLv; else return x.times - y.times; });
//                break;
//            case 1:
//                list.Sort((x, y) => { if (x.buffLv != y.buffLv) return x.buffLv - y.buffLv; else return (int)(x.time - y.time); });
//                break;
//            case 2:
//                list.Sort((x, y) => { if (x.buffLv != y.buffLv) return x.buffLv - y.buffLv; else return (int)(y.time - x.time); });
//                break;
//        }
        
//        if(list[0]!= newBuff) list[0].Exit();
//        else list.Remove(newBuff);
//    }
//    public void Destory()
//    {
//        if (OnExit != null)
//        {
//            OnExit();

//            OnExit = null;
//        }
//    }
//}
