---@class EventDispatcher lua事件
EventDispatcher={_eventListen={}}
----事件管理器(监听,销毁,通知)
--添加事件 xx.Remove=EventManager:AddListener(XX,XX)
--删除事件 xx.Remove()
--once 接收一次事件后自动移除
function EventDispatcher.CallBack(eventId,callBack,handle,once)
	local t=EventDispatcher.GetListenTable(eventId)

	--local callBackTbl={callBack=callBack,once=once}

	table.SetCallBack(t,callBack,handle)
end

function EventDispatcher.GetListenTable(eventId)
	table.BeTable(EventDispatcher._eventListen,eventId)
	return EventDispatcher._eventListen[eventId]
end

function EventDispatcher.Notify(eventId,data)
	local _tbl=EventDispatcher.GetListenTable(eventId)

	for i,callBack in ipairs(_tbl) do
		callBack(data)
	end
end
