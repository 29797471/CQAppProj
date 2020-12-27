using CqCore;
using System;
using System.Threading;
/*
using WinCore;

namespace CqBehavior.Task
{
    /// <summary>
    /// 股票策略分析
    /// </summary>
    [Editor("股票策略分析")]
    [MenuItemPath("添加/金融/股票/策略分析")]
    public class SharesForecast : CqActionNode
    {
        

        public int month;
        [Priority(2,1)]
        [TextBox("统计历史时间(月)"), MinWidth(100)]
        public int Month
        {
            get { return month; }
            set { month = value; Update("Month"); }
        }
        [Priority(2)]
        [TextBox("代码"), MinWidth(100)]
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; Update("Code"); }
        }
        public string m_Code;

        [Priority(5)]
        [CheckBox("显示每日操盘"), MinWidth(100)]
        public bool ShowCommand
        {
            get { return mShowCommand; }
            set { mShowCommand = value; Update("ShowCommand"); }
        }
        public bool mShowCommand;

        [Priority(5,1)]
        [CheckBox("随机全部策略数据"), MinWidth(100)]
        public bool RandomAll
        {
            get { return mRandomAll; }
            set { mRandomAll = value; Update("RandomAll"); }
        }
        public bool mRandomAll;

        [Priority(7)]
        [GroupBox(), Width(500)]
        public TradeStrategyData TradeStrategyData
        {
            get
            {
                if (tradeStrategyData == null) tradeStrategyData = new TradeStrategyData();
                return tradeStrategyData;
            }
            set
            {
                tradeStrategyData = value;
                Update("TradeStrategyData");
            }
        }
        public TradeStrategyData tradeStrategyData;

        [Button, Click("OnForecast")]
        [Priority(10)]
        public string Btn { get { return "模拟投资"; } }

        public void OnForecast(object obj)
        {
            var fe = new FileEdit { CustomCode = "" };
            var sa = new ShareAi(tradeStrategyData, m_Code,Month);
            if(ShowCommand)
            {
                sa.OnMessage = (str) =>
                {
                    fe.CustomCode += str;
                };
                sa.SimulatedInvestment();

                WinUtil.OpenEditorWindow(fe);
            }
            else
            {
                sa.SimulatedInvestment();
                Content =string.Format("最终资金:{0} 盈利{1:F}%,历史最大亏损率-{2:F}%",
                    sa.holdMoney,(sa.holdMoney/tradeStrategyData.mStartGold-1)*100,sa.HistoryMaxLossK*100);
            }
        }
        [Priority(10,1)]
        [TextBox("模拟结果", 100), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), MinWidth(100)]
        public string Content
        {
            get { return mContent; }
            set { mContent = value; Update("Content"); }
        }
        public string mContent;

        [Priority(15)]
        [TextBox("现有持仓", 100), MinWidth(100)]
        public int RealHold
        {
            get
            {
                return mRealHold;
            }
            set
            {
                mRealHold = value;
                Update("RealHold");
            }
        }
        public int mRealHold;

        /// <summary>
        /// 成本价
        /// </summary>
        [Priority(15,1),Visibility("RealHold", AttributeTarget.Parent,"x!=0")]
        [TextBox("成本价"),MinWidth(100)]
        public float AveragePrice
        {
            get
            {
                return mAveragePrice;
            }
            set
            {
                mAveragePrice = value;
                Update("AveragePrice");
            }
        }
        public float mAveragePrice;

        public string Btn2 { get { return "投资建议"; } }

        [Priority(16,1)]
        [TextBox(), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), MinWidth(100)]
        public string Proposal
        {
            get { return mProposal; }
            set { mProposal = value; Update("Proposal"); }
        }
        public string mProposal;

        /// <summary>
        /// 现有持仓情况的未来投资计划
        /// </summary>
        public void InvestmentAnalysis(object o)
        {
            if (RealHold == 0)
            {
                ShareAi sa = new ShareAi(tradeStrategyData, m_Code, Month);
                sa.SetCurrentDay();
                if (sa.PriceLow(tradeStrategyData.FristBuyDays))
                {
                    Proposal = string.Format("当前价格小于近期均价,应买入{0}", tradeStrategyData.FristVolume);
                }
                else if (sa.PriceHigh(tradeStrategyData.FristSellDays))
                {
                    Proposal = string.Format("当前价格大于近期均价,应卖出{0}", tradeStrategyData.FristVolume);
                }
                else
                {
                    Proposal = "当前状态不明确,不建议投资";
                }
            }
            else
            {
                ShareAi sa = new ShareAi(tradeStrategyData, m_Code, Month);
                sa.SetCurrentDay();
                if(RealHold>0)
                {
                    var price = (tradeStrategyData.IncomeK / 100 + 1) * AveragePrice;
                    
                    var priceX = (-tradeStrategyData.LossK / 100 + 1) * AveragePrice;

                    var priceY = (tradeStrategyData.StopLossK / 100 + 1) * priceX;

                    //price*RealHold+x*priceX=(RealHold+x)*priceY
                    var x = (priceY-price) * RealHold/(priceX-priceY);
                    var y = (int)x;
                    Proposal = string.Format("在{0:F}时平仓,在{1:F}时买入{3},买入后均价{2:F}",
                        price, priceX,priceY, y);
                }
                else
                {
                    var price = (-tradeStrategyData.IncomeK / 100 + 1) * AveragePrice;

                    var priceX = (tradeStrategyData.LossK / 100 + 1) * AveragePrice;

                    var priceY = (-tradeStrategyData.StopLossK / 100 + 1) * priceX;

                    //price*RealHold+x*priceX=(RealHold+x)*priceY
                    var x = (priceY - price) * RealHold / (priceX - priceY);
                    var y = (int)x;
                    Proposal = string.Format("在{0:F}时平仓,在{1:F}时卖出{3},买入后均价{2:F}",
                        price, priceX, priceY, -y);
                }
            }
        }

        protected override void OnExit()
        {
            //if(handle!=null)
            //{
            //    handle.Cancel();
            //    handle = null;
            //}
        }
        protected override void OnExecute()
        {
            ShowCommand = false;
            ShareAi sa = new ShareAi(tradeStrategyData, m_Code, Month);
            sa.SimulatedInvestment();
            var money = sa.holdMoney;

            ThreadUtil.Call(() =>
            {
                while(IsRuning)
                {
                    var temp = Torsion.Clone(tradeStrategyData);
                    if (RandomAll)
                    {
                        var t1 = RandomUtil.Random(5, 120);
                        temp.FristBuyDays = t1;
                        temp.FristSellDays = t1;
                        temp.IncomeK = RandomUtil.Random(10, 200) / 10f;
                        temp.StopLossK = RandomUtil.Random(10, 50) / 10f;
                        var min = (int)Math.Floor(temp.StopLossK * 10 + 10);
                        temp.LossK = RandomUtil.Random(min, 200) / 10f;
                        temp.FristVolume = RandomUtil.Random(20, 100);
                    }
                    else
                    {
                        switch (RandomUtil.Random(0, 5))
                        {
                            case 0:
                                var t1 = RandomUtil.Random(5, 120);
                                temp.FristBuyDays = t1;
                                temp.FristSellDays = t1;
                                break;
                            case 1:
                                temp.IncomeK = RandomUtil.Random(10, 200) / 10f;
                                break;
                            case 2:
                                var min = (int)Math.Floor(temp.StopLossK * 10);
                                temp.LossK = RandomUtil.Random(min, 200) / 10f;
                                break;
                            case 3:
                                var max = (int)Math.Floor(temp.LossK * 10);
                                temp.StopLossK = RandomUtil.Random(10, max) / 10f;
                                break;
                            case 4:
                                temp.FristVolume = RandomUtil.Random(10, 100);
                                break;
                        }
                    }

                    sa = new ShareAi(temp, m_Code, Month);
                    sa.SimulatedInvestment();
                    if (sa.holdMoney > money)
                    {
                        WpfThread.Call(() =>
                        {
                            TradeStrategyData = temp;
                            money = sa.holdMoney;
                            Content = money.ToString();
                        });
                        
                    }
                }
            });
        }
        //DelayHandle handle;
    }
}
*/