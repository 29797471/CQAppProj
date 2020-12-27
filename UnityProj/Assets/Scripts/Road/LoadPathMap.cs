using System.Collections.Generic;
using UnityCore;
using UnityEngine;

public class LoadPathMap : MonoBehaviour
{
    public string key;
    [Button("保存"), Click("Save")]
    public string _1;

    [Button("读取"), Click("Read")]
    public string _2;

    public void Clear()
    {
        PathMapRule.instance.PathMapInst.ClearAll();
        Save();
    }

    public void Save()
    {
        var list = new List<TestPathV>();
        var inst = PathMapRule.instance.PathMapInst;
        foreach (var pathe in inst.links)
        {
            if (pathe.OneSide)
            {
                list.Add(new TestPathV()
                {
                    from = pathe.from.pos,
                    to = pathe.to.pos,
                    id = pathe.style.id,
                });
            }
        }
        PlayerPrefs.SetString(key, Torsion.Serialize(list));
    }

    public void Read()
    {
        if (!PlayerPrefs.HasKey(key)) return;
        var list = Torsion.TryDeserialize<List<TestPathV>>(PlayerPrefs.GetString(key));
        PathMapRule.instance.ClearAll();
        foreach (var it in list)
        {
            PathMapRule.instance.AddLinkWidthOutMerge(it.from, it.to, it.id);
        }
        PathMapRule.instance.PathMapInst.ReCalcAll();
        PathMapRule.instance.PathMapInst.MakeAll();
    }
    private void Start()
    {
        Read();
    }
}
public class TestPathV
{
    public Vector2 from;
    public Vector2 to;
    public int id;
}

