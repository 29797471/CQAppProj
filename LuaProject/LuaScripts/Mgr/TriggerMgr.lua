super_class.TriggerMgr()

function TriggerMgr:Init()
	TriggerMgr:SetVar()
	TriggerMgr:RunTrigger()
end

function TriggerMgr:SetVar()
	
	self.temp_12666=0--数字1
	
	self.temp_54887="a"--字符串1
	
end

function TriggerMgr:RunTrigger()
	--默认托管给退出游戏的全局句柄
	local handle=GlobalMgr.instance.ExitHandle
	
	--测试 -> f5重启游戏
	EventMgr_KeyBoardDown.CallBack(function (data)
		if data.key==UnityEngine.KeyCode.F5	then
			GlobalMgr.instance:ReStart(false)
		end
	end,handle)
	
	--清缓存
	EventMgr_GameStartup.CallBack(function (data)
		if true	then
			Resources.UnloadUnusedAssets();
GC.Collect();
		end
	end,handle)
	
	--启动时载入进度
	EventMgr_CanvasInitComplete.CallBack(function (data)
		if true	then
			if SettingModel.instance.CheckUpdate then
        DlgMgr.Inst:Open("ProgressWin")
else
        DlgMgr.Inst:Open("LoginWin")
        --DlgMgr.instance:Open("LoadingWin")
end

		end
	end,handle)
	
	--热更新完成进入登陆界面
	EventMgr_ResUpdateComplete.CallBack(function (data)
		if true	then
			DlgMgr.Inst:Open("LoginWin")
 
		end
	end,handle)
	
	--用户操作 -> 打开关闭加载UI
	EventMgr_DoOpenOrCloseLoadingDlg.CallBack(function (data)
		if true	then
			if data.isOpen then
   DlgMgr.Inst:Open("LoadingWin")
else
   DlgMgr.Inst:Close("LoadingWin")
end
		end
	end,handle)
	
	--用户操作 -> 操作失败时弹出的错误提示
	EventMgr_DoOpenUserOprFail.CallBack(function (data)
		if true	then
			CommonWinMgr.Open(1009)
		end
	end,handle)
	
	--切换到前台时检查更新
	EventMgr_ApplicationPause.CallBack(function (data)
		if (not (data.pause))	then
			FileVersionMgr.instance:CheckUpdateOnce(function (_netResInfo)
    CommonWinMgr.Open(1003,function (btnIndex)
        if btnIndex==0 then
            Application.OpenURL(CustomSetting.Inst.updateUrl.."/".._netResInfo.appFileName)	
        end
    end,nil,_netResInfo.appVersion)
end,function (_netResInfo)
    CommonWinMgr.Open(1011,function (btnIndex)
        if btnIndex==0 then
            GlobalMgr.instance:ReStart(true)
        end
    end)
end)
		end
	end,handle)
	
end

TriggerMgr:Init()