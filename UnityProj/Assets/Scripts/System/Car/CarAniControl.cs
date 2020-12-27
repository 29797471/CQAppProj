using CqCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAniControl : MonoBehaviour
{
    public Animator npc;

    AnimationClip GetClip(string clipName)
    {
        if (dic == null)
        {
            var ac = npc.runtimeAnimatorController;
            dic = new Dictionary<string, AnimationClip>();
            foreach (var it in ac.animationClips)
            {
                dic[it.name] = it;
            }
        }
        AnimationClip _clip = null;
        dic.TryGetValue(clipName, out _clip);
        return _clip;
    }
    Dictionary<string, AnimationClip> dic;


    AnimationClip clip;

    CarMoveControl move;
    public float speed
    {
        get
        {
            if (move == null) move = GetComponent<CarMoveControl>();
            return move.realSpeed;
        }
        set
        {
            if (move == null) move = GetComponent<CarMoveControl>();
            move.realSpeed = value;
        }
    }
    IEnumerator LoopAni()
    {
        var t = 0f;
        while (true)
        {
            if (clip != null) clip.SampleAnimation(npc.gameObject, t);
            yield return null;
            if (speed == 0)
            {
                t += GlobalCoroutine.deltaTime;
            }
            else t += GlobalCoroutine.deltaTime * speed / NPCActionConfig.Inst.aniSpeedK;
            if (clip != null) t %= clip.length;
        }
    }
    IEnumerator DoActionGroup(int groupId)
    {
        var group = NPCActionConfig.Inst.GetById(groupId);
        if (group == null) yield break;
        var lastSpeed = speed;
        var lastClip = clip;
        for (int i = 0; i < group.actions.Count; i++)
        {
            var act = group.actions[i];
            switch (act.style)
            {
                case CqNPCActionStyle.Recovery:
                    {
                        speed = lastSpeed;
                        clip = lastClip;
                    }
                    break;
                case CqNPCActionStyle.Wait:
                    {
                        yield return float.Parse(act.value);
                    }
                    break;
                case CqNPCActionStyle.Speed:
                    {
                        speed = float.Parse(act.value);
                    }
                    break;
                case CqNPCActionStyle.ChangeAct:
                    {
                        var temp = GetClip(act.value);
                        if (temp != null)
                        {
                            clip = temp;
                        }
                        else
                        {
                            Debug.LogError("找不到这个动画" + act.value);
                        }
                        //var y = Ani[act.value];
                        //var x = y.clip;
                    }
                    break;
            }
        }
    }

}
