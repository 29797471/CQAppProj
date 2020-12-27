--窗口.xlsx - 通用弹窗
ConfigData_CommonWin=
{
	{
		id = 1003,
		style = "MessageBox_2",
		contentId = 103,
		btnGroup = {1,5},
	},
	{
		id = 1004,
		style = "MessageBox_2",
		contentId = 104,
		btnGroup = {1,5},
	},
	{
		id = 1005,
		style = "MessageBox_1",
		contentId = 105,
		btnGroup = {5},
	},
	{
		id = 1008,
		style = "MessageBox_1",
		contentId = 108,
		btnGroup = {1},
	},
	{
		id = 1009,
		style = "MessageBox_1",
		contentId = 109,
		btnGroup = {1},
	},
	{
		id = 1010,
		style = "MessageBox_2",
		contentId = 110,
		btnGroup = {1,5},
	},
	{
		id = 1011,
		style = "MessageBox_2",
		contentId = 111,
		btnGroup = {1,2},
	},
	{
		id = 1012,
		style = "MessageBox_1",
		contentId = 112,
		btnGroup = {1},
	},
	{
		id = 2001,
		style = "Float",
		contentId = 100,
		btnGroup = {},
	},
	{
		id = 2002,
		style = "Float",
		contentId = 101,
		btnGroup = {},
	},
}
function ConfigData_CommonWin_GetTorsion()
	return table.SerializeValue(ConfigData_CommonWin)
end

Config_CommonWin={}
for i,v in pairs(ConfigData_CommonWin) do
	Config_CommonWin[v.id]=v
end


