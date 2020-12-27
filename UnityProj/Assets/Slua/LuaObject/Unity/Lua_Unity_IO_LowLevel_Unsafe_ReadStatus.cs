using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_ReadStatus : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.ReadStatus");
		addMember(l,0,"Complete");
		addMember(l,1,"InProgress");
		addMember(l,2,"Failed");
		LuaDLL.lua_pop(l, 1);
	}
}
