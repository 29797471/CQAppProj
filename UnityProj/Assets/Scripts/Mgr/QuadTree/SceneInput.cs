using UnityCore;
using UnityEngine;

public class SceneInput : MonoBehaviourExtended
{
    Camera cam;
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        QuadTreeMgr.instance.Init(RectUtil.GetRect(new Vector2(500, 0), new Vector2(2000, 2000)));
    }
    private void OnEnable()
    {
        if(Input.touchSupported)
        {
            InputMgr.instance.OneTouch.Click_CallBack(MouseLeft_OnClick, DisabledHandle);
        }
        else if(Input.mousePresent)
        {
            InputMgr.instance.MouseLeft.Click_CallBack(MouseLeft_OnClick, DisabledHandle);
        }
    }
    private void Start()
    {
        AddAll();
    }

    public bool rayCheck=true;
    private void MouseLeft_OnClick()
    {
        //Debug.Log(Torsion.Serialize(Input.mousePosition));
        
        QuadTreeMgr.instance.drawCheck.Clear();
        if(rayCheck)
        {
            var p = cam.ScreenToWorldPoint(Input.mousePosition, new Plane(Vector3.up, Vector3.zero));
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if(p!=null)
            {
                QuadTreeMgr.instance.drawCheck.DrawLine(ray.origin, (Vector3)p);
            }
            var list = QuadTreeMgr.instance.GetIntersects<ColliderTreeNode>(ray);
            if(list.Count>0)
            {
                var it = list[0];
                //Debug.Log(it.bc.PathInHierarchy());
                QuadTreeMgr.instance.drawCheck.DrawBounds(it.worldBounds);
                var ct=it.bc.GetComponent<ClickTrigger>();
                if (ct != null) ct.OnClick();
            }
        }
        else
        {
            var p = cam.ScreenToWorldPoint(Input.mousePosition, new Plane(Vector3.up, Vector3.zero));

            var list = QuadTreeMgr.instance.GetIntersects<ColliderTreeNode>(((Vector3)p).ToVector2());
            foreach (var it in list)
            {
                Debug.Log(it.bc.PathInHierarchy());
                QuadTreeMgr.instance.drawCheck.DrawBounds(it.worldBounds);
            }
        }
    }

    public void AddAll()
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var it in colliders)
        {
            QuadTreeMgr.instance.Input(new ColliderTreeNode(it),DestroyHandle);
        }
        //QuadTreeMgr.instance.Draw();
    }
}
public class ColliderTreeNode : IQuadTreeObj
{
    public Collider bc { get; private set; }

    /// <summary>
    /// 世界坐标系下的外围包围盒
    /// </summary>
    public Bounds worldBounds;

    public ColliderTreeNode(Collider bc)
    {
        this.bc = bc;
        worldBounds = bc.bounds;
        
        var center = worldBounds.center.ToVector2();
        var size = worldBounds.size.ToVector2();
        bc.enabled = false; 
        rect = RectUtil.GetRect(center, size);
    }
    public override bool CheckIntersects(object sharp)
    {
        if (sharp is Vector2)
        {
            return rect.Contains((Vector2)sharp);
        }
        else if (sharp is Rect)
        {
            return rect.Intersects((Rect)sharp);
        }
        else if (sharp is Bounds)
        {
            return worldBounds.Intersects((Bounds)sharp);
        }
        else if (sharp is Ray)
        {
            return worldBounds.IntersectRay((Ray)sharp,out priority);
        }
        return false;
    }
    public override string ToString()
    {
        return bc.PathInHierarchy();
    }
}
public class BoxColliderTreeNode : IQuadTreeObj
{
    public BoxCollider bc { get; private set; }

    /// <summary>
    /// 本地坐标系下的外围包围盒
    /// </summary>
    public Bounds localBounds;

    /// <summary>
    /// 世界坐标系下的外围包围盒
    /// </summary>
    public Bounds worldBounds;

    public BoxColliderTreeNode(BoxCollider bc)
    {
        this.bc = bc;
        localBounds = new Bounds(bc.center, bc.size);
        worldBounds = bc.bounds;
        bc.enabled = false;
        rect = RectUtil.GetRect(worldBounds.center, worldBounds.size);
    }
    public override bool CheckIntersects(object sharp)
    {
        if (sharp is Vector2)
        {
            return rect.Contains((Vector2)sharp);
        }
        else if (sharp is Rect)
        {
            return rect.Intersects((Rect)sharp);
        }
        else if (sharp is Bounds)
        {
            return worldBounds.Intersects((Bounds)sharp);
        }
        else if (sharp is Ray)
        {
            var ray = (Ray)sharp;
            var inverseMat = bc.transform.worldToLocalMatrix;
            var mat = bc.transform.localToWorldMatrix;
            float dis;
            if (worldBounds.IntersectRay(ray))
            {
                var localRay = inverseMat.MultiplyRay(ray);
                var bl = localBounds.IntersectRay(localRay, out dis);
                if(bl)
                {
                    var localColliderPoint = localRay.GetPoint(dis);
                    var p = mat.MultiplyPoint(localColliderPoint);
                    priority = Vector3.Distance(p, ray.origin);
                    return true;
                }
            }
        }
        return false;
    }
    public override string ToString()
    {
        return bc.PathInHierarchy();
    }
}

