using UnityEngine;

public class DlgLayer
{
    Canvas MainCanvas
    {
        get
        {
            return DlgLayerMgr.Inst.MainCanvas;
        }
    }

    GameObject mObj;
    public GameObject Obj
    {
        get
        {
            return mObj;
        }
    }
    public void CreateObj(Transform canvas,int index)
    {
        mObj = new GameObject(data.layer);
        mObj.transform.SetParent(canvas);
        mObj.AddComponent<RectTransform>().ReSet();
        mObj.transform.SetSiblingIndex(index);
        mObj.SetActive(true);
    }
    Config_WinLayer.DataItem data;
    public int Priority
    {
        get
        {
            return data.priority;
        }
    }
    public DlgLayer(string id)
    {
        data= Config_WinLayer.Dic[id];
        if(data==null)
        {
            Debug.LogError(string.Format("没有找到这个窗口分组({0})",id));
        }
    }
}
