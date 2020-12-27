



-----------本地 -> 服务器时间戳更新------------
---@class EventMgr_ServerTimeUpdate 服务器时间戳更新
EventMgr_ServerTimeUpdate={}

---@param timestamp int
function EventMgr_ServerTimeUpdate.Notify(timestamp,sender )
	EventMgr.ServerTimeUpdate.Notify(timestamp,sender)
end
---@param _action fun(timestamp:int,)
function EventMgr_ServerTimeUpdate.CallBack(_action,handle)
	EventMgr.ServerTimeUpdate.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(timestamp:int,)
function EventMgr_ServerTimeUpdate.CallBackOnce(_action,handle)
	EventMgr.ServerTimeUpdate.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 网络Ping延时变更------------
---@class EventMgr_NetPingUpdate 网络Ping延时变更
EventMgr_NetPingUpdate={}

---@param time int
function EventMgr_NetPingUpdate.Notify(time,sender )
	EventMgr.NetPingUpdate.Notify(time,sender)
end
---@param _action fun(time:int,)
function EventMgr_NetPingUpdate.CallBack(_action,handle)
	EventMgr.NetPingUpdate.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(time:int,)
function EventMgr_NetPingUpdate.CallBackOnce(_action,handle)
	EventMgr.NetPingUpdate.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 多语言 -> 语言切换------------
---@class EventMgr_LanguageChange 语言切换
EventMgr_LanguageChange={}

---@param sysLanguage UnityEngine.SystemLanguage
function EventMgr_LanguageChange.Notify(sysLanguage,sender )
	EventMgr.LanguageChange.Notify(sysLanguage,sender)
end
---@param _action fun(sysLanguage:UnityEngine.SystemLanguage,)
function EventMgr_LanguageChange.CallBack(_action,handle)
	EventMgr.LanguageChange.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(sysLanguage:UnityEngine.SystemLanguage,)
function EventMgr_LanguageChange.CallBackOnce(_action,handle)
	EventMgr.LanguageChange.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 消息 -> 批处理------------
---@class EventMgr_CmdCommand 批处理
EventMgr_CmdCommand={}

---@param command string
function EventMgr_CmdCommand.Notify(command,sender )
	EventMgr.CmdCommand.Notify(command,sender)
end
---@param _action fun(command:string,)
function EventMgr_CmdCommand.CallBack(_action,handle)
	EventMgr.CmdCommand.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(command:string,)
function EventMgr_CmdCommand.CallBackOnce(_action,handle)
	EventMgr.CmdCommand.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 窗口显示------------
---@class EventMgr_WindowShow 窗口显示
EventMgr_WindowShow={}

---@param win string
---@param desc string
function EventMgr_WindowShow.Notify(win,desc,sender )
	EventMgr.WindowShow.Notify(win,desc,sender)
end
---@param _action fun(win:string,desc:string,)
function EventMgr_WindowShow.CallBack(_action,handle)
	EventMgr.WindowShow.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(win:string,desc:string,)
function EventMgr_WindowShow.CallBackOnce(_action,handle)
	EventMgr.WindowShow.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 窗口隐藏------------
---@class EventMgr_WindowHide 窗口隐藏
EventMgr_WindowHide={}

---@param win string
---@param desc string
function EventMgr_WindowHide.Notify(win,desc,sender )
	EventMgr.WindowHide.Notify(win,desc,sender)
end
---@param _action fun(win:string,desc:string,)
function EventMgr_WindowHide.CallBack(_action,handle)
	EventMgr.WindowHide.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(win:string,desc:string,)
function EventMgr_WindowHide.CallBackOnce(_action,handle)
	EventMgr.WindowHide.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 缓动 -> 窗口缓入开始------------
---@class EventMgr_WindowFadeInStart 窗口缓入开始
EventMgr_WindowFadeInStart={}

---@param winName string
function EventMgr_WindowFadeInStart.Notify(winName,sender )
	EventMgr.WindowFadeInStart.Notify(winName,sender)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeInStart.CallBack(_action,handle)
	EventMgr.WindowFadeInStart.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeInStart.CallBackOnce(_action,handle)
	EventMgr.WindowFadeInStart.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 缓动 -> 窗口缓入结束------------
---@class EventMgr_WindowFadeInEnd 窗口缓入结束
EventMgr_WindowFadeInEnd={}

---@param winName string
function EventMgr_WindowFadeInEnd.Notify(winName,sender )
	EventMgr.WindowFadeInEnd.Notify(winName,sender)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeInEnd.CallBack(_action,handle)
	EventMgr.WindowFadeInEnd.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeInEnd.CallBackOnce(_action,handle)
	EventMgr.WindowFadeInEnd.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 缓动 -> 窗口缓出开始------------
---@class EventMgr_WindowFadeOutStart 窗口缓出开始
EventMgr_WindowFadeOutStart={}

---@param winName string
function EventMgr_WindowFadeOutStart.Notify(winName,sender )
	EventMgr.WindowFadeOutStart.Notify(winName,sender)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeOutStart.CallBack(_action,handle)
	EventMgr.WindowFadeOutStart.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeOutStart.CallBackOnce(_action,handle)
	EventMgr.WindowFadeOutStart.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 缓动 -> 窗口缓出结束------------
---@class EventMgr_WindowFadeOutEnd 窗口缓出结束
EventMgr_WindowFadeOutEnd={}

---@param winName string
function EventMgr_WindowFadeOutEnd.Notify(winName,sender )
	EventMgr.WindowFadeOutEnd.Notify(winName,sender)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeOutEnd.CallBack(_action,handle)
	EventMgr.WindowFadeOutEnd.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,)
function EventMgr_WindowFadeOutEnd.CallBackOnce(_action,handle)
	EventMgr.WindowFadeOutEnd.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 按钮点击------------
---@class EventMgr_ButtonClick 按钮点击
EventMgr_ButtonClick={}

---@param winName string
---@param btnPath string
function EventMgr_ButtonClick.Notify(winName,btnPath,sender )
	EventMgr.ButtonClick.Notify(winName,btnPath,sender)
end
---@param _action fun(winName:string,btnPath:string,)
function EventMgr_ButtonClick.CallBack(_action,handle)
	EventMgr.ButtonClick.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,btnPath:string,)
function EventMgr_ButtonClick.CallBackOnce(_action,handle)
	EventMgr.ButtonClick.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 开关点击------------
---@class EventMgr_ToggleClick 开关点击
EventMgr_ToggleClick={}

---@param winName string
---@param btnPath string
---@param select bool
function EventMgr_ToggleClick.Notify(winName,btnPath,select,sender )
	EventMgr.ToggleClick.Notify(winName,btnPath,select,sender)
end
---@param _action fun(winName:string,btnPath:string,select:bool,)
function EventMgr_ToggleClick.CallBack(_action,handle)
	EventMgr.ToggleClick.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,btnPath:string,select:bool,)
function EventMgr_ToggleClick.CallBackOnce(_action,handle)
	EventMgr.ToggleClick.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 模拟开关点击------------
---@class EventMgr_SimulatorToggleClick 模拟开关点击
EventMgr_SimulatorToggleClick={}

---@param winName string
---@param togglePath string
function EventMgr_SimulatorToggleClick.Notify(winName,togglePath,sender )
	EventMgr.SimulatorToggleClick.Notify(winName,togglePath,sender)
end
---@param _action fun(winName:string,togglePath:string,)
function EventMgr_SimulatorToggleClick.CallBack(_action,handle)
	EventMgr.SimulatorToggleClick.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(winName:string,togglePath:string,)
function EventMgr_SimulatorToggleClick.CallBackOnce(_action,handle)
	EventMgr.SimulatorToggleClick.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 通知 -> 执行打开或者关闭加载窗口------------
---@class EventMgr_DoOpenOrCloseLoadingDlg 执行打开或者关闭加载窗口
EventMgr_DoOpenOrCloseLoadingDlg={}

---@param isOpen bool
function EventMgr_DoOpenOrCloseLoadingDlg.Notify(isOpen,sender )
	EventMgr.DoOpenOrCloseLoadingDlg.Notify(isOpen,sender)
end
---@param _action fun(isOpen:bool,)
function EventMgr_DoOpenOrCloseLoadingDlg.CallBack(_action,handle)
	EventMgr.DoOpenOrCloseLoadingDlg.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(isOpen:bool,)
function EventMgr_DoOpenOrCloseLoadingDlg.CallBackOnce(_action,handle)
	EventMgr.DoOpenOrCloseLoadingDlg.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 窗口 -> 通知 -> 执行弹出用户操作失败窗口------------
---@class EventMgr_DoOpenUserOprFail 执行弹出用户操作失败窗口
EventMgr_DoOpenUserOprFail={}

function EventMgr_DoOpenUserOprFail.Notify(sender )
	EventMgr.DoOpenUserOprFail.Notify(sender)
end
---@param _action fun()
function EventMgr_DoOpenUserOprFail.CallBack(_action,handle)
	EventMgr.DoOpenUserOprFail.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_DoOpenUserOprFail.CallBackOnce(_action,handle)
	EventMgr.DoOpenUserOprFail.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 游戏状态 -> 游戏启动------------
---@class EventMgr_GameStartup 游戏启动
EventMgr_GameStartup={}

function EventMgr_GameStartup.Notify(sender )
	EventMgr.GameStartup.Notify(sender)
end
---@param _action fun()
function EventMgr_GameStartup.CallBack(_action,handle)
	EventMgr.GameStartup.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_GameStartup.CallBackOnce(_action,handle)
	EventMgr.GameStartup.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 游戏状态 -> UI画布初始化完成------------
---@class EventMgr_CanvasInitComplete UI画布初始化完成
EventMgr_CanvasInitComplete={}

function EventMgr_CanvasInitComplete.Notify(sender )
	EventMgr.CanvasInitComplete.Notify(sender)
end
---@param _action fun()
function EventMgr_CanvasInitComplete.CallBack(_action,handle)
	EventMgr.CanvasInitComplete.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_CanvasInitComplete.CallBackOnce(_action,handle)
	EventMgr.CanvasInitComplete.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 游戏状态 -> 资源热更新完成------------
---@class EventMgr_ResUpdateComplete 资源热更新完成
EventMgr_ResUpdateComplete={}

function EventMgr_ResUpdateComplete.Notify(sender )
	EventMgr.ResUpdateComplete.Notify(sender)
end
---@param _action fun()
function EventMgr_ResUpdateComplete.CallBack(_action,handle)
	EventMgr.ResUpdateComplete.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_ResUpdateComplete.CallBackOnce(_action,handle)
	EventMgr.ResUpdateComplete.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 游戏状态 -> 初始化服务器信息完成------------
---@class EventMgr_InitServerAddressComplete 初始化服务器信息完成
EventMgr_InitServerAddressComplete={}

function EventMgr_InitServerAddressComplete.Notify(sender )
	EventMgr.InitServerAddressComplete.Notify(sender)
end
---@param _action fun()
function EventMgr_InitServerAddressComplete.CallBack(_action,handle)
	EventMgr.InitServerAddressComplete.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_InitServerAddressComplete.CallBackOnce(_action,handle)
	EventMgr.InitServerAddressComplete.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 游戏状态 -> 登录成功------------
---@class EventMgr_LoginComplete 登录成功
EventMgr_LoginComplete={}

function EventMgr_LoginComplete.Notify(sender )
	EventMgr.LoginComplete.Notify(sender)
end
---@param _action fun()
function EventMgr_LoginComplete.CallBack(_action,handle)
	EventMgr.LoginComplete.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_LoginComplete.CallBackOnce(_action,handle)
	EventMgr.LoginComplete.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 游戏状态 -> 初始化模块------------
---@class EventMgr_InitModel 初始化模块
EventMgr_InitModel={}

function EventMgr_InitModel.Notify(sender )
	EventMgr.InitModel.Notify(sender)
end
---@param _action fun()
function EventMgr_InitModel.CallBack(_action,handle)
	EventMgr.InitModel.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_InitModel.CallBackOnce(_action,handle)
	EventMgr.InitModel.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 加载 -> 开始加载资源------------
---@class EventMgr_ResLoadStart 开始加载资源
EventMgr_ResLoadStart={}

function EventMgr_ResLoadStart.Notify(sender )
	EventMgr.ResLoadStart.Notify(sender)
end
---@param _action fun()
function EventMgr_ResLoadStart.CallBack(_action,handle)
	EventMgr.ResLoadStart.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_ResLoadStart.CallBackOnce(_action,handle)
	EventMgr.ResLoadStart.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 加载 -> 资源加载完成------------
---@class EventMgr_ResLoadEnd 资源加载完成
EventMgr_ResLoadEnd={}

function EventMgr_ResLoadEnd.Notify(sender )
	EventMgr.ResLoadEnd.Notify(sender)
end
---@param _action fun()
function EventMgr_ResLoadEnd.CallBack(_action,handle)
	EventMgr.ResLoadEnd.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_ResLoadEnd.CallBackOnce(_action,handle)
	EventMgr.ResLoadEnd.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------本地 -> 绘制样式改变------------
---@class EventMgr_DrawStyleChanged 绘制样式改变
EventMgr_DrawStyleChanged={}

---@param style RoadDrawStyle
function EventMgr_DrawStyleChanged.Notify(style,sender )
	EventMgr.DrawStyleChanged.Notify(style,sender)
end
---@param _action fun(style:RoadDrawStyle,)
function EventMgr_DrawStyleChanged.CallBack(_action,handle)
	EventMgr.DrawStyleChanged.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(style:RoadDrawStyle,)
function EventMgr_DrawStyleChanged.CallBackOnce(_action,handle)
	EventMgr.DrawStyleChanged.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------设备交互 -> 退出应用------------
---@class EventMgr_ApplicationQuit 退出应用
EventMgr_ApplicationQuit={}

function EventMgr_ApplicationQuit.Notify(sender )
	EventMgr.ApplicationQuit.Notify(sender)
end
---@param _action fun()
function EventMgr_ApplicationQuit.CallBack(_action,handle)
	EventMgr.ApplicationQuit.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun()
function EventMgr_ApplicationQuit.CallBackOnce(_action,handle)
	EventMgr.ApplicationQuit.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------设备交互 -> 应用暂停------------
---@class EventMgr_ApplicationPause 应用暂停
EventMgr_ApplicationPause={}

---@param pause bool
function EventMgr_ApplicationPause.Notify(pause,sender )
	EventMgr.ApplicationPause.Notify(pause,sender)
end
---@param _action fun(pause:bool,)
function EventMgr_ApplicationPause.CallBack(_action,handle)
	EventMgr.ApplicationPause.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(pause:bool,)
function EventMgr_ApplicationPause.CallBackOnce(_action,handle)
	EventMgr.ApplicationPause.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------设备交互 -> Android交互------------
---@class EventMgr_AndroidClick Android交互
EventMgr_AndroidClick={}

---@param style int
function EventMgr_AndroidClick.Notify(style,sender )
	EventMgr.AndroidClick.Notify(style,sender)
end
---@param _action fun(style:int,)
function EventMgr_AndroidClick.CallBack(_action,handle)
	EventMgr.AndroidClick.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(style:int,)
function EventMgr_AndroidClick.CallBackOnce(_action,handle)
	EventMgr.AndroidClick.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------设备交互 -> 键盘按下------------
---@class EventMgr_KeyBoardDown 键盘按下
EventMgr_KeyBoardDown={}

---@param key UnityEngine.KeyCode
function EventMgr_KeyBoardDown.Notify(key,sender )
	EventMgr.KeyBoardDown.Notify(key,sender)
end
---@param _action fun(key:UnityEngine.KeyCode,)
function EventMgr_KeyBoardDown.CallBack(_action,handle)
	EventMgr.KeyBoardDown.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(key:UnityEngine.KeyCode,)
function EventMgr_KeyBoardDown.CallBackOnce(_action,handle)
	EventMgr.KeyBoardDown.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------设备交互 -> 多触点移动------------
---@class EventMgr_MoreInputMove 多触点移动
EventMgr_MoreInputMove={}

---@param delta float
function EventMgr_MoreInputMove.Notify(delta,sender )
	EventMgr.MoreInputMove.Notify(delta,sender)
end
---@param _action fun(delta:float,)
function EventMgr_MoreInputMove.CallBack(_action,handle)
	EventMgr.MoreInputMove.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(delta:float,)
function EventMgr_MoreInputMove.CallBackOnce(_action,handle)
	EventMgr.MoreInputMove.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------设备交互 -> 单触点移动------------
---@class EventMgr_OneInputMove 单触点移动
EventMgr_OneInputMove={}

---@param delta Vector2
function EventMgr_OneInputMove.Notify(delta,sender )
	EventMgr.OneInputMove.Notify(delta,sender)
end
---@param _action fun(delta:Vector2,)
function EventMgr_OneInputMove.CallBack(_action,handle)
	EventMgr.OneInputMove.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(delta:Vector2,)
function EventMgr_OneInputMove.CallBackOnce(_action,handle)
	EventMgr.OneInputMove.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------系统 -> 设备数据更新------------
---@class EventMgr_DeviceDataUpdate 设备数据更新
EventMgr_DeviceDataUpdate={}

---@param isLeft bool
---@param power float
function EventMgr_DeviceDataUpdate.Notify(isLeft,power,sender )
	EventMgr.DeviceDataUpdate.Notify(isLeft,power,sender)
end
---@param _action fun(isLeft:bool,power:float,)
function EventMgr_DeviceDataUpdate.CallBack(_action,handle)
	EventMgr.DeviceDataUpdate.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(isLeft:bool,power:float,)
function EventMgr_DeviceDataUpdate.CallBackOnce(_action,handle)
	EventMgr.DeviceDataUpdate.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------研发工具事件 -> 弹框消息------------
---@class EventMgr_MsgBox 弹框消息
EventMgr_MsgBox={}

---@param content string
---@param title string
---@param style int
function EventMgr_MsgBox.Notify(content,title,style,sender )
	EventMgr.MsgBox.Notify(content,title,style,sender)
end
---@param _action fun(content:string,title:string,style:int,)
function EventMgr_MsgBox.CallBack(_action,handle)
	EventMgr.MsgBox.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(content:string,title:string,style:int,)
function EventMgr_MsgBox.CallBackOnce(_action,handle)
	EventMgr.MsgBox.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------研发工具事件 -> 气泡消息------------
---@class EventMgr_MsgBalloon 气泡消息
EventMgr_MsgBalloon={}

---@param content string
---@param title string
---@param icon int
---@param duration float
function EventMgr_MsgBalloon.Notify(content,title,icon,duration,sender )
	EventMgr.MsgBalloon.Notify(content,title,icon,duration,sender)
end
---@param _action fun(content:string,title:string,icon:int,duration:float,)
function EventMgr_MsgBalloon.CallBack(_action,handle)
	EventMgr.MsgBalloon.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(content:string,title:string,icon:int,duration:float,)
function EventMgr_MsgBalloon.CallBackOnce(_action,handle)
	EventMgr.MsgBalloon.CallBackOnce(function (sender,e) _action(e) end,handle)
end


-----------研发工具事件 -> 打印消息------------
---@class EventMgr_MsgPrint 打印消息
EventMgr_MsgPrint={}

---@param content string
---@param duration float
function EventMgr_MsgPrint.Notify(content,duration,sender )
	EventMgr.MsgPrint.Notify(content,duration,sender)
end
---@param _action fun(content:string,duration:float,)
function EventMgr_MsgPrint.CallBack(_action,handle)
	EventMgr.MsgPrint.CallBack(function (sender,e) _action(e) end,handle)
end
---@param _action fun(content:string,duration:float,)
function EventMgr_MsgPrint.CallBackOnce(_action,handle)
	EventMgr.MsgPrint.CallBackOnce(function (sender,e) _action(e) end,handle)
end

