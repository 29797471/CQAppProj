super_class.TextMgr()

TextMgr.ConvertArg={} 

--当文本参数未定义时用原始配置文本显示;反之,空文本显示
TextMgr.useArgName=not GlobalMgr.instance.isMobilePlatform

TextMgr.textList={}--文本控件列表

require 'Mgr/TextMgr_Convert'

--获取相应id的提示文本
--flag -1:表示默认值,不处理摘括号 0:表示去除所有括号 其它按位来保留括号内容
function TextMgr.GetText(id,flag,args)
	if id==0 then
		return "翻译id尚未配置"
	end
	
	local GetText=TextMgr.data[id]
	if GetText==nil then
		printByStack("文本转译id="..id.."不存在")

		return "文本转译id("..id..")不存在"
	end
	--print("id="..id)
	local txt = GetText(args)
	
	--print("txt= "..txt)
    if type(flag)=="boolean" then
        if flag then
            flag=1
        else
            flag=2
        end
        --printRed("转类型"..flag)
    end
	if not TextMgr.useArgName and flag==-1 then
		flag=0
	end
	if flag~=-1 then--基于位标记来去除大括号
		--print("flag="..flag)
		local i=1
		txt=string.gsub(txt, "{.-}", function(s)
			if StateCheck(flag,i) then
				s=string.sub(s,2,#s-1)
			else
				s=""
			end
			i=i*2
			return s
		end)
	end

	return txt
end

function TextMgr.Init()

	TextMgr.ToConvertArg()

	TextMgr.ReLoadLanguage(SettingModel.instance.LanguageName)

	if EventMgr_LanguageChange~=nil then
		EventMgr_LanguageChange.CallBack(function (eventData)
			TextMgr.ReLoadLanguage(SettingModel.instance.LanguageName)
		end)
	end
end

--生成对应的转换函数
function TextMgr.ToConvertArg()
    TextMgr.Convert={}
    for exp,v in pairs(TextMgr.ConvertArg) do
        TextMgr.Convert[exp]=function (args,i)
			if args==nil or args[i]==nil then
                if TextMgr.useArgName then
                    return "%"..exp..(i-1).."%"
                else
                    return ""
                end
            else
                return TextMgr.ConvertArg[exp](args[i])
            end
        end
    end
end

function TextMgr.ReLoadLanguage(language)
	LanguageMgr.instance:UpdateTimeAndNumberByLanguage()
    
	if language=="ChineseSimplified" then
		language="Chinese"
	end
	
	TextMgr.data=dofile("Excel/Language/"..language)

	for i,languageText in ipairs(TextMgr.textList) do
		languageText:UpdateText()
	end
	--加载语言包完成
	
end

--添加文本控件到语言改变通知列表,当文本控件销毁时自动移除
function TextMgr.SetCallBack(languageText)
	languageText:UpdateText()
	table.SetCallBack(TextMgr.textList,languageText,languageText.DestroyHandle)
end
	
TextMgr.Init()

