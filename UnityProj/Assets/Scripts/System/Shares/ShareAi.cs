using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
/// <summary>
/// 股票模拟投资
/// </summary>
public class ShareAi
{
    public Action<string> OnMessage;

    /// <summary>
    /// 策略数据
    /// </summary>
    TradeStrategyData data;

    /// <summary>
    /// 历史数据
    /// </summary>
    List<SharesHistoryData> historyData;

    /// <summary>
    /// 当前持有
    /// </summary>
    public int currentCount;

    /// <summary>
    /// 持仓前资金
    /// </summary>
    public float inMoney;

    /// <summary>
    /// 持有资金
    /// </summary>
    public float holdMoney;

    /// <summary>
    /// 当前价格
    /// </summary>
    public float CurrentPrice
    {
        get
        {
            return historyData[dayIndex].Close;
        }
    }
    /// <summary>
    /// 浮动盈亏率
    /// </summary>
    public float EarnPercent
    {
        get
        {
            var k = (float)Math.Round(CurrentPrice / CostPrice - 1, 4);
            if (currentCount < 0) return -k;
            return k;
        }
    }
    /// <summary>
    /// 成本,均价
    /// </summary>
    public float CostPrice
    {
        get
        {
            return (inMoney - holdMoney) / currentCount;
        }
    }

    /// <summary>
    /// 历史最大亏损率
    /// </summary>
    public float HistoryMaxLossK;
    /// <summary>
    /// 最大可买数量
    /// </summary>
    public int MaxInCount()
    {
        return (int)Math.Floor(holdMoney / CurrentPrice);
    }
    public ShareAi(TradeStrategyData data,string code,int months)
    {
        this.data = data;
        //一个月大概5*4个数据
        historyData = SharesHistoryModel.instance.GetHistoryDataList(code, months==0?-1:months * 20);
        historyData.Reverse();
    }

    /// <summary>
    /// 模拟从历史数据最早开始作投资
    /// </summary>
    public void SimulatedInvestment()
    {
        holdMoney = data.StartGold;
        dayIndex = 0;
        do
        {
            if(OnMessage!=null)OnMessage(string.Format("{0} {1:F}", historyData[dayIndex].Date, CurrentPrice));
            DoByDay();
            PrintState();
            if (holdMoney == 0) break;
            dayIndex++;
        } while (dayIndex < historyData.Count);
    }
    int dayIndex;

    public void SetCurrentDay()
    {
        dayIndex = historyData.Count - 1;
    }

    /// <summary>
    /// 
    /// </summary>
    public void DoByDay()
    {
        tempCount = 0;
        
        ///最后一天平仓看结果
        if (dayIndex== historyData.Count-1)
        {
            if (currentCount != 0)
                ScaleInOut(-currentCount);
            return;
        }
        if(currentCount == 0)
        {
            if (dayIndex > data.FristBuyDays)
            {
                if(PriceLow(data.FristBuyDays))
                {
                    ScaleInOut(data.FristVolume);
                    return;
                }
            }
            if (dayIndex > data.FristSellDays)
            {
                if (PriceHigh(data.FristSellDays))
                {
                    ScaleInOut(-data.FristVolume);
                    return;
                }
            }
        }
        else
        {
            var ep = EarnPercent;
            if (ep < 0)
            {
                HistoryMaxLossK = Math.Max(HistoryMaxLossK, -ep);
                if (HistoryMaxLossK > data.MaxLossK/100)
                {
                    currentCount = 0;
                    holdMoney = 0;
                    return;
                }
            }
            //满足条件系统强制平仓(如果平仓后的资金为负)
            if (holdMoney + CurrentPrice * currentCount < 0)
            {
                currentCount = 0;
                holdMoney = 0;
            }
            else if (ep > 0 && ep > data.IncomeK / 100)
            {
                ScaleInOut(-currentCount);
            }
            //亏损超过一定比例则拉近均价
            else if (ep < 0 && -ep > data.LossK / 100)
            {
                if (currentCount > 0)
                {
                    while (-EarnPercent > data.StopLossK / 100)
                    {
                        var bl = ScaleInOut(1);
                        if (!bl) break;
                    }
                }
                else
                {
                    while (-EarnPercent > data.StopLossK / 100)
                    {
                        var bl = ScaleInOut(-1);
                        if (!bl) break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 当日价格小于近期历史任何一段时间的均价,
    /// </summary>
    public bool PriceLow(int day)
    {
        var price = historyData[dayIndex].Close;
        float total = price;
        for (int i = 1; i < day; i++)
        {
            total += historyData[dayIndex - i].Close;
            if (total < (i+1)*price) return false;
        }
        return true;
    }
    /// <summary>
    /// 当日价格大于近期历史任何一段时间的均价,
    /// </summary>
    public bool PriceHigh(int day)
    {
        var price = historyData[dayIndex].Close;
        float total = price;
        for (int i = 1; i < day; i++)
        {
            total += historyData[dayIndex - i].Close;
            if (total > (i + 1) * price) return false;
        }
        return true;
    }

    /// <summary>
    /// 加减仓/建仓(count 负数为减仓)
    /// </summary>
    public bool ScaleInOut(int count)
    {
        if (currentCount == 0) inMoney = holdMoney;
        
        holdMoney -= CurrentPrice * count;
        //持有资金不能为负(做多超过本金),也不能超过刚开始持有的2倍(借钱做空超过本金)
        if(holdMoney < 0 || holdMoney>2*inMoney)
        {
            holdMoney += CurrentPrice * count;
            return false;
        }
        currentCount += count;
        tempCount += count;
        
        return true;
    }
    /// <summary>
    /// 当日成交量
    /// </summary>
    int tempCount;
    public void PrintState()
    {
        if (OnMessage != null)
        {
            if(holdMoney==0)
            {
                OnMessage(string.Format("\t系统强制平仓,你完蛋了"));
            }
            else
            {
                if (currentCount != 0)
                {
                    OnMessage(string.Format("\t当前持有({0}),盈亏({1:N2}%)",
                        currentCount, EarnPercent * 100, holdMoney));
                }
                if (tempCount != 0)
                {
                    if (currentCount == 0)
                        OnMessage(string.Format("\t平仓  剩余资金({0})", holdMoney));
                    else
                        OnMessage(string.Format("\t{0}{1}  均价{2:F}  剩余资金({3:F})", tempCount > 0 ? "买入" : "卖出", Math.Abs(tempCount), CostPrice, holdMoney));
                }
            }
            OnMessage("\n");
        }
        
    }
}
*/
