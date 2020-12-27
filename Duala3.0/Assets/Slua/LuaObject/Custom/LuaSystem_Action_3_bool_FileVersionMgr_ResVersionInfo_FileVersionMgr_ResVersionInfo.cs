
using System;
using System.Collections.Generic;
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal void Lua_System_Action_3_bool_FileVersionMgr_ResVersionInfo_FileVersionMgr_ResVersionInfo(LuaFunction ld ,bool a1,FileVersionMgr.ResVersionInfo a2,FileVersionMgr.ResVersionInfo a3) {
            IntPtr l = ld.L;
            int error = pushTry(l);

			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			ld.pcall(3, error);
			LuaDLL.lua_settop(l, error-1);
		}
	}
}
