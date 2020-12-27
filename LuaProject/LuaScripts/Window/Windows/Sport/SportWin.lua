super_class.SportWin(LuaWin)

function SportWin:OnInit()
	self.data=
	{
		left=
		{
			percent=0.5,power=0,average=0,total=0,num=0,
			UpdateData=function (thisData,_power)
				thisData.power=math.floor(_power)
				thisData.percent=_power/100
				
				thisData.total=thisData.total+_power
				thisData.num=thisData.num+1
				thisData.average=math.floor(thisData.total/thisData.num)
			end,
		},
		right=
		{
			percent=0.5,power=0,average=0,total=0,num=0,
			UpdateData=function (thisData,_power)
				thisData.power=math.floor(_power)
				thisData.percent=_power/100
				
				thisData.total=thisData.total+_power
				thisData.num=thisData.num+1
				thisData.average=math.floor(thisData.total/thisData.num)
			end,
		},
	}
	EventMgr_DeviceDataUpdate.AddListener(function (data)

		if data.isLeft then
			self.data.left:UpdateData(data.power)
		else
			self.data.right:UpdateData(data.power)
		end
	end,self.destroyHandle)

	self.dlg.DataContent=self.data
	
end
