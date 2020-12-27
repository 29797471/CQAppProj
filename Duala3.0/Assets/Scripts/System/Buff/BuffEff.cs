//using CqCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

///// <summary>
///// Buff效果对象
///// </summary>
//public class BuffEff
//{
//    uint percent;
//    BuffEffectTable effData;

//    float data;//每次触发特效的增量

//    float totalBuffData;//记录总增量值,当销毁时如果要清除属性增量,则处理

//    public BuffEff(BuffConfigTable.BuffEffect tableData,uint lv,Buff buff)
//    {
//        effData=BuffEffectTableManager.instance.Find(tableData.id);
//        percent = tableData.percentStart+ tableData.percentDelta * (lv - 1);
//        data= (float)(effData.data.start+ effData.data.delta * (lv - 1));
//        totalBuffData = 0;
//        if(effData.cancelBuff==1)//属性为0时取消Buff
//        {
//            buff.target.OnSetAttritube += (arg1, arg2, arg3) =>
//            {
//                if ((uint)arg1 == effData.target && (uint)arg2 == effData.type && arg3==0)
//                {
//                    buff.Exit();
//                }
//          };
//        }
//    }
//    private void Usc_OnSetAttritube(uint arg1, uint arg2, float arg3)
//    {

//    }
//    public void DoEffect(BuffObject target)
//    {
//        if (UnityEngine.Random.Range(0, 10000 - 1) < percent)
//        {
//            switch (effData.style)
//            {
//                case 1:
//                    target.ChangeAttr(effData.target, effData.type, effData.fromType, effData.fromTarget,effData.notSelf==0, data,false );
//                    break;
//                case 2:
//                    target.ChangeAttr(effData.target, effData.type, effData.fromType, effData.fromTarget, effData.notSelf == 0, data,true);
//                    break;
//                case 3:
//                    target.ChangeBuffAttr(effData.target, effData.type, data,false);
//                    totalBuffData += data;
//                    break;
//                case 4:
//                    target.ChangeBuffAttr(effData.target, effData.type, data, true);
//                    totalBuffData += data;
//                    break;
//                case 12:
//                    target.ChangeAttr(effData.target, effData.type, effData.fromType, effData.fromTarget, effData.notSelf == 0, data, true);
//                    break;
//            }
            
            
//        }
//    }
//    public void Cancel(BuffObject target)
//    {
//        switch (effData.style)
//        {
//            case 3://如果有临时性的属性加成，清除加成的属性值
//				if(totalBuffData!=0)
//				{
//					target.ChangeBuffAttr(effData.target, effData.type, -totalBuffData);
//					totalBuffData=0;
//				}
//                break;
//            case 4://如果有临时性的属性加成百分比，清除加成的属性百分比
//				if(totalBuffData!=0)
//				{
//					target.ChangeBuffAttr(effData.target, effData.type, -totalBuffData, true);
//					totalBuffData=0;
//				}
//                break;
//            case 12://效果消失时当该属性不为0,设置属性为0
//                if(target.GetAttr(effData.target, effData.type)!=0)target.SetAttr(effData.target, effData.type, 0);
//                break;
//        }
//    }
//}