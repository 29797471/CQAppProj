super_class.GroupSelectWin(LuaWin)

function GroupSelectWin:OnInit()
	self.data={group={},list={},Close=function () self:Close() end}

end

function GroupSelectWin:OnData()
	table.Clear(self.data.group)

	local a=IconModel.instance.Group
	for i=0,a.Count-1 do
		local temp=a[i]
		table.insert(self.data.group,{name=temp.mName})
	end
	CallArrayChanged(self.data.group)

	table.Clear(self.data.list)
	local a=IconModel.instance.List
	for i=0,a.Count-1 do
		local temp=a[i]
		table.insert(self.data.list,{name=temp.mName,spr=temp.Spr,Click=function ()
			temp.Select=true
		end})
	end
	CallArrayChanged(self.data.list)
end
