super_class.FinanceWin(LuaWin)

function FinanceWin:OnInit()
    
	self.data=
	{
        list={},
    }
    self:Add("股票","SharesWin")
    self:Add("基金","FundWin")
    self:Add("其它")
    self.dlg.DataContent=self.data
end

--窗口关闭时
function FinanceWin:Add(t,winName)
    local tempItem=
    {
        select=false,
        name=t,
        winName=winName,
    }
    self:AddMemberChanged(tempItem,"select",function (bl)
        --print("bl"..tostring(bl))
        if bl then
            if tempItem.winName~=nil then
                DlgMgr.Inst:Open(tempItem.winName)
            end
            --self.data.select=tempItem
        else
            if tempItem.winName~=nil then
                DlgMgr.Inst:Close(tempItem.winName)
            end
        end
    end)
    
    table.insert(self.data.list,tempItem)
    return tempItem
end

--窗口关闭时
function FinanceWin:OnHide()
end

function FinanceWin:OnShow()
    self.data.list[1].select=true
    --item.Select()
end