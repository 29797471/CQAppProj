using System;
using UnityEngine;

/// <summary>
/// https://www.nowapi.com/?app=intf.appkey
/// </summary>
[Serializable]
[CreateAssetMenu]
public class NowApiConfig : ScriptableObject
{
    public string appKey;
    public string sign;

    static NowApiConfig mInst;
    public static NowApiConfig Inst
    {
        get
        {
            if (mInst == null)
            {
                mInst = Resources.Load<NowApiConfig>("NowApiConfig");
            }
            return mInst;
        }
    }
}
