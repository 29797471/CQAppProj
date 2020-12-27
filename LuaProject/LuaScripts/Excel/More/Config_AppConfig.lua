--应用生成配置.xlsx - 应用生成配置
ConfigData_AppConfig=
{
	{
		configName = "正式版",
		updateUrl = "http://dfh5-zy.allrace.com/ServerAssets/test",
		appId = "a.cqqqq.real_time",
		fullRes = false,
		appName = "实时行情",
		developModel = false,
		appIcon = "Assets/Arts/AppIcon/630.png",
		clearPersistentData = false,
	},
	{
		configName = "测试版",
		updateUrl = "http://cq.test",
		appId = "a.cqqqq.real_time_test",
		fullRes = false,
		appName = "实时行情-测试",
		developModel = true,
		appIcon = "Assets/Arts/AppIcon/xx.png",
		clearPersistentData = false,
	},
}
function ConfigData_AppConfig_GetTorsion()
	return table.SerializeValue(ConfigData_AppConfig)
end

Config_AppConfig={}
for i,v in pairs(ConfigData_AppConfig) do
	if Config_AppConfig[v.configName]==nil then
		Config_AppConfig[v.configName]={}
	end
	Config_AppConfig[v.configName][v.updateUrl]=v
end


