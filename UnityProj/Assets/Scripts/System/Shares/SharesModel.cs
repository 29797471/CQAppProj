using CqCore;
using System;
using System.Collections;
/*
先统计该股票振幅的平均范围，如果为-3%-+3%
策略（不计手续费）:当连续下跌超过-2%时以初投资金买入,当亏损超过-2%时，通过加仓调整亏损为-1%，
当赢利超过2%时通过减 仓调整持有资金为(3-盈利率)%*（初投资金）*k
*/
/// <summary>
/// 股票数据接口
/// </summary>
public class SharesModel : Singleton<SharesModel>
{

    /// <summary>
    /// 新浪最新股票数据查询接口
    /// </summary>
    private const string apiUrl = "http://hq.sinajs.cn/list={0}";
    public enum Market
    {
        /// <summary>
        /// 美国
        /// </summary>
        [EnumLabel("gb_")]
        USA,
        /// <summary>
        /// 上海
        /// </summary>
        [EnumLabel("sh")]
        ShangHai,
        /// <summary>
        /// 深圳
        /// </summary>
        [EnumLabel("sz")]
        ShenZhen,
        /// <summary>
        /// 香港
        /// </summary>
        [EnumLabel("hk")]
        HongKong,
    }

    static Causality1_3Async<string, Market,int, string> GetDataFormSina_Ca;
    /// <summary>
    /// 从新浪查询股票数据
    /// </summary>
    /// <param name="code"></param>
    public void GetDataFormSina(string code,Action<Market, int,string> OnResult)
    {
        _GetDataFormSina(code, OnResult);
        /*
        //SharesHistoryModel.instance.OpenDownloadUrl(code);
        if (GetDataFormSina_Ca==null)
        {
            GetDataFormSina_Ca = new Causality1_3Async<string, Market, int, string>(_GetDataFormSina);
        }
        GetDataFormSina_Ca.CallAsync(code, OnResult);
        */
    }
    void _GetDataFormSina(string code, Action<Market, int, string> OnResult)
    {
        Market m = GetMarket(code); 

        var _code = string.Format(apiUrl, EnumUtil.GetEnumLabelName(m) + code);

        WaitLoadingMgr.instance.Start(RequestDataFormSina(_code, m, OnResult));
    }
    
    IEnumerator RequestDataFormSina(string url, Market m, Action<Market, int, string> OnResult)
    {
        var asyncReturn = new AsyncReturn<string>();
        var request = new CqRequest(url);
        yield return request.SendAsync(asyncReturn);
        var result = asyncReturn.data;
        if (result == null)
        {
            OnResult(m,3, null);
        }
        else
        {
            var content = result.Split('"')[1];
            if (content == "FAILED" || content == "")
            {
                OnResult(m,2, null);
            }
            else
            {
                OnResult(m,1, content);
            }
        }
    }
    public Market GetMarket(string code)
    {
        if (char.IsLetter(code[0]))
        {
            return Market.USA;
        }
        else if (code[0] == '0' && code.Length == 5)
        {
            return Market.HongKong;
        }
        else if (code[0] == '0' && code.Length == 6)
        {
            return Market.ShenZhen;
        }
        return Market.ShangHai;
    }
}