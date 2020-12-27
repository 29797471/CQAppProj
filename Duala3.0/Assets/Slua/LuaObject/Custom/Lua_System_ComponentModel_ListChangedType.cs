using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_ComponentModel_ListChangedType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ListChangedType");
		addMember(l,0,"Reset");
		addMember(l,1,"ItemAdded");
		addMember(l,2,"ItemDeleted");
		addMember(l,3,"ItemMoved");
		addMember(l,4,"ItemChanged");
		addMember(l,5,"PropertyDescriptorAdded");
		addMember(l,6,"PropertyDescriptorDeleted");
		addMember(l,7,"PropertyDescriptorChanged");
		LuaDLL.lua_pop(l, 1);
	}
}
