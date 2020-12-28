using UnityEngine;

namespace MQTTProto
{
    public static class MQTTConst
    {
        public const string ip = "mq.tongxinmao.com";
        public const int port = 18830;

        public static void RegType()
        {
            AssemblyUtil.RegType(
                typeof(LogMsg_Game), 
                typeof(Heart_Game),
                typeof(Order_Command),
                typeof(ProfilerMsg_Game),
                typeof(HierarchyMsg_Game));
        }
    }
    public class LogMsg_Game
    {
        public string condition;

        public string stackTrace;

        public LogType type;
    }
    public class Heart_Game
    {
        /// <summary>
        /// 时间戳(毫秒)
        /// </summary>
        public long Unix_timestamp_long;
    }
    public class Order_Command
    {
        public string topic;

        /// <summary>
        /// 1.执行脚本命令<para/>
        /// 2.生成内存快照后发送到命令端<para/>
        /// 3.生成Hierarchy树后发送到命令端
        /// </summary>
        public int opr;

        public string command;
    }
    public class ProfilerMsg_Game
    {
        public string state;
        public string data;
    }
    public class HierarchyMsg_Game
    {
        public string state;
        public string data;
    }
}
