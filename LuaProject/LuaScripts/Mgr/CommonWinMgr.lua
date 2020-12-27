--通用窗口
CommonWinMgr = {}


--flag 单选段落(flag:1,2,3,4...)
function CommonWinMgr.OpenBySingle(messageId,flag,args,callbackFunc)
	CommonWinMgr.ShowMessageBoxById(messageId,GetBitFlag(flag),args,callbackFunc)
end

--各种提示方式
--0.或者空白 文本
--1.漂浮提示(无限制:一般来源于用户点击的提示,点击多少提示多少,彼此不受影响)
--2.漂浮提示(有限制:一般来源于系统提示,彼此之间按时间队列依次出现)
--3.弹出框
--4.弹出确认框
--5.弹出确认取消框
--6.重新请求退出框
--7.更新退出框
--8.错误提示框
--9.漂浮提示栏(替换:当有新的提示的时候会立即替换掉原来的提示)
--10-13到“通用窗口”表查看
--14.漂浮提示栏(无提示音效)(替换:当有新的提示的时候会立即替换掉原来的提示)
--15.只在Editor下弹窗，其他平台则输出到LogError(用于部分服务端错误码)
--flag 位的方式作多选段落(flag:1,2,4,8...)
--支持传入boolean变量了
function CommonWinMgr.Open(messageId,callbackFunc,flag,args)
	if callbackFunc~=nil then
		if type(callbackFunc)~="function" then
			local delegate_callbackFunc=callbackFunc
			callbackFunc=function (index) ActionMgr.DoAction_int(delegate_callbackFunc,index) end
		end
	end
    if type(flag)=="boolean" then
        if flag then
            flag=1
        else
            flag=2
        end
        --printRed("转类型"..flag)
    end
	local tbl=Config_CommonWin[messageId]
	
	local content=TextMgr.GetText(tbl.contentId,flag,args)

	local styleData=Config_CommonWinStyle[tbl.style]
	
	local btnNames={}
	for i,v in ipairs(tbl.btnGroup) do
		btnNames[i]=TextMgr.GetText(v)
	end
	print("提示id:"..messageId.."  内容:"..content)
	DlgMgr.Inst:Open(styleData.win,{callback=callbackFunc,content=content,btns=btnNames})
end

--不作多语言翻译,传文本弹出提示信息
--主要用于研发中部分需要确认的提示
function CommonWinMgr.DebugOpen(content)
	DlgMgr.Inst:Open("Message1BtnWin",{content=content,btns={"好的"}})
end

--不作多语言翻译,漂浮提示信息
--主要用于研发中部分需要确认的提示
function CommonWinMgr.FloatText(content)
	DlgMgr.Inst:Open("FloatWin",{content=content})
end



function CommonWinMgr.Init()
	CommonWinMgr.dic={}
	CommonWinMgr.dic[1]=function (args)
		
	end
end



CommonWinMgr.Init()