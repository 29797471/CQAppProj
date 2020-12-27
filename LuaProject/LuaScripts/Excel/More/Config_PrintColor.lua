--颜色表.xlsx - Color
ConfigData_PrintColor=
{
	{
		name = "Azure",
		desc = "天蓝色",
		value = "#0080FF",
	},
	{
		name = "Pink",
		desc = "粉红色",
		value = "#FFC0CB",
	},
	{
		name = "DeepPink",
		desc = "深粉红色",
		value = "#FF1493",
	},
	{
		name = "Cyan",
		desc = "青色",
		value = "#00FFFF",
	},
	{
		name = "Green",
		desc = "绿色",
		value = "#00FF00",
	},
	{
		name = "Red",
		desc = "红色",
		value = "#FF0000",
	},
	{
		name = "Grey",
		desc = "深灰色",
		value = "#46484b",
	},
}
function ConfigData_PrintColor_GetTorsion()
	return table.SerializeValue(ConfigData_PrintColor)
end

Config_PrintColor={}
for i,v in pairs(ConfigData_PrintColor) do
	Config_PrintColor[v.name]=v
end



local __data=Config_PrintColor
Config_PrintColor=setmetatable({},{__index = function (tbl,key) return __data[key].value end})
