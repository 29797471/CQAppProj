﻿
using System;
using System.Collections.Generic;
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal void Lua_System_EventHandler_1_EventMgr_DeviceDataUpdate__EventArgs(LuaFunction ld ,System.Object a1,EventMgr.DeviceDataUpdate._EventArgs a2) {
            IntPtr l = ld.L;
            int error = pushTry(l);

			pushValue(l,a1);
			pushValue(l,a2);
			ld.pcall(2, error);
			LuaDLL.lua_settop(l, error-1);
		}
	}
}
