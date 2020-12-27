PrefabMgr={}


function PrefabMgr.Init()
	--local list=Resources.FindObjectsOfTypeAll(LuaWindow)
	PrefabMgr.data={}
	local path="Assets/Resources/Window"

	FileOpr.PreorderTraversal(path,function (file)
		local ext=FileOpr.GetNameByExtension(file)
		if ext==".prefab" then
			local shortName=FileOpr.GetNameByShort(file)
			local parent=FileOpr.GetParent(file)
			local path=parent.."\\"..shortName
			local resLoadPath=FileOpr.ToRelativePath(path,"Assets/Resources")
			
			PrefabMgr.data[shortName]=Resources.Load(resLoadPath)
		end
	end,function (folder) end)

	--[[
	for i=1,list.Length do
		local it=list[i]
		PrefabMgr.data[it.name]=it.gameObject
	end
	]]--
	printTbl(PrefabMgr.data)

end

--PrefabMgr.Init()