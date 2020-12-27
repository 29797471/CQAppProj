using CqCore;
using UnityEditor;

namespace Automation
{
    public static class MakeAll
    {
        [MenuItem("Assets/生成代码")]
        public static void MakeCode()
        {
            if (EditorUtility.DisplayDialog("提示", "生成代码?", "确定", "取消"))
            {
                ProcessUtil.Start(@"..\make_code", null, true);
                //EditorPrefs.DeleteKey("SLUA_REMIND_GENERTE_LUA_INTERFACE");
                //LuaCodeGen.ClearALL();
            }
        }
        
    }
}
