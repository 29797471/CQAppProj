super_class.TestWin(LuaWin)

function TestWin:OnInit()
	
	self.data=
	{
        percent=0.254758,
        flag=1,
		username="wewewe",
		password="11",
		remember=false,
		version="1.1.111",
		list={{name="a"},{name="b"},{name="cww"}},
		a={b="ffff"},
		Notify=function ()
			print("awef")
			GlobalMgr.instance:PushNotification( 2, "操盘通知", "阿里巴巴可以买入了!");
		end,
		Vibrate=function ()
			GlobalMgr.instance:Vibrate()
		end,
		md5="cqqqq",
	}
	self:AddMemberChanged(self.data,"md5",function (md5)
		GlobalMgr.instance.systemCopyBuffer=GlobalMgr.instance:Calc(md5)
		
		print(GlobalMgr.instance.systemCopyBuffer)
	end)
	self.data.Add=function ()
		table.insert(self.data.list,{name="wewewex"})
		printTbl(self.data.list)
	end
	
	self.data.Clear=function ()
		table.Clear(self.data.list)
	end
	
	self.data.Remove=function ()
		table.remove(self.data.list,2)
	end

	--local lr=self.dlg.Obj:GetComponent("LuaRoot")
	--lr:LinkLuaTable(self.data)

	self.dlg.DataContent=self.data
end

function TestWin:OnShow()
end
