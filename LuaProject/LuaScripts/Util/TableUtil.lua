TableUtil={}

--保证a[b]是一个 table
function table.BeTable(a,b)
	if a[b]==nil then
		a[b]={}
	end
	return a[b]
end

table.insertX=table.insert
table.insert=function (tbl,...)
	local args = { ... }
	if #args==1 then
		local index=#tbl
		table.insertX(tbl,...)
		CallArrayChanged(tbl,ListChangedType.ItemAdded,index,-1)
	elseif #args==2 then
		if args[1]>#tbl+1 then
			printRed("错误:在长度为"..#tbl.."的数组中插入"..args[1])
			return
		end
		table.insertX(tbl,...)
		CallArrayChanged(tbl,ListChangedType.ItemAdded,args[1]-1,-1)
	end
end

--在表格中删除元素,返回删除执行结果
table.removeX=table.remove

--重新定义lua对象的删除函数,可以产生回调
table.remove=function (tbl,index)
	table.removeX(tbl,index)
    CallArrayChanged(tbl,ListChangedType.ItemDeleted,index-1,-1)
end

function table.SetCallBack(tbl,callBack,handle)
	table.insertX(tbl,callBack)
	
	AddDelegate(handle,function ()
		--print("Remove table.SetCallBack")
		table.RemoveItemX(tbl,callBack)
	end)
end

function table.Clear(tbl)
	while #tbl~=0 do
		table.removeX(tbl,1)
	end
	CallArrayChanged(tbl,ListChangedType.Reset,-1,-1)
end

function CallArrayChanged(ary,listChangedType, newIndex,oldIndex)
	--print("newIndex="..newIndex)
	if ary.__ListChanged~=nil then
		--print("__ListChanged "..#ary.__ListChanged)
		for i,OnChanged in ipairs(ary.__ListChanged) do
			OnChanged(listChangedType,newIndex,oldIndex)
		end
	end
end


--在表格中删除元素,返回删除执行结果
function table.RemoveItem(tbl,item)
	local index=table.index(tbl,item)
	if index==0 then
		return false
	end
	table.remove(tbl,index)
    return true
end

--在表格中删除元素,返回删除执行结果
function table.RemoveAll(tbl,matchFun)
	for i=#tbl,1,-1 do
		if matchFun(tbl[i]) then
			table.remove(tbl,i)
		end
	end
end

--在表格中删除元素,返回删除执行结果
function table.RemoveItemX(tbl,item)
	local index=table.index(tbl,item)
	if index==0 then
		return false
	end
	table.removeX(tbl,index)
    return true
end

--返回元素在表中索引
function table.index(tbl,item)
	for i,v in ipairs(tbl) do
		if v==item then
			return i
		end
	end
    return 0
end

--数组元素反序
function table.reverseTable(tab)  
    local tmp = {}  
    for i = 1, #tab do  
        local key = #tab  
        tmp[i] = table.removeX(tab)  
    end  
  
    return tmp  
end  

function table.SetValue(tbl,key,value)
	tbl[key]=value
	--printTbl(tbl)
	--print("table["..key.."]".. "="..tostring(value))
end