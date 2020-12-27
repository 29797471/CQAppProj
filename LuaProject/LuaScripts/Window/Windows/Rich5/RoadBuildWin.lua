super_class.RoadBuildWin(LuaWin)

function RoadBuildWin:OnInit()
	self.dlg.DataContent=RoadMgr.data
end

function RoadBuildWin:OnShow(arg)
	RoadMgr.data.roadBuildModel=true
end

function RoadBuildWin:OnHide()
	RoadMgr.data.roadBuildModel=false
end
