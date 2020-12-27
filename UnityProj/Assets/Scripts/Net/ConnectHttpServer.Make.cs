//using CqCore;
//using CustomModel;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Http.Send;
//using Http.Receive;

//public class HttpReturn
//{
//        public string data;
//        public uint status;
//        public uint info;
//}

//namespace Http.Send
//{
//	public class UploadSuitData
//	{
//		public int msgId=101;
//		public string token;
//		public string name;
//		public int id;
//		public string data;
//	}
//}

//namespace Http.Send
//{
//	public class GetSuitData
//	{
//		public int msgId=102;
//		public string token;
//		public int id;
//	}
//}

//namespace Http.Send
//{
//	public class LoginHttpServer
//	{
//		public int msgId=103;
//		public string username;
//		public string password;
//	}
//}

//namespace Http.Send
//{
//	public class SendOrderInfo
//	{
//		public int msgId=104;
//		public string token;
//		public List<KSTechInfo> order_info;
//		public string name;
//		public string phone;
//	}
//}

//namespace Http.Send
//{
//	public class LoginOut
//	{
//		public int msgId=105;
//		public string token;
//	}
//}

//namespace Http.Send
//{
//	public class KSTechInfo
//	{
//		public uint style;
//		public string sprite;
//		public Dictionary<string, uint> dic;
//	}
//}

//namespace Http.Send
//{
//	public class GetServerInfo
//	{
//		public int msgId=1234;
//	}
//}

//namespace Http.Receive
//{
//	public class UploadSuitDataReturn
//	{
//	}
//}

//namespace Http.Receive
//{
//	public class SuitDataReturn
//	{
//		public string suit;
//	}
//}

//namespace Http.Receive
//{
//	public class LoginHttpServerReturn
//	{
//		public string token;
//	}
//}

//namespace Http.Receive
//{
//	public class OrderInfoReturn
//	{
//		public uint style_id;
//		public uint cate_id;
//		public List<string> cell;
//	}
//}

//namespace Http.Receive
//{
//	public class LoginOutReturn
//	{
//	}
//}

//namespace Http.Receive
//{
//	public class ServerInfoReturn
//	{
//		public List<string> httpServerUrl;
//		public List<string> resServerUrl;
//	}
//}

//public partial class ConnectHttpServer : Singleton<ConnectHttpServer>
//{
//    /// <summary>
//    ///  上传套装数据
//    /// </summary>
//    public void UploadSuitData(string token,string name,int id,string data,Action<HttpReturn,UploadSuitDataReturn> OnReceive=null)
//    {
//	var _dic = new Dictionary<string, object>();
//	_dic["msgId"] = 101;
//	_dic["token"] = token;
//	_dic["name"] = name;
//	_dic["id"] = id;
//	_dic["data"] = data;
//#if false
//	Debug.Log(string.Format("{0}({1})\n{2}", "上传套装数据", "UploadSuitData", Json.Serialize(_dic)));
//#endif
//        Request(_dic, OnReceive);
//    }
//    /// <summary>
//    ///  获取套装数据
//    /// </summary>
//    public void GetSuitData(string token,int id,Action<HttpReturn,SuitDataReturn> OnReceive=null)
//    {
//	var _dic = new Dictionary<string, object>();
//	_dic["msgId"] = 102;
//	_dic["token"] = token;
//	_dic["id"] = id;
//#if false
//	Debug.Log(string.Format("{0}({1})\n{2}", "获取套装数据", "GetSuitData", Json.Serialize(_dic)));
//#endif
//        Request(_dic, OnReceive);
//    }
//    /// <summary>
//    ///  登录
//    /// </summary>
//    public void LoginHttpServer(string username,string password,Action<HttpReturn,LoginHttpServerReturn> OnReceive=null)
//    {
//	var _dic = new Dictionary<string, object>();
//	_dic["msgId"] = 103;
//	_dic["username"] = username;
//	_dic["password"] = password;
//#if true
//	Debug.Log(string.Format("{0}({1})\n{2}", "登录", "LoginHttpServer", Json.Serialize(_dic)));
//#endif
//        Request(_dic, OnReceive);
//    }
//    /// <summary>
//    ///  发送工艺信息
//    /// </summary>
//    public void SendOrderInfo(string token,List<KSTechInfo> order_info,string name,string phone,Action<HttpReturn,OrderInfoReturn> OnReceive=null)
//    {
//	var _dic = new Dictionary<string, object>();
//	_dic["msgId"] = 104;
//	_dic["token"] = token;
//	_dic["order_info"] = order_info;
//	_dic["name"] = name;
//	_dic["phone"] = phone;
//#if true
//	Debug.Log(string.Format("{0}({1})\n{2}", "发送工艺信息", "SendOrderInfo", Json.Serialize(_dic)));
//#endif
//        Request(_dic, OnReceive);
//    }
//    /// <summary>
//    ///  退出登录
//    /// </summary>
//    public void LoginOut(string token,Action<HttpReturn,LoginOutReturn> OnReceive=null)
//    {
//	var _dic = new Dictionary<string, object>();
//	_dic["msgId"] = 105;
//	_dic["token"] = token;
//#if false
//	Debug.Log(string.Format("{0}({1})\n{2}", "退出登录", "LoginOut", Json.Serialize(_dic)));
//#endif
//        Request(_dic, OnReceive);
//    }
//    /// <summary>
//    ///  获取服务器信息
//    /// </summary>
//    public void GetServerInfo(Action<HttpReturn,ServerInfoReturn> OnReceive=null)
//    {
//	var _dic = new Dictionary<string, object>();
//	_dic["msgId"] = 1234;
//#if false
//	Debug.Log(string.Format("{0}({1})\n{2}", "获取服务器信息", "GetServerInfo", Json.Serialize(_dic)));
//#endif
//        Request(_dic, OnReceive);
//    }
//}
