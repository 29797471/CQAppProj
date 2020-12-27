super_class.LuaWin()

function LuaWin:Init(dlg)
    self.dlg=dlg
    self.hideHandle=dlg.HideHandle
    self.destroyHandle=dlg.DestroyHandle
    
    self:OnInit()
end

--打开窗口
function LuaWin:Open()
	self.dlg:Open()
end

--关闭窗口
function LuaWin:Close()
	self.dlg:Close()
end

--初始化回调
function LuaWin:OnInit()
end

--显示回调
function LuaWin:OnShow(args)
	--print(self.dlg.Obj.name..":OnShow")
end

--隐藏回调
function LuaWin:OnHide()
    --print(self.dlg.Obj.name..":OnHide")
end

--删除回调
function LuaWin:OnDestroy()
end

function LuaWin:Instantiate(relativeName,OnLoad,newName,parent)
    printRed(relativeName)
    ResMgr.Instantiate(relativeName,OnLoad,newName,parent,self.destroyHandle,true)
end

function LuaWin:AddMemberChanged(tbl,member,fun,handle)
    if handle==nil then
        handle=self.destroyHandle
    end
    Proxy.AddMemberChanged(tbl,member,fun,handle)
end

function LuaWin:LoopCall(seconds,fun,handle)
    if handle==nil then
        handle=self.destroyHandle
    end
    GlobalCoroutine.LoopCall(seconds, fun, handle)
end

function LuaWin:DelayCall(seconds,fun,handle)
    if handle==nil then
        handle=self.destroyHandle
    end
    GlobalCoroutine.DelayCall(seconds, fun, handle)
end