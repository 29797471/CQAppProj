using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManager : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManager");
		createTypeMetatable(l,null, typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManager));
	}
}
