--委托扩展

--向托管对象中添加委托
function AddDelegate(handle,fun)
    if handle~=nil then
		if type(handle)=="table" then
			table.insert(handle,fun)
		else
			CSharpGlobal.AddCancelAct(handle,fun)
		end
	end
end

--执行托管对象中的委托,并清除
function CancelAll(handle)
    if handle~=nil then
		if type(handle)=="table" then
			for i,act in ipairs(handle) do
                if  type(act)~="function" then
                    ActionMgr.DoAction(act)
                else
                    act()
                end
            end
            table.clear(handle)
		else
			handle:CancelAll()
		end
	end
end
