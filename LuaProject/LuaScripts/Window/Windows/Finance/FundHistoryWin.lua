super_class.FundHistoryWin(LuaWin)

function FundHistoryWin:OnInit()
	
end


--窗口显示时
function FundHistoryWin:OnShow(item)
	
	if item.historyList==nil then
		SaveDataMgr.GetFundOldData(item.code,function (list)
			item.historyList=list
		end)
	end
--"http://quotes.money.163.com/service/chddata.html?code=0600756&start=20160902&end=20171108&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;VOTURNOVER;"
--		"http://quotes.money.163.com/service/chddata.html?code=0600756&start=20160902&end=20171108&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP"
	
	item.Calc=function ()
		if item.historyList~=nil then
			DlgMgr.Inst:Open("StrategyCalcWin",item)
		end
	end
	self.dlg.DataContent=item
end

--窗口关闭时
function FundHistoryWin:OnHide()
end
