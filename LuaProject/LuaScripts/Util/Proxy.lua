--------------------------------------对属性Set和Get方法的支持--------------
--通过luatable 的元表,来实现设置获取属性时回调方法
Proxy={}

--初始化data数据,使之可以对data的内部属性实现set,get方法,data中不能有外部定义的方法(定义元表后这些方法会失效)
function Proxy.InitProxy(data)
	if data.__proxy~=nil then
		return
	end
	if data.__proxy==nil then
		data.__proxy = {}
	end
	if data.__OnMemberChanged==nil then
		data.__OnMemberChanged={}
	end
	
	for key in pairs(data) do
		if key~="__OnMemberChanged" and key~="__proxy" then
			--print(key)
			if rawget(data,key)~=nil then
				data.__proxy[key]=data[key]
				rawset(data,key,nil)
			end
		end
	end
	
	local mt={
	    __index = function (t,k) return t.__proxy[k] end,
		__newindex = function (t,k,v)
			if t.__proxy[k]~=v then
				local lastV=t.__proxy[k]
				t.__proxy[k]=v
				for i,OnMemberChanged in ipairs(data.__OnMemberChanged) do
					OnMemberChanged(k,v,lastV)
				end
			end
			--print("v="..#data.__PropertyChanged)
		end
	}
	setmetatable(data,mt)
end

-------------------绑定数据结构一个属性到外部处理源,当属性变化时 通知外部源处理数据更新事件---------
function Proxy._AddChanged(data,OnChanged,handle)
	Proxy.InitProxy(data)
	table.SetCallBack(data.__OnMemberChanged,OnChanged,handle)
end

--提供给lua窗口 监听lua表成员改变
--handle托管对象,当托管对象调用Destroy时解除这个属性关联
function Proxy.AddMemberChanged(data,key,OnChanged,handle)
	
	local OnMemberChanged=function (k,newV,lastV) 
		if key==k then
			OnChanged(newV,lastV)
		end
	end
	Proxy._AddChanged(data,OnMemberChanged,handle)
end

-------------主要提供给c#--------------------


-- 监听lua字典成员改变
function Proxy.AddChanged(data,delegate_OnChanged,handle)
	local _OnChanged=function (k,newV,lastV)
		ActionMgr.DoAction_string(delegate_OnChanged,k)
	end
	Proxy._AddChanged(data,_OnChanged,handle)
end

--监听lua列表改变
function Proxy.AddListChanged(ary,delegate_OnChanged)
	local _OnChanged=function (listChangedType,newIndex,oldIndex) 
		ActionMgr.DoAction(delegate_OnChanged,listChangedType,newIndex,oldIndex)
	end
	if ary.__ListChanged==nil then
		ary.__ListChanged={}
	end
	local list=ary.__ListChanged
	table.insertX(list,_OnChanged)
	return function () table.RemoveItemX(list,_OnChanged) end
end

--注入一个c#回调函数到lua字典的一个成员函数对象
function Proxy.SetCallBack(tbl,member,delegate)
	tbl[member]= function (...)
		--print("lua函数调用回调一个c#注入的绑定方法")
		ActionMgr.DoDelegate(delegate,...)
	end
end

