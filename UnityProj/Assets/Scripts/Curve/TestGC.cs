using CqCore;
using UnityCore;
using UnityEngine;
using UnityEngine.Events;

public class TestGC : MonoBehaviourExtended
{
    public Transform aTran;
    public Transform bTran;

    
    public Vector3 v=Vector3.up;
    static UnityEvent e = new UnityEvent();
    CancelHandle handle = new CancelHandle();

    [ContextMenu("Clear")]
    public void AA()
    {
        Debug.Log(TimeUtil.Unix_timestamp);
    }
    void Update()
    {

    }

    private  void InitModel_EventHandler(object sender, EventMgr.InitModel._EventArgs e)
    {
        throw new System.NotImplementedException();
    }
    private  void InitModel_EventHandlerX(object sender, EventMgr.InitModel._EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public  void TT()
    {

    }
}
