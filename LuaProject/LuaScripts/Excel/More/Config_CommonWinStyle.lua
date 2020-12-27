--窗口.xlsx - 通用窗口样式
ConfigData_CommonWinStyle=
{
	{
		style = "Float",
		win = "FloatWin",
	},
	{
		style = "Float_queue",
		win = "FloatWin",
	},
	{
		style = "MessageBox_0",
		win = "MessageWin",
	},
	{
		style = "MessageBox_1",
		win = "Message1BtnWin",
	},
	{
		style = "MessageBox_2",
		win = "Message2BtnWin",
	},
}
function ConfigData_CommonWinStyle_GetTorsion()
	return table.SerializeValue(ConfigData_CommonWinStyle)
end

Config_CommonWinStyle={}
for i,v in pairs(ConfigData_CommonWinStyle) do
	Config_CommonWinStyle[v.style]=v
end


