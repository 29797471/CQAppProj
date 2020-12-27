super_class.LogoWin(LuaWin)

--等待获取数据
function LogoWin:WaitGetData(OnGetData)
	OnGetData()
end

--窗口资源加载完成时
function LogoWin:OnInit()
end

--窗口显示时
function LogoWin:OnShow()
end

--窗口关闭时
function LogoWin:OnHide()
end
