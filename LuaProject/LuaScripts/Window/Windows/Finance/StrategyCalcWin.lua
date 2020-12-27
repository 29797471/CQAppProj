super_class.StrategyCalcWin(LuaWin)

function StrategyCalcWin:OnInit()
	self.data=
	{
		name="",
		code="",
		short=false,
	}
	if SaveDataMgr.data.calcInfo==nil then
		SaveDataMgr.data.calcInfo={}
	end
	if SaveDataMgr.data.conUpDownK==nil then
		SaveDataMgr.data.timeK=0.3

		SaveDataMgr.data.conUpDownK=0.01
		SaveDataMgr.data.assetK=0.38

		SaveDataMgr.data.lostK=0.0353
		SaveDataMgr.data.newLostK=0.01
		SaveDataMgr.data.winK=0.0518
	end
	
	self.data.timeK=SaveDataMgr.data.timeK

	self.data.conUpDownK=SaveDataMgr.data.conUpDownK
	self.data.conUpDownStr=""..self.data.conUpDownK

	self.data.assetK=SaveDataMgr.data.assetK
	self.data.assetStr=""..self.data.assetK

	self.data.lostK=SaveDataMgr.data.lostK
	self.data.lostStr=""..self.data.lostK
	
	self.data.newLostK=SaveDataMgr.data.newLostK
	self.data.newLostStr=""..self.data.newLostK

	self.data.winK=SaveDataMgr.data.winK
	self.data.winStr=""..self.data.winK

	

	self:AddMemberChanged(self.data,"conUpDownK",function (v)
		self.data.Calc()
	end)
	self:AddMemberChanged(self.data,"lostK",function (v)
		self.data.Calc()
	end)
	self:AddMemberChanged(self.data,"newLostK",function (v)
		self.data.Calc()
	end)
	self:AddMemberChanged(self.data,"winK",function (v)
		self.data.Calc()
	end)
	self:AddMemberChanged(self.data,"assetK",function (v)
		self.data.Calc()
	end)
	self:AddMemberChanged(self.data,"timeK",function (v)
		local list=self.item.historyList
		local i=math.ceil(#list*v)
		if i==0 then
			i=1
		end
		self.data.time=list[i].time
		self.data.Calc()
	end)
    self.dlg.DataContent=self.data
end

--窗口显示时
function StrategyCalcWin:OnShow(item)
	self.data.name=item.name
	self.data.code=item.code
	self.item=item
	local CalcResult=function (recordUnsold)
		local list=item.historyList
		
		local startIndex=math.ceil(#list*self.data.timeK)
		if startIndex==0 then
			startIndex=1
		end
		self.data.time=list[startIndex].time

		return BuySellMgr.Calc(list,startIndex,self.data,recordUnsold)
		--self.data.conUpDownStr=""..(self.data.conUpDownK+0.0001)
		
	end
	self.data.Calc=function ()
		CalcResult(true)
		--self.data.conUpDownStr=""..(self.data.conUpDownK+0.0001)
		
	end
	self.data.Save=function ()
		local tbl={}

		tbl.conUpDownK=self.data.conUpDownK

		tbl.lostK=self.data.lostK
	
		tbl.newLostK=self.data.newLostK

		tbl.winK=self.data.winK

		tbl.assetK=self.data.assetK

		tbl.timeK=self.data.timeK
		SaveDataMgr.data.
		SaveDataMgr.SaveData()
	end

	self.data.OpenInfo=function ()
		DlgMgr.Inst:Open("FundInvestWin",CalcResult(true))
	end
	self.data.OpenTradeInfo=function ()
		DlgMgr.Inst:Open("FundInvestWin",CalcResult(false))
	end

	self.data.Calc()
	--self:LoopCall(0.1,self.data.Calc)
end

--窗口关闭时
function StrategyCalcWin:OnHide()
end