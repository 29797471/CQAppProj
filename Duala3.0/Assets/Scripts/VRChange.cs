using UnityCore;
using UnityEngine;

public class VRChange : MonoBehaviourExtended
{
    public bool vr_3d;
    public GameObject vr_camrea;
    public GameObject nor_camrea;

    [Button("3D切换"),Click("Change")]
    public string _1;

    [Button("重置正方向"), Click("ResetDir")]
    public string _2;

    private void Start()
    {
        Change();
    }
    public void Change()
    {
        vr_3d = !vr_3d;
        vr_camrea.SetActive(vr_3d);
        nor_camrea.SetActive(!vr_3d);
        SetLeftRight();
    }
    [TextBox("两眼和中心距离")]
    public float x=0.4f;
    public void SetLeftRight()
    {
        var ary=vr_camrea.GetComponentsInChildren<Camera>(true);
        ary[0].transform.localPosition = Vector3.left * x;
        ary[1].transform.localPosition = Vector3.right * x;
    }
    public void ResetDir()
    {
        transform.localEulerAngles = Vector3.up * (0 - transform.GetChild(0).localEulerAngles.y);
        //transform.localEulerAngles=Vector3.up*( 180 - transform.GetChild(0).localEulerAngles.y);
    }
}
