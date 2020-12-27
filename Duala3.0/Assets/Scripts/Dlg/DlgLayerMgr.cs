using CqCore;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口显示层级分组管理
/// </summary>
public class DlgLayerMgr :SystemMgr<DlgLayerMgr>
{
    public Canvas MainCanvas { get; private set; }
    public override void Dispose()
    {
        if(MainCanvas!=null)
        {
            Object.Destroy(MainCanvas.gameObject);
        }
        base.Dispose();
    }
    Dictionary<string, DlgLayer> layerDic;
    List<DlgLayer> layerList;
    public DlgLayer GetLayer(string id)
    {
        if(layerDic==null)
        {
            layerDic = new Dictionary<string, DlgLayer>();
            layerList = new List<DlgLayer>();
        }
        if(!layerDic.ContainsKey(id))
        {
            var dlgLayer= new DlgLayer(id);
            layerList.Add(dlgLayer);
            layerList.Sort(x => x.Priority);
            dlgLayer.CreateObj(MainCanvas.transform, layerList.IndexOf(dlgLayer));
            layerDic[id] = dlgLayer;
        }
        return layerDic[id];
    }

    public void Init()
    {
        if (MainCanvas == null)
        {
            ResMgr.LoadAsset(CustomSetting.Inst.uiRootPath, (System.Action<GameObject>)(prefab =>
            {
                var obj = Object.Instantiate(prefab);
                obj.name = prefab.name;
                MainCanvas = obj.GetComponentInChildren<Canvas>();
                EventMgr.CanvasInitComplete.Notify();
            }), GlobalMgr.instance.ExitHandle);
        }
    }
}
