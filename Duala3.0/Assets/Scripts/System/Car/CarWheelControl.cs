using CqCore;
using System.Collections;
using UnityCore;
using UnityEngine;

public class CarWheelControl : MonoBehaviourExtended
{

    [TextBox("车轮半径")]
    public float WheelR = 0.5f;
    private void OnEnable()
    {
        StartCoroutine(LoopWheels(), DisabledHandle);
    }
    IEnumerator LoopWheels()
    {
        var Wheels = transform.FindAll(x => x.name.Contains("Wheel_")).ConvertAll(x => x.gameObject);
        var fronts = Wheels.FindAll(x => !x.name.Contains("Back"));
        var backs = Wheels.FindAll(x => x.name.Contains("Back"));
        var wheelRot = Vector3.zero;
        var move = GetComponent<CarMoveControl>();
        while (true)
        {
            //车轮转动角度增量
            var angle = (GlobalCoroutine.deltaTime * move.realSpeed) / WheelR * Mathf.Rad2Deg;
            
            wheelRot.x += angle;
            wheelRot.y = move.realDir;
            foreach (var it in fronts)
            {
                it.transform.localEulerAngles = wheelRot;
            }
            wheelRot.y = 0;
            foreach (var it in backs)
            {
                it.transform.localEulerAngles = wheelRot;
            }
            yield return null;
        }
    }
}
