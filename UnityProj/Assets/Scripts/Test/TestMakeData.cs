using System;
using System.Collections;
using UnityCore;
using UnityEngine;

public class TestMakeData : MonoBehaviourExtended
{
    public int year=1982;
    public int month=1;
    public int day=27;

    public int maxAge=120;
    [ContextMenu("Select")]
    public void Select()
    {
        var mat = transform.localToWorldMatrix;
        var xx=mat.GetPosition();
        mat.SetPosition(Vector3.one * 3);
        var yy = mat.GetPosition();

        var a = Vector3.one;
        var b = Vector3.one * 2;
        var c = Vector3.one * 3.5f;

        var birthday = new DateTime(year, month, day);
        var we=TimeUtil.GetBirthdays(birthday, maxAge);
        var wewe= birthday.SolarToChineseLunisolarDate();
        Debug.Log(wewe);
        Debug.Log(Torsion.Serialize(we));
        Debug.Log(Torsion.Serialize(we.ConvertAll(x=>x+year)));
    }
    IEnumerator AA()
    {
        while(true)
        {
            yield return Sleep(1.0f);
            Debug.Log(1);
        }
    }
    System.Collections.Generic.IEnumerator<int> BB()
    {
        Debug.Log("BB");
        int i = Time.frameCount;
        yield return 0;
        Debug.Log(Time.frameCount-i);
    }
    IEnumerator CC()
    {
        Debug.Log("CC");
        yield break;
    }
    [ContextMenu("SelectX")]
    public void SelectX()
    {
        StartCoroutine(AA());
    }
}
