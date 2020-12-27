super_class.SuitStyleWin(LuaWin)

function SuitStyleWin:OnInit()
	self.data={list={}}
	local a=SuitStyleModel.instance.NodeList
	for i=0,a.Count-1 do
		local temp=a[i]
		table.insert(self.data.list,{name=temp.Name,select=false,Click=function ()
		
		SuitWin:OnData(temp.NodeList)
		end})
	end	
end

function SuitStyleWin:OnShow()
	WinMgr.Open("SuitWin") 
	self.data.list[1].select=true
	self.data.list[1]:Click()
end
function SuitStyleWin:OnHide()
	WinMgr.Close("SuitWin") 
end
