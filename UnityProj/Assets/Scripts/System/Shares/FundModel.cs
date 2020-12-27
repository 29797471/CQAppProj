using CqCore;
using System;
using System.Collections;

/// <summary>
/// 基金数据接口
/// </summary>
public class FundModel : SystemMgr<FundModel>
{
    /// <summary>
    /// 最新估算净值查询链接
    /// </summary>
    private const string url_get_new_gsjz = "http://fundgz.1234567.com.cn/js/{0}.js";

    private const string pattern_get_new_gsjz = @"\{(?<json>.*)\}";

    private const string codeMatch = @"^\d{6}$";

    Causality1_2Async<string, int,string> UpdateFunCode_Ca;

    /// <summary>
    /// 最新估算净值查询
    /// </summary>
    public void UpdateFunCode(string fundcode, Action<int, string> OnResult)
    {
        if (UpdateFunCode_Ca == null)
        {
            UpdateFunCode_Ca = new Causality1_2Async<string, int, string>(_UpdateFunCode);
        }
        UpdateFunCode_Ca.CallAsync(fundcode, OnResult);
    }
    /// <summary>
    /// 最新估算净值查询
    /// </summary>
    void _UpdateFunCode(string fundcode, Action<int,string> OnResult)
    {
        if(!RegexUtil.IsMatch(fundcode, codeMatch))
        {
            OnResult(4,null);
            return;
        }
        WaitLoadingMgr.instance.Start(RequestFunCode(fundcode, OnResult));
    }
    IEnumerator RequestFunCode(string fundcode, Action<int,string> OnResult)
    {
        var asyncReturn = new AsyncReturn<string>();
        var request = new CqRequest(string.Format(url_get_new_gsjz, fundcode));
        yield return request.SendAsync(asyncReturn);
        var text = asyncReturn.data;
        if (text == null)
        {
            OnResult( 3, null);
        }
        else
        {
            try
            {
                var g = RegexUtil.Matches(text, pattern_get_new_gsjz)[0].ToString();
                OnResult(1,g);
            }
            catch (Exception)
            {
                OnResult(2,null);
            }
        }
    }

   

}
