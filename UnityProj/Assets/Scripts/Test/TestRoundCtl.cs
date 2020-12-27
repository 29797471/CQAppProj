using UnityCore;
using UnityEngine;
using UnityEngine.UI;

public class TestRoundCtl : MonoBehaviour
{
    [Button("初始化n个"),Click("Init")]
    public string _1;

    public int num = 3;
    public void Init()
    {
        var rs = GetComponent<ScrollGrid>();
        rs.UpdateData = (obj, index) =>
        {
            obj.GetComponentInChildren<Text>().text = index.ToString();
        };
        rs.DataCount = num;
    }
    private void Start()
    {
        Init();
    }
}
