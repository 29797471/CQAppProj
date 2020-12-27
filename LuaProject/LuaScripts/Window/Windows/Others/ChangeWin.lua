super_class.ChangeWin(LuaWin)

function ChangeWin:OnInit()
	self.data={list={}}

	local a=ChangeModel.instance.mNodeList
	for i=0,a.Count-1 do
		local temp=a[i]
		table.insert(self.data.list,{name=temp.mName,Click=function ()
			temp.Select=true
			WinMgr.Open("GroupSelectWin")
			GroupSelectWin:OnData()
		end})
	end
end

function ChangeWin:OnData()
end