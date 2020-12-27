------------文本变量类型转换函数--------

--10	文本参数直接返回
TextMgr.ConvertArg["text"]=function (a) 
	return a 
end


--13	获取翻译id对应的文本(该文本中不再含变量)
TextMgr.ConvertArg["id"]=function (a)
	if(type(a) == "string") then 
		a = tonumber(a)
	end
	return TextMgr.GetText(a)
end

--12
TextMgr.ConvertArg["num"]=function (a)
	return a 
end

--21: 传入数字,保留4位
TextMgr.ConvertArg["keep_number"]=function (a) return  MathUtil.KeepNumber(a,4) end

--14	保留2位有效数字
TextMgr.ConvertArg["float"]=function (num)
	return LanguageMgr.instance:ToNumber(num,1)
end

--11  0~1的小数转百分比显示(保留2位小数)
TextMgr.ConvertArg["percent"]=function (num)
	return (math.floor(num*10000)/100).."%" --string.format("%0.2f",num).."%"
end

--20	0~1的小数转百分比显示(保留2位小数),小于0为绿色百分比,大于0为带+的红色百分比
TextMgr.ConvertArg["percent_color"]=function (num)
	if num >0 then
		return string.format("<color=red>+%0.2f%%</color>",math.floor(num*10000)/100)
	else
		return string.format("<color=green>%0.2f%%</color>",math.floor(num*10000)/100)
	end
end

--15	传入时间戳(秒)
TextMgr.ConvertArg["time"]=function (a)return TimeUtil.GetTimeStringBySecond(a,"yyyy/MM/dd HH:mm:ss") end
--传入时间戳(秒)
TextMgr.ConvertArg["time_ymd"]=function (a)return TimeUtil.GetTimeStringBySecond(a,"yyyy/MM/dd") end
--传入时间戳(秒)
TextMgr.ConvertArg["time_hm"]=function (a)return TimeUtil.GetTimeStringBySecond(a,"HH:mm") end

--16	传入时段(秒)
TextMgr.ConvertArg["period"]=function (a)return TimeUtil.TimeSpanFormat(a) end


--网络延时
TextMgr.ConvertArg["net_delay"]=function (a)
	if a<100 then
		return "<color=green>"..a.."ms</color>"
	elseif a<200 then
		return "<color=yellow>"..a.."ms</color>"
	else
		return "<color=red>"..a.."ms</color>"
	end
end

--float   传入:一个浮点数或者一个大数,
		-->显示:一个数字的货币表现形式(xx.xx万或者xx.xx亿,保留6位有效数字,)

--time 传入:Unix时间戳(Java无时区的时刻,单位秒)
		--> 显示:某个时候 xxxx/xx/xx xx:xx:xx(yyyy/MM/dd HH:mm:ss) (本地时区的时刻)

--time_ymd 传入:Unix时间戳(Java无时区的时刻,单位秒)
--> 显示:某个时候 xxxx/xx/xx(yyyy/MM/dd) (本地时区的日期)

--time_hm 传入:Unix时间戳
		--> 显示:某个时候 xx:xx(HH:mm) (本地时区的时刻)

--period 传入:Unix时间戳(到期时刻)
		-->显示: 一个倒计时文本 xx时xx分 或者(xx分xx秒)  (到目标时刻剩余的时段) 相关组件加上UpdateTextPerSecond来每秒驱动更新到期剩余时间

--periodhms 传入:Unix时间戳(到期时刻)
		-->显示: 一个倒计时文本 xx:xx:xx  (到目标时刻剩余的时段) 相关组件加上UpdateTextPerSecond来每秒驱动更新到期剩余时间
--periodhmsRev 传入unix时间戳(开始计时时刻)
		-->显示:一个计时器文本 xx:xx:xx (计时开始到现在的时间)相关组件加上UpdateTextPerSecond来每秒驱动更新到期剩余时间

--periodx 传入:时段(描述一个固定的时段)
		-->显示:一个固定的时间长度 xx:xx:xx

