--窗口.xlsx - 显示层级表
ConfigData_WinLayer=
{
	{
		layer = "User",
		priority = 3,
	},
	{
		layer = "Full",
		priority = 5,
	},
	{
		layer = "System",
		priority = 7,
	},
	{
		layer = "SceneUI",
		priority = 15,
	},
	{
		layer = "Guide",
		priority = 20,
	},
	{
		layer = "Tips",
		priority = 25,
	},
	{
		layer = "Suspended",
		priority = 30,
	},
	{
		layer = "Loading",
		priority = 40,
	},
}
function ConfigData_WinLayer_GetTorsion()
	return table.SerializeValue(ConfigData_WinLayer)
end

Config_WinLayer={}
for i,v in pairs(ConfigData_WinLayer) do
	Config_WinLayer[v.layer]=v
end


