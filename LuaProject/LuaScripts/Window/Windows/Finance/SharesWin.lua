super_class.SharesWin(LuaWin)

function SharesWin:OnInit()
	
	self.data=
	{
		list={},
		code="",
		Add=function ()
			SaveDataMgr.GetSharesData(self.data.code,function (itemData)
				self:AddItem(itemData,true)
			end)
			self.data.code=""
		end,
		update=false,
		ClearAll=function ()
			table.Clear(self.data.list)
			table.Clear(self.shares)
			SaveDataMgr.SaveData()
		end,
	}

	
	self:LoopCall(1,function () 
		if self.data.update then
			--print("刷新")
			for i,v in ipairs(self.data.list) do
				self:UpdateItem(v)
			end
		end
	end)

	self.shares=SaveDataMgr.GetShares()
	for i,code in ipairs(self.shares) do
		local item={}
		item.code=code
		self:AddItem(item)
		self:UpdateItem(item)
	end
	--print(table.SerializeValue(self.data))
	
	self.dlg.DataContent=self.data

	EventMgr_KeyBoardDown.CallBack(function (data)
		if data.key==UnityEngine.KeyCode.KeypadEnter or  data.key==UnityEngine.KeyCode.Return	then
			self.data.Add()
		end
	end,self.destroyHandle)

	--LuaGlobal.instance:DelayCall(5,function () print("xxx") end,self)
end



function SharesWin:AddItem(item,user_opr)
	item.Del=function ()
		table.RemoveItem(self.data.list,item)
		table.RemoveItem(self.shares,item.code)
		SaveDataMgr.SaveData()
	end
	item.Select=function ()
		SharesHistoryModel.instance:OpenDownloadUrl(item.code)
	end

	item.History=function ()
		DlgMgr.Inst:Open("SharesHistoryWin",item)
		
	end
	table.insert(self.data.list,item)
	if user_opr then
		table.insert(self.shares,item.code)
		SaveDataMgr.SaveData()
	end

	EventMgr_KeyBoardDown.CallBack(function (data)
		if data.key==UnityEngine.KeyCode.Enter	then
			self.data.Add()
		end
	end,self.destroyHandle)
end

function SharesWin:UpdateItem(item)
	SaveDataMgr.GetSharesData(item.code,function (data)
		item.rise_fall_per=data.rise_fall_per
		item.last_price=data.last_price
		item.market=data.market
		item.name=data.name
	end)
end


--窗口显示时
function SharesWin:OnShow()
end

--窗口关闭时
function SharesWin:OnHide()
end