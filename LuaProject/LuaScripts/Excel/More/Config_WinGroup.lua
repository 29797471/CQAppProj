--窗口.xlsx - 分组控制
ConfigData_WinGroup=
{
	{
		group = "Finance",
		wins = {"FinanceWin","ReturnWin"},
		closeOther = true,
	},
	{
		group = "Login",
		wins = {"LoginWin","BackWin"},
		closeOther = true,
	},
	{
		group = "Progress",
		wins = {"ProgressWin"},
		closeOther = false,
	},
	{
		group = "Sport",
		wins = {"SportWin","ReturnWin"},
		closeOther = true,
	},
	{
		group = "TestAndroid",
		wins = {"TestWin","ReturnWin"},
		closeOther = true,
	},
	{
		group = "FundHistory",
		wins = {"FundHistoryWin","ReturnWin"},
		closeOther = false,
	},
}
function ConfigData_WinGroup_GetTorsion()
	return table.SerializeValue(ConfigData_WinGroup)
end

Config_WinGroup={}
for i,v in pairs(ConfigData_WinGroup) do
	Config_WinGroup[v.group]=v
end


