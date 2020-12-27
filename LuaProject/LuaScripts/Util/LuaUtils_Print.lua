
--内置函数
--print
--printerror
--warn

--有颜色的打印
function printColor(color,...)
    local nodeStart="<color="..color..">"
    local nodeEnd="</color>"
    local arg = { ... }
    local msg = table.concat(arg, "\t")
    local ary=string.split(msg,"\n")
    msg=nil
    for i,v in ipairs(ary) do
        if msg==nil then
            msg=nodeStart..v..nodeEnd
        else
            msg=msg.."\n"..nodeStart..v..nodeEnd
        end
    end
    print(msg)
end
