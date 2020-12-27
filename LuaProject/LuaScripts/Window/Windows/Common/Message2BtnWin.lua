super_class.Message2BtnWin(LuaWin)

function Message2BtnWin:OnInit()
	self.data=
	{
		content="",
		tween=nil,
	}

	self.dlg.DataContent=self.data
end

function Message2BtnWin:OnShow(args)
	self.data.content=args.content
	self.data.btn0=args.btns[1]
	self.data.btn1=args.btns[2]

	self.data.Call0=function ()
		self:Close()
		if args.callback~=nil then
			args.callback(0)
		end
	end
	self.data.Call1=function ()
		self:Close()
		if args.callback~=nil then
			args.callback(1)
		end
	end
end
