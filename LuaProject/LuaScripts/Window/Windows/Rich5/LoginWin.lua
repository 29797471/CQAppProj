super_class.LoginWin(LuaWin)

function LoginWin:OnInit()
	self.data=
	{
		updateUrl=CustomSetting.Inst.updateUrl,
		Login=function () 
			DlgMgr.Inst:Open("SettingWin")
		end,
		TestScene=function ()
			DlgMgr.Inst:Open("MainWin")
		end,
		TestAndroid=function ()
			DlgMgr.Inst:Open("TestWin")
		end,
		Money=function ()
			DlgMgr.Inst:Open("FinanceWin")
		end,
		ping=GlobalMgr.instance.pingMs,
	}

	if FileVersionMgr.instance.LocalResInfo~= nil then
		local info=FileVersionMgr.instance.LocalResInfo
		self.data.appV=info.appVersion
		self.data.resV=info.resVersion
	else
		self.data.appV=""
		self.data.resV=""
	end
	self.dlg.DataContent=self.data

	EventMgr_NetPingUpdate.CallBack(function (data)
		self.data.ping=data.time
	end,self.destroyHandle)
end

function LoginWin:OnShow()
	 
end
