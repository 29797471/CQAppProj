--注意事项:1.所有lua文件使用utf-8 保存,不能用utf-8 BoM
require 'Util/Init'
require 'Event/Init'
require "Excel/Init"
require 'Mgr/Init'
require 'Window/Init'

import "UnityEngine"
if not UnityEngine.GameObject then
	error("Click Make/All to generate lua wrap file")
end


--[[MyGameObject=Slua.Class(GameObject,
	nil,
	{
		AddComponent=function(self,t)
			print "overloaded AddComponent"
			self.__base:AddComponent(t)
		end,
	}	
)
]]--

--lua 来开始建立绑定关系
--c# 对应的组件来和lua发生绑定关系
--回调lua绑定方法,传入属性改变时的处理回调

function main()

	---设置随机种子
    math.randomseed(tostring(os.time()):reverse():sub(1, 6))
	--local t=dofile('module/some')
	--assert(type(t)=='table')
end

function Dispose_main()
	EventDispatcher=nil
end

