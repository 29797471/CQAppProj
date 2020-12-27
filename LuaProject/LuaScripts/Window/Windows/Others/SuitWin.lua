super_class.SuitWin(LuaWin)

function SuitWin:OnInit()
	self.data={list={},select={spr=nil,Next=function ()
		self:Close()
		SuitStyleWin:Close()
		self.select.Select=true
		self.select:Next() 
		WinMgr.Open("ChangeWin")
	end}}
	
end

function SuitWin:OnData(dataList)
	table.Clear(self.data.list)

	for i=0,dataList.Count-1 do
		local temp=dataList[i]
		table.insert(self.data.list,
		{
			name=temp.NameX,
			spr=temp.Spr,
			Click=function ()
				self.data.select.spr=temp.Spr
				self.data.select.name=temp.NameX
				self.select=temp
			end
		})
	end
	CallArrayChanged(self.data.list)

	if #self.data.list>0 then
		self.data.list[1].select=true
		self.data.list[1]:Click()
	end
end
