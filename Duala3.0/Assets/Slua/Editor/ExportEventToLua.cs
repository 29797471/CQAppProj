using SLua;

public static class ExportEventToLua
{
	public static void OnAddCustomClass(LuaCodeGen.ExportGenericDelegate add)
	{
		add(typeof(EventMgr.ServerTimeUpdate), null);
		add(typeof(EventMgr.NetPingUpdate), null);
		add(typeof(EventMgr.LanguageChange), null);
		add(typeof(EventMgr.CmdCommand), null);
		add(typeof(EventMgr.WindowShow), null);
		add(typeof(EventMgr.WindowHide), null);
		add(typeof(EventMgr.WindowFadeInStart), null);
		add(typeof(EventMgr.WindowFadeInEnd), null);
		add(typeof(EventMgr.WindowFadeOutStart), null);
		add(typeof(EventMgr.WindowFadeOutEnd), null);
		add(typeof(EventMgr.ButtonClick), null);
		add(typeof(EventMgr.ToggleClick), null);
		add(typeof(EventMgr.SimulatorToggleClick), null);
		add(typeof(EventMgr.DoOpenOrCloseLoadingDlg), null);
		add(typeof(EventMgr.DoOpenUserOprFail), null);
		add(typeof(EventMgr.GameStartup), null);
		add(typeof(EventMgr.CanvasInitComplete), null);
		add(typeof(EventMgr.ResUpdateComplete), null);
		add(typeof(EventMgr.InitServerAddressComplete), null);
		add(typeof(EventMgr.LoginComplete), null);
		add(typeof(EventMgr.InitModel), null);
		add(typeof(EventMgr.ResLoadStart), null);
		add(typeof(EventMgr.ResLoadEnd), null);
		add(typeof(EventMgr.DrawStyleChanged), null);
		add(typeof(EventMgr.ApplicationQuit), null);
		add(typeof(EventMgr.ApplicationPause), null);
		add(typeof(EventMgr.AndroidClick), null);
		add(typeof(EventMgr.KeyBoardDown), null);
		add(typeof(EventMgr.MoreInputMove), null);
		add(typeof(EventMgr.OneInputMove), null);
		add(typeof(EventMgr.DeviceDataUpdate), null);
		add(typeof(EventMgr.MsgBox), null);
		add(typeof(EventMgr.MsgBalloon), null);
		add(typeof(EventMgr.MsgPrint), null);
		//add(typeof(EventMgr.%EventId%._EventArgs), null);
	}
}