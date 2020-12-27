local somemodule = require 'module.some'
import "UnityEngine"
if not UnityEngine.GameObject then
	error("Click Make/All to generate lua wrap file")
end


MyGameObject=Slua.Class(GameObject,
	nil,
	{
		AddComponent=function(self,t)
			print "overloaded AddComponent"
			self.__base:AddComponent(t)
		end,
	}	
)

function main()
	--local t=dofile('module/some')
	--assert(type(t)=='table')

	local go = GameObject.Find("Canvas/Button")
	local btn = go:GetComponent("Button")

	local btext = GameObject.Find("Canvas/Button/Text")
	local tt = btext:GetComponent(UI.Text)
	tt.text="test"
	
	local ab={}
	
	local mt = {}
	mt.__index = function(t, k) print("call index function") end
	mt.__newindex = function(t,k,v) print("call new index function") end
	setmetatable(ab, mt)

	ab.b=3
	btn.onClick:AddListener(function () print("abc") end)
end

