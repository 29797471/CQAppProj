super_class.MainWin(LuaWin)

function MainWin:OnInit()
	
	self.dlg.DataContent=
	{
		Chat=function ()
			DlgMgr.Inst:Open("LoginWin")
		end,
		drag=false,
		gyro=false,
		enabled=true,
		BuildRoad=function()
			DlgMgr.Inst:Open("RoadBuildWin")
		end,
		autoAddCar=false,
	}
	self.data=self.dlg.DataContent
	
	self:AddMemberChanged(self.data,"gyro",function (bl)
		self.data.enabled=not bl
	end)

	self:AddMemberChanged(self.data,"autoAddCar",function (bl)
		RoadMgr.data.autoAddCar=bl
	end)
	self:AddMemberChanged(self.data,"drag",function (bl)
		RoadMgr.data.drag=bl
	end)
	self:Instantiate("Road_City",function (obj)
		local linkObj=obj:GetComponent("LinkObject")
		linkObj.DataContent=RoadMgr.data
--[[
		self:Instantiate("RoadWay",function (obj4)
			self:Instantiate("Car/PathCar",function (obj2)  
				local linkObj2=obj2:GetComponent("LinkObject") 
				linkObj2.DataContent=self.data
				self.data.Play()
				self:Instantiate("Houses",function (obj3)  
				end)
			end)
		end)]]
	end)
end

function MainWin:OnData()
end

function MainWin:OnShow(args)
	Screen.autorotateToLandscapeLeft = true
    Screen.autorotateToLandscapeRight = true
    Screen.autorotateToPortrait = false
	Screen.autorotateToPortraitUpsideDown = false
	Screen.orientation= ScreenOrientation.LandscapeLeft
	Screen.orientation = ScreenOrientation.AutoRotation
end

function MainWin:OnHide()
    Screen.autorotateToLandscapeLeft = false
    Screen.autorotateToLandscapeRight = false
    Screen.autorotateToPortrait = true
	Screen.autorotateToPortraitUpsideDown = true
	Screen.orientation= ScreenOrientation.Portrait
	Screen.orientation = ScreenOrientation.AutoRotation
end
