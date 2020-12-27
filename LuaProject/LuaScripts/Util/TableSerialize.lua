--提供LuaTable 转json的序列化
local controlStr = "\n"

--表达式分隔符
local ExpressionSeparator = "="

--表达式结束符
local ExpressionEnd = ";"

function TabIn()
    controlStr=controlStr.."\t"
end

function TabOut()
    controlStr=string.sub(controlStr,1,-2)
end


--打印列表tbl所有数据
function table.SerializeList(data)
    local args=controlStr.."["
    TabIn()
    for i,value in ipairs(data) do
        args=args..controlStr..table.SerializeValue(value)
        if i~=#data then
            args=args..","
        end
	end
    TabOut()
    args=args..controlStr.."]"
    return args
end
--打印字典tbl所有数据
function table.SerializeObj(data)
    local args=controlStr.."{"
    TabIn()
    for key,value in pairs(data) do
		args=args..controlStr..key..ExpressionSeparator..table.SerializeValue(value)
        if i~=#data then
            args=args..ExpressionEnd
        end
	end
    TabOut()
    args=args..controlStr.."}"
    return args
end

--任何数据类型的字符串转换方式
function table.SerializeValue(t)
    if type(t) == "string"  then
        return '"'..t..'"'
    elseif type(t) == "number" then
        return tostring(t)
    elseif  type(t) == "table" then
        if isList(t) then
            return table.SerializeList(t)
        else
            return table.SerializeObj(t)
        end
    else
        return tostring(t)
    end
end

function isList(tbl)
	local n = #tbl 
	local j=1
	for i,v in pairs(tbl) do  
        if type(i) ~= "number" then  
            return false
        end  
		if i~=j or i>n then
			return false
		end
		j=j+1
	end
	return true
end