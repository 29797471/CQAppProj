
EventEnum = CreatEnumTable(
{
    "NetStateChange",--网络 -> 网络状态变化
},1001)


-----------网络 -> 网络状态变化------------
---@class EventMgr_NetStateChange 网络状态变化
EventMgr_NetStateChange={}

function EventMgr_NetStateChange.Notify(sender )
	local _tbl={}
	EventDispatcher.Notify(EventEnum.NetStateChange,_tbl)
end
---@param _action fun()
function EventMgr_NetStateChange.CallBack(_action,handle)
	EventDispatcher.CallBack(EventEnum.NetStateChange,_action,handle)
end
---@param _action fun()
function EventMgr_NetStateChange.CallBackOnce(_action,handle)
	EventDispatcher.CallBack(EventEnum.NetStateChange,_action,handle,true)
end

