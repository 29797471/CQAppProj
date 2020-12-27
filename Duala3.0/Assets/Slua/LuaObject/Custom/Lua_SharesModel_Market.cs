using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SharesModel_Market : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Market");
		addMember(l,0,"USA");
		addMember(l,1,"ShangHai");
		addMember(l,2,"ShenZhen");
		addMember(l,3,"HongKong");
		LuaDLL.lua_pop(l, 1);
	}
}
