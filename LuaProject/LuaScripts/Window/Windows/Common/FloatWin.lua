super_class.FloatWin(LuaWin)

function FloatWin:OnInit()
	self.data=
	{
		content="",
		tween=nil,
	}
	
	self.dlg.DataContent=self.data
end

function FloatWin:OnShow(args)
	self.data.content=args.content
	--self.data.Play()
end
