using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTrigger : MonoBehaviour, IPointerClickHandler
{
    public string openWin = "SelectBuild";
    public void OnClick()
    {
        Debug.Log("OnClick\n"+this.PathInHierarchy());
        //DlgMgr.instance.Open(openWin);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnClick\n" + this.PathInHierarchy());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("!AD");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("!OnTriggerEnter");
    }
    /*
    private void OnMouseUpAsButton()
    {
        
    }
    */
}
