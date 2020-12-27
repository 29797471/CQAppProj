--全局常量.csv - 
ConfigData_GlobalConst=
{
	{
		name = "netResPath",
		value = "http://192.168.0.3/ServerAssets/Android",
	},
}
function ConfigData_GlobalConst_GetTorsion()
	return table.SerializeValue(ConfigData_GlobalConst)
end

Config_GlobalConst={}
for i,v in pairs(ConfigData_GlobalConst) do
	Config_GlobalConst[v.name]=v
end



local __data=Config_GlobalConst
Config_GlobalConst=setmetatable({},{__index = function (tbl,key) return __data[key].value end})
