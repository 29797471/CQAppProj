super_class.ProgressBarWin(LuaWin)

function ProgressBarWin:OnInit()

	local handle=CancelHandle()

	local appDownloadUrl=CustomSetting.Inst.appDownloadUrl
	local fileName=FileOpr.GetFileName(appDownloadUrl)
	local localFile=Application.persistentDataPath .. "/" .. fileName
	self.data=
	{
		progress=0,
		content="正在下载:"..fileName,
		Cancel=function ()
			self:Close()
			handle:CancelAll()
		end,
	}
	
	self.dlg.DataContent=self.data
	
	UnityHttpUtil.DownloadFile(appDownloadUrl,localFile,
		   function (bl)
			 self:Close()
			 print("打开"..localFile)
			 Application.OpenURL(localFile)--无效,不知道怎么打开
			 --AndroidUtil.InstallApk(localFile) 
			 Application.OpenURL("http://www.baidu.com")
			 Application.OpenURL("weixin://")
			 
		 end,
		 function (p) 
			self.data.progress=p 
		end,handle)
	--[[ UnityWebRequestUtil.DownloadFile(appDownloadUrl,localFile,
		   function (bl)
			 self:Close()
			 AndroidUtil.InstallApk(localFile)
		 end,
           function (p) print(p) self.data.progress=p end,handle) ]]
end


function ProgressBarWin:OnShow()
end
