super_class.FundWin(LuaWin)

function FundWin:OnInit()
	
	self.data=
	{
		list={},
		code="",
		Add=function ()
			SaveDataMgr.GetFundData(self.data.code,function (itemData)
				self:AddItem(itemData,true)
			end)
			self.data.code=""
		end,
		Update=function ()
			for i,v in ipairs(self.data.list) do
				self:UpdateItem(v)
			end
		end,
		ClearAll=function ()
			table.Clear(self.data.list)
			table.Clear(self.funds)
			SaveDataMgr.SaveData()
		end,
	}
	
	self.funds=SaveDataMgr.GetFunds()
	--printTbl(self.funds)
	for i,code in ipairs(self.funds) do
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

end


function FundWin:AddItem(item,user_opr)
	item.Del=function ()
		table.RemoveItem(self.data.list,item)
		table.RemoveItem(self.funds,item.code)
		SaveDataMgr.SaveData()
	end
	item.Select=function ()
		--DlgMgr.Inst:Open("FundHistoryWin",item)
	end
	item.History=function ()
		DlgMgr.Inst:Open("FundHistoryWin",item)
	end
	table.insert(self.data.list,item)
	if user_opr then
		table.insert(self.funds,item.code)
		SaveDataMgr.SaveData()
	end
end

function FundWin:UpdateItem(item)
	SaveDataMgr.GetFundData(item.code,function (data)
		item.rise_fall_per=data.rise_fall_per
		item.last_price=data.last_price
		item.name=data.name
	end)
end

--窗口显示时
function FundWin:OnShow()
end

--窗口关闭时
function FundWin:OnHide()
end
