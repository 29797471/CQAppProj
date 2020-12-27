using System.Collections.Generic;
using UnityEngine;

namespace TestNetData
{
    public class CRegClient
    {
        public bool isGameClient;
    }
    public class SRegClient
    {
        public uint id;
    }
    public class CSendLog
    {
        public string condition;
        public string stackTrace;
        public UnityEngine.LogType type;
    }
    public class CSendCMD
    {
        public string content;
        public string type;
    }
    public class SSendCMD
    {
        public string content;
        public string type;
    }

    public class SLog
    {
        public uint clientId;
        public string condition;
        public string stackTrace;
        public LogType type;
    }
    public class CGetRegList
    {
    }
    public class SRegList
    {
        public List<uint> list;
    }

    public class CLinkClient
    {
        public uint linkId;
    }
}