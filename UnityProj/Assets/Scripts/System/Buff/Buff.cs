//using CqCore;
//using System;
//using System.Collections.Generic;

///// <summary>
///// BUFF对象
///// </summary>
//public class Buff
//{
//    public uint id;
//    public int times;
//    public float deltaTime;
//    public int buffLv;
//    public uint buffState;

//    public BuffObject target;

//    event System.Action OnEnter;
//    public event System.Action OnExit;
//    BuffConfigTable tableData;
//    /// <summary>
//    /// buff创建时间
//    /// </summary>
//    public float time;
//    public Buff(uint buffId, BuffObject target, uint buffLv)
//    {
//        tableData = BuffConfigTableManager.instance.Find(buffId);
//        this.time = UnityEngine.Time.time;
//        this.id = tableData.id;
//        this.buffState = tableData.state;
//        this.buffLv = (int)buffLv;
//        times = (int)tableData.times;
//        this.target = target;
//        deltaTime = (float)(tableData.liveTime.start+ tableData.liveTime.delta * (buffLv - 1)) / tableData.times;
//        effList = new List<BuffEff>();
//        for (int i=0;i<tableData.effectList.list.Count;i++)
//        {
//            var buffEff = new BuffEff(tableData.effectList.list[i],buffLv,this);
//            effList.Add(buffEff);
//            OnExit +=()=> buffEff.Cancel(target);
//        }
//        OnExit += () =>effList.Clear();


//        ///特效
//        OnEnter+=()=>OnExit += target.PlayEffect(tableData.effect, tableData.effectPos);

//        ///属性改变
//        OnEnter += () => { TickEffect();  OnExit += () => { if (UnCallTickEffect != null) UnCallTickEffect.Cancel(); }; };

//        ///条件触发取消
//        OnEnter += () =>
//        {
//            int eventId = 0;
//            //eventId = MessageManager.instance.AddCallback(( id,  param1,  param2)=> 
//            //{
//            //    TriggerCommandData eventData = (TriggerCommandData)param1;
//            //    if (eventData.id== tableData.cancel)
//            //    {
//            //        MessageManager.instance.RemoveCallback(eventId);
//            //        eventId = 0;
//            //        Exit();
//            //    }
//            //},
//            //(int)EventEnum.TriggerCommand
//            //);
//            //OnExit += () => {if(eventId!=0) MessageManager.instance.RemoveCallback(eventId); eventId = 0; };
//        };

//    }

//    List<BuffEff> effList;
    
//    public void Enter()
//    {
//        if(OnEnter!=null)
//        {
//            OnEnter();

//            OnEnter = null;
//            //FloatWindow.PlayFloat("生成 Buff id=" + id);
//        }
//    }
//    public void NoKeepAliveExit()
//    {
//        if (tableData.keepAlive == 0) Exit();
//    }

//    public void Exit()
//    {
//        if (OnExit != null)
//        {
//            OnExit();

//            OnExit = null;
//            //FloatWindow.PlayFloat("销毁 Buff id=" + id);
//        }
//    }

//    /// <summary>
//    /// 触发一次效果
//    /// </summary>
//    public void TickEffect()
//    {
//        for (int i = 0; i < effList.Count; i++)
//        {
//            var eff = effList[i];
//            eff.DoEffect(target);
//        }

//        times--;
//        if (times > 0) UnCallTickEffect=UnityDelay.Call(deltaTime, TickEffect);
//        else
//        {
//            UnCallTickEffect=UnityDelay.Call(deltaTime, Exit);
//        }
//    }
//    DelayHandle UnCallTickEffect;
//}