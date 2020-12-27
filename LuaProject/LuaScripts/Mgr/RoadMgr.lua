RoadMgr={}
function RoadMgr.Init()
	RoadMgr.data=
	{
		roadBuildModel=false,
		add_remove_find=0,
		RoadStyleId=101,
		Remove=function ()
			RoadMgr.data.add_remove_find=1
		end,
		Make4=function ()
			RoadMgr.data.add_remove_find=0
			RoadMgr.data.RoadStyleId=101
		end,
		Make6=function ()
			RoadMgr.data.add_remove_find=0
			RoadMgr.data.RoadStyleId=102
		end
	}
end
RoadMgr.Init()