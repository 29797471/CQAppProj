using CqCore;
using System.Collections;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;
using UnityEngine.UI;

public class MakeCar : MonoBehaviourExtended
{
    public Camera tempCamera;
    public Text tempText;

    [TextBox("生成数量")]
    public int carNum = 30;

    [TextBox("生成间隔")]
    public float makeDelta = 1f;

    [CheckBox("绘制包围盒")]
    public bool drawBounds = true;

    [CheckBox("绘制运动碰撞盒")]
    public bool drawRunBounds = true;

    [CheckBox("绘制车距控制碰撞盒")]
    public bool drawStopBounds = true;

    [CheckBox("绘制路线")]
    public bool drawPath = true;

    int id;

    public bool autoAddCar;
    void Start()
    {
        StartCoroutine(LoopMakeCar_IT());
    }
    [Button("清除生成"), Click("Clear")]
    public string _2;

    CancelHandle removeCars=new CancelHandle();
    public void Clear()
    {
        removeCars.CancelAll();
    }

    public IEnumerator LoopMakeCar_IT()
    {
        var returnObj = new AsyncReturn<GameObject>();
        for (int i = 0; i < carNum; i++)
        {
            while (!autoAddCar) yield return null;
            var prefabName = string.Format("Car/car_{0}", RandomUtil.Random(1, 11));

            yield return ResMgr.Instantiate_IT(prefabName, returnObj, string.Format("{0}({1})", id, prefabName), transform);
            id++;
            var obj = returnObj.data;
            obj.SetActive(false);
            obj.AddComponent<CarPath>().drawPath = drawPath;
            var mc = obj.AddComponent<CarMoveControl>();
            mc.tempText = tempText;
            mc.id = id;
            obj.AddComponent<CarWheelControl>();
            obj.AddComponent<CarAiControl>();
            var col=obj.AddComponent<CarCollider>();
            col.drawBounds = drawBounds;
            col.drawRunBounds = drawRunBounds;
            col.drawStopBounds = drawStopBounds;
            GameObjectUtil.Clone(tempCamera.gameObject, null, obj.transform);
            tempText.text = id.ToString();
            
            obj.SetActive(true);
            removeCars.CancelAct += () => Destroy(obj);
            yield return GlobalCoroutine.Sleep(makeDelta);

        }
    }
}
