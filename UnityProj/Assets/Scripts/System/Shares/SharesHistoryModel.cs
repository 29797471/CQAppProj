using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 股票历史数据接口
/// </summary>
public class SharesHistoryModel : Singleton<SharesHistoryModel>
{
    public string SinaHisUrl = "http://money.finance.sina.com.cn/quotes_service/api/json_v2.php/CN_MarketData.getKLineData?symbol=sz000001&scale=5&ma=5&datalen=1023";
    public string USAFundDownloadUrl1 = "https://query1.finance.yahoo.com/v7/finance/download/{0}?period1=0&period2=9999999999&interval=1d&events=history&crumb=3Uq1jTK0hPQ";
    public string USAFundDownloadUrl = "https://query1.finance.yahoo.com/v7/finance/download/{0}?period1=0&period2=9999999999&interval=1d&events=history&crumb=CD2NdLaJRQ3";
    public string SHFundDownloadUrl = "";

    /// <summary>
    /// 美股数据下载地址
    /// </summary>
    public string usa_downurl = "https://finance.yahoo.com/quote/{0}/history?period1=0&period2=9951888000&interval=1d&filter=history&frequency=1d&guccounter=1";


    /*
    /// <summary>
    /// 获取历史数据
    /// </summary>
    public List<SharesHistoryData> GetHistoryDataList(string code,int count=-1)
    {
        var xx = OtherSet.DataPath;
        var filePath = string.Format(@"{0}\{1}.csv", xx, code.ToUpper());
        var e=FileOpr.GetFileEncoding(filePath);
        var txt = File.ReadAllText(filePath, e);
        List<SharesHistoryData> list = null;
        switch (SharesModel.instance.GetMarket(code))
        {
            case SharesModel.Market.USA:
                {
                    list= CSV.TryDeserialize<SharesHistoryData>(txt);
                    list.Reverse();
                    break;
                }
            case SharesModel.Market.ShenZhen:
            case SharesModel.Market.ShangHai:
                {
                    list = new List<SharesHistoryData>();
                    var temp = CSV.TryDeserialize<SharesHistoryData_China>(txt);
                    foreach(var it in temp)
                    {
                        list.Add(new SharesHistoryData()
                        {
                            Date = it.日期,
                            Open = it.开盘价,
                            Close = it.收盘价,
                            High = it.最高价,
                            Low = it.最低价,
                            Volume = it.成交量,
                        });
                    }
                    break;
                }
        }
        if (count == -1) return list;
        if (list!=null && list.Count > count) list.RemoveRange(count, list.Count - count);
        return list;
    }*/

    /// <summary>
    /// 打开历史股票数据链接手动下载
    /// </summary>
    /// <param name="code"></param>
    public void OpenDownloadUrl(string code)
    {
        var m = SharesModel.instance.GetMarket(code);
        switch (m)
        {
            case SharesModel.Market.HongKong:
                {
                    //https://m.cn.investing.com/equities/tencent-holdings-hk-historical-data?cid=964280
                }
                break;
            case SharesModel.Market.USA:
                {
                    /*
                    GlobalCoroutine.Start(RequestShareCodeOld(string.Format(USAFundDownloadUrl,
                        code)));*/

                    //浏览器对应的下载授权代码
                    HttpUtil.OpenUrl(string.Format(USAFundDownloadUrl,code), HttpUtil.BrowserStyle.Default);
                }
                break;
            case SharesModel.Market.ShenZhen:
            case SharesModel.Market.ShangHai:
                {

                    //浏览历史数据的链接
                    //http://www.aigaogao.com/tools/history.html?s={0}
                    //http://money.finance.sina.com.cn/corp/go.php/vMS_MarketHistory/stockid/{0}.phtml

                    //var url = "http://quotes.money.163.com/trade/lsjysj_{0}.html";
                    HttpUtil.OpenUrl(string.Format(SHFundDownloadUrl,
                        code), HttpUtil.BrowserStyle.Firefox);
                }
                break;
        }

        //打开下载链接
        //HttpUtil.OpenUrl(string.Format("https://finance.yahoo.com/quote/{0}/history",code));
    }

    IEnumerator RequestShareCodeOld(string url)
    {
        var asyncReturn = new AsyncReturn<string>();
        var request = new CqRequest(url);
        yield return request.SendAsync(asyncReturn);
        //(HttpUtil.ToWebRequest(url,null, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:77.0) Gecko/20100101 Firefox/77.0"), bytes);
        
        UnityEngine.Debug.Log(asyncReturn.data);
    }

    public void RequestChineseShares(string code,Action<string> OnResult)
    {
        WaitLoadingMgr.instance.Start(RequestChineseShares(DateTime.Now-TimeSpan.FromDays(360) ,code,3,false, OnResult));
    }

    /// <summary>
    /// 沪深、港股股票,历史数据查询
    /// </summary>
    IEnumerator RequestChineseShares(DateTime BeginDay, string Code, int Index, bool Rehabilitation/*复权*/, Action<string> OnResult)
    {
        var strs=new string[] { "5", "30", "60", "day", "week", "month" };
        string host = "http://stock.market.alicloudapi.com";
        string path = "/realtime-k";
        
        string appcode = "e22651a54c5c4151b7211aa430819cea";
        //string appKey = "61f5694c6345b05b6295ccbf281f1eb0";
        string querys = "beginDay=" + BeginDay.ToString("yyyyMMdd") + "&code=" + Code + "&time=" + strs[Index] + "&type=" + (Rehabilitation ? "qfq" : "bfq");
        //beginDay  开始时间，格式为yyyyMMdd，如果不写则默认是当天。结束时间永远是当前时间
        //code      沪深、港股股票编码
        //time      5 = 5分k线(默认) ，30 = 30分k线，60 = 60分k线，day = 日k线，week = 周k线，month = 月k线。注意港股不支持5分、30分和60分k线。
        //type 复权方式，支持两种方式 。 bfq =不复权(默认方式) qfq =前复权。当time为[day,week,month]时此字段有效。
        
        //string bodys = "";
        string url = host + path;
        

        if (0 < querys.Length)
        {
            url = url + "?" + querys;
        }
        var headers = new Dictionary<string, string>();
        headers["Authorization"] = "APPCODE " + appcode;

        var asyncReturn = new AsyncReturn<string>();
        var request = new CqRequest(url,headers);
        yield return request.SendAsync(asyncReturn);
        OnResult(asyncReturn.data);
    }
}