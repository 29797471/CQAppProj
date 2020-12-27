--cq
function string.split(str, delimiter)
	if str==nil or str=='' or delimiter==nil then
		return nil
	end
	
    local result = {}
    for match in (str..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match)
    end
    return result
end

--cq
function string.refind(str,pattern,init,plain)
    local rev = string.reverse(str)
    local i=string.find(rev,pattern,init,plain)
    if i==nil then
        return nil
    end
    return string.len(rev)-i+1
end

-----Cq
function printByStack(...)
    local s=...
	for level = 2, math.huge do
		local t = debug.getinfo(level,"Sln")

	    if not t then 
            break
        end

	    s=table.SerializeValue(s)..string.format("\r\n%s   %s%s",tostring(t.name),t.short_src,t.currentline )
	end
	print(s)
end

function printTbl(a)
	print(table.SerializeValue(a))
end

function printRed(...)
    local arg = { ... }
    local msg = table.concat(arg,"\t")
    print("<color=#ff0000>"..msg.."</color>")
end

function printTorsion(x)
    print(Torsion.Serialize(x,true,false))
end