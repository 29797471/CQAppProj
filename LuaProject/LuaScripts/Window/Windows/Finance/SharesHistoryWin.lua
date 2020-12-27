super_class.SharesHistoryWin(LuaWin)

function SharesHistoryWin:OnInit()
	
end


--窗口显示时
function SharesHistoryWin:OnShow(item)
	
	if item.historyList==nil then
		SaveDataMgr.GetSharesHistory(item.code,function (list)
			item.historyList=list
		end)
	end
	self.dlg.DataContent=item
end

--窗口关闭时
function SharesHistoryWin:OnHide()
end
