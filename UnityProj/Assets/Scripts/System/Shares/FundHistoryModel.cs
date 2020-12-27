using CqCore;
using System;
using System.Collections;

/// <summary>
/// 基金历史数据接口
/// </summary>
public class FundHistoryModel : Singleton<FundHistoryModel>
{
    private const string url_get_old_jz = "http://fund.eastmoney.com/pingzhongdata/{0}.js";

    private const string pattern_get_old_jz = "var ([a-zA-Z].*?) ?= ?(.*?);";


    private const string codeMatch = @"^\d{6}$";

    static Causality1_2Async<string, int, string> GetFundOldData_Ca;

    /// <summary>
    /// 最新估算净值查询
    /// </summary>
    public void GetFundOldData(string fundcode, Action<int, string> OnResult)
    {
        
        if (GetFundOldData_Ca == null)
        {
            GetFundOldData_Ca = new Causality1_2Async<string, int, string>(_GetFundOldData);
        }
        GetFundOldData_Ca.CallAsync(fundcode, OnResult);
    }

    /// <summary>
    /// 历史净值查询
    /// </summary>
    void _GetFundOldData(string fundcode, Action<int, string> OnResult)
    {
        if (!RegexUtil.IsMatch(fundcode, codeMatch))
        {
            OnResult(4,null);
            return;
        }
        WaitLoadingMgr.instance.Start(RequestFunCodeOld(fundcode, OnResult));
    }
    IEnumerator RequestFunCodeOld(string fundcode, Action<int,string> OnResult)
    {
        var asyncReturn = new AsyncReturn<string>();
        var request = new CqRequest(string.Format(url_get_old_jz, fundcode));
        yield return request.SendAsync(asyncReturn);
        var result = asyncReturn.data;
        if (result == null)
        {
            OnResult(3, null);
        }
        else
        {
            try
            {
                //将javaScript 转化成lua
                var groupXX = RegexUtil.Matches(result, pattern_get_old_jz);
                
                string res = "";
                var last = groupXX[groupXX.Count - 2];
                foreach (var it in groupXX)
                {
                    var key = it.Groups[1].Value;
                    var value = it.Groups[2].Value;
                    res += string.Format("\"{0}\":{1}", key, value);
                    if (it == last) break;
                    else res += ",";
                }
               
                OnResult(1,"{"+res+"}");
            }
            catch (Exception)
            {
                OnResult(2,null);
            }
        }
        
    }

}
