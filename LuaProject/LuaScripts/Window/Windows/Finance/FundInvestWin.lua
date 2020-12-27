super_class.FundInvestWin(LuaWin)

function FundInvestWin:OnInit()
	
end


--窗口显示时
function FundInvestWin:OnShow(item)
	self.dlg.DataContent=item
end

--窗口关闭时
function FundInvestWin:OnHide()
end
