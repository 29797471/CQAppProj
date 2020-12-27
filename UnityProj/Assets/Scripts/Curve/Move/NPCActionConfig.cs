using CqCore;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

public enum CqNPCActionStyle
{
    [EnumLabel("复原")]
    Recovery = 0,
    [EnumLabel("等待")]
    Wait,
    [EnumLabel("速度")]
    Speed,
    [EnumLabel("动作")]
    ChangeAct,
}

[System.Serializable]
public class CqNPCAction
{
    public CqNPCActionStyle style;

    public string value;
}
[System.Serializable]
public class NPCActionGroup
{
    public string name;

    public int Id
    {
        get
        {
            return CustomHash.CRCHash(name);
        }
    }

    public List<CqNPCAction> actions;

}
[CreateAssetMenu]
public class NPCActionConfig : ScriptableObject
{
    [SerializeField]
    public List<NPCActionGroup> groups;

    /// <summary>
    /// 移动动画的基准速度
    /// </summary>
    [TextBox("移动动画的基准速度")]
    public float aniSpeedK = 1f;

    static NPCActionConfig mInst = null;
    public static NPCActionConfig Inst
    {
        get
        {
            if (mInst == null) mInst = Resources.Load<NPCActionConfig>("NPCActionConfig");
            return mInst;
        }
    }
    Dictionary<int, NPCActionGroup> _tempDic;
    public NPCActionGroup GetById(int groupId)
    {
        if (groupId == 0) return null;
        if (_tempDic == null || _tempDic.Count != groups.Count)
        {
            _tempDic = new Dictionary<int, NPCActionGroup>();
            foreach (var it in groups)
            {
                _tempDic[it.Id] = it;
            }
        }
        NPCActionGroup group;
        if (!_tempDic.TryGetValue(groupId, out group))
        {
            group = groups.Find(x => x.Id == groupId);
            if (group != null)
            {
                _tempDic[group.Id] = group;
            }
        }
        return group;
    }

}
