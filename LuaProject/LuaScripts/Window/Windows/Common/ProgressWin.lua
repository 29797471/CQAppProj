super_class.ProgressWin(LuaWin)

function ProgressWin:OnInit()
	self.data=
	{
		percent=0,
		text="",
		updateUrl=CustomSetting.Inst.updateUrl,
	}
	
	self.dlg.DataContent=self.data

	EventMgr_WindowShow.CallBack(function (data) 
		if data.win=="LoginWin" then
			self:Close()
		end
	end,self.destroyHandle)
end


function ProgressWin:OnShow()
	if SettingModel.instance.CheckUpdate then
		self:CheckNet()
	else
		self:Complete()
	end
end

function ProgressWin:CheckNet()
	self.data.text="检查网络环境"
	
	if Application.internetReachability == NetworkReachability.NotReachable then
        CommonWinMgr.Open(1005,function (btnIndex)
			self:Quit()
		end)
	else
		self:CompareApp()
	end
end

--1.下载info.data
--对比客户端版本
function ProgressWin:CompareApp()
	print("CompareApp")
	self.data.text="获取最新版本中"
	self.data.percent=0.1

	FileVersionMgr.instance:CompareInfo(function (result,localResInfo,netResInfo)
		self.data.appVersion=localResInfo.appVersion
		self.data.resVersion=localResInfo.resVersion
		
		self.data.netResInfo=netResInfo
		
		if result==false then
			CommonWinMgr.Open(1010,function (btnIndex)
				if btnIndex==0 then
					self:Complete()
				else
					self:Quit()
				end
			end)
			return
		end
		if localResInfo.appVersion < netResInfo.appVersion then
			printRed("应用程序版本比较低")
			CommonWinMgr.Open(1003,function (btnIndex)
				if btnIndex==0 then--从网络info中读取最新的apk文件名,并下载
					--DlgMgr.Inst:Open("ProgressBarWin")
					--Application.OpenURL(CustomSetting.Inst.appDownloadUrl)
					Application.OpenURL(CustomSetting.Inst.updateUrl.."/"..netResInfo.appFileName)
					self:Quit()
				else
					self:Quit()
				end
			end,nil,netResInfo.appVersion)
		else
			if localResInfo.appVersion > netResInfo.appVersion then
				printRed("应用程序版本比较高")
			else
				printRed("应用程序是最新的")
			end
			
			if localResInfo.resVersion<netResInfo.resVersion then
				printRed("资源比较旧")
				self:CompareRes()
			else
				printRed("资源是最新的")
				self:Complete()
			end
		end
	end)
end

--1.下载md5.dat
--2.对比md5.dat
function ProgressWin:CompareRes()
	print("CompareRes")
	self.data.text="获取最新资源中"
	self.data.percent=0.2
	FileVersionMgr.instance:CompareMd5(function (result)
		
		if result==false then
			CommonWinMgr.Open(1010,function (btnIndex)
				if btnIndex==0 then
					self:Complete()
				else
					self:Quit()
				end
			end)
			return
		end
		local tempSize=0
		local files,totalSize=FileVersionMgr.instance:CheckUpdateFiles(tempSize)
		self.totalCount=files.Count
		self.totalSize=totalSize
		self.data.text="等待更新资源文件(0/"..self.totalCount..")"..StringUtil.FormatBytes(self.totalSize,false)
		self.data.percent=0.2
		
		if files.Count==0 then
			self:Complete()
			return
		end

		if Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork then
			--移动网络,弹出提示下载更新
			print("移动网络,提示更新")
			CommonWinMgr.Open(1004,function (btnIndex)
				if btnIndex==0 then
					self:UpdateFiles(files)
				else
					self:Quit()
				end
			end,nil,self.data.netResInfo.resVersion,self.totalCount,StringUtil.FormatBytes(self.totalSize,false))
		else
			--Wifi
			print("WIFI,直接下载更新资源")
			self:UpdateFiles(files)
		end
	end)
end

function ProgressWin:UpdateFiles(files)
	FileVersionMgr.instance:UpdateFiles(files,function (loadedCount,file,loadedSize)
		self.data.text="等待更新资源文件("..loadedCount.."/"..self.totalCount..")"..StringUtil.FormatBytes(self.totalSize,false).." - ["..file.."]"
		
		self.data.percent=0.2+0.8*loadedSize/self.totalSize
	end,function ()
		--更新完毕后,重启游戏
		GlobalMgr.instance:ReStart(false)
		--[[
		CommonWinMgr.Open(1008,function (btnIndex)
			--self:Quit()
			GlobalMgr.instance:ReStart(false)
		end)]]--
		
		-- self:Complete()
	end)
end

function ProgressWin:Complete()
	self.data.text="资源是最新的"
	self.data.percent=1
	SettingModel.instance.mCheckUpdate=false
	EventMgr_ResUpdateComplete.Notify()
end

function ProgressWin:Quit()
	GlobalMgr.instance:Quit()
end
