--事件打印类

function EventMgr_Print()
	--默认托管给退出游戏的全局句柄
	local handle=GlobalMgr.instance.ExitHandle
	local _printCSharp=function (name,eventData)
		printColor(Config_PrintColor.Azure,name..Torsion.Serialize(eventData,true,false,false,100))  
	end
	local _printLua=function (name,eventData)
		printColor(Config_PrintColor.Azure,name..table.SerializeValue(eventData))  
	end

	EventMgr_LanguageChange.CallBack(function (eventData)
		_printCSharp("语言切换(LanguageChange)\n",eventData)
	end,handle)
	EventMgr_WindowShow.CallBack(function (eventData)
		_printCSharp("窗口显示(WindowShow)\n",eventData)
	end,handle)
	EventMgr_WindowHide.CallBack(function (eventData)
		_printCSharp("窗口隐藏(WindowHide)\n",eventData)
	end,handle)
	EventMgr_DoOpenOrCloseLoadingDlg.CallBack(function (eventData)
		_printCSharp("执行打开或者关闭加载窗口(DoOpenOrCloseLoadingDlg)\n",eventData)
	end,handle)
	EventMgr_ApplicationQuit.CallBack(function (eventData)
		_printCSharp("退出应用(ApplicationQuit)\n",eventData)
	end,handle)
	EventMgr_ApplicationPause.CallBack(function (eventData)
		_printCSharp("应用暂停(ApplicationPause)\n",eventData)
	end,handle)
	EventMgr_AndroidClick.CallBack(function (eventData)
		_printCSharp("Android交互(AndroidClick)\n",eventData)
	end,handle)
end

EventMgr_Print()
