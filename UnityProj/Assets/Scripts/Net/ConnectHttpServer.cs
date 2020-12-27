//using CqCore;
//using CustomModel;
//using System;
//using System.Collections.Generic;
//using UnityEngine;


//public partial class ConnectHttpServer : Singleton<ConnectHttpServer>
//{
//    public string httpServerUrl;
//    public string httpName;

//    /// <summary>
//    /// 要求客户端的最低版本
//    /// </summary>
//    public string requireClientVersion;

//    public bool NeedUpdateClient()
//    {
//        return string.Compare(requireClientVersion, Application.version) > 0;
//    }

//    /// <summary>
//    /// 初始化
//    /// 获取相关服务器地址
//    /// </summary>
//    public void Init()
//    {
//        httpServerUrl = "";//CustomSetting.Inst.serverUrl;
//        if(SettingModel.instance.HasKey("clientType"))
//        {
//            //CustomSetting.Inst.clientType =EnumUtil.ConvertStringToEnum<ClientTypeEnum>(SettingModel.instance.GetString("clientType"));
//        }
//        GetServerInfo( (result, data) =>
//        {
//            var httpServerList = data.httpServerUrl;
//            var resServerList = data.resServerUrl;
//            //var x=AssemblyUtil.GetMemberAttribute<ServerInfoAttribute>(EnumUtil.GetEnumFieldInfo( ));
            
//            var httpInfoList = httpServerList[0].Split(',');
//            httpServerUrl = httpInfoList[0];
//            if (httpInfoList.Length > 1) httpName = httpInfoList[1];
//            if (httpInfoList.Length > 2) requireClientVersion = httpInfoList[2];
            
//            if(true)
//            {
//                //ResMgrX.resName = EnumUtil.GetEnumLabelName(CustomSetting.Inst.resFrom);
//                //ResMgrX.resourcesUrl= ResMgrX.ToAbsolutePath(CustomSetting.Inst.localResPath);
//                ResMgrX.resVersion = "svn";
//            }
//            else
//            {
//                var resInfoList = resServerList[1].Split(',');
//                ResMgrX.resourcesUrl = resInfoList[0]+CustomSetting.Inst.netResPath;
//                if (resInfoList.Length > 1) ResMgrX.resName = resInfoList[1];
//                if (resInfoList.Length > 2) ResMgrX.resVersion = resInfoList[2];
//            }
//            ResMgrX.resVersion += "("+ requireClientVersion + ")";
//            EventMgr.InitServerAddressComplete.Notify();
//        });
//    }
//    DelayHandle Request<T>( Dictionary<string, object> args = null, Action<HttpReturn,T> OnReceive = null)
//    {
//        return null;
//        //Action OnCom = null;
//        //if (ResMgrX.resourcesUrl!=null)
//        //{
//        //    LoadMgr.ShowLoading();
//        //    OnCom = LoadMgr.StartLoading();
//        //}
//        //return WWWUtil.Request(httpServerUrl, args,_data=>
//        //{
//        //    if(OnCom != null) OnCom();
//        //    LoadMgr.HideLoading();
//        //    Debug.Log("服务器返回数据\n" + _data);
//        //    HttpReturn result=null;
//        //    try
//        //    {
//        //        result = Json.Deserialize<HttpReturn>(_data);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Debug.LogError("服务器返回数据格式错误\n" + e.Message);
//        //    }
//        //    if (result.info != 0)
//        //    {
//        //        //MessageModel.instance.Show(result.info);
//        //    }

//        //    if (OnReceive != null)
//        //    {
//        //        OnReceive(result, Json.Deserialize<T>(result.data));
//        //    }
//        //});
//    }
//}
