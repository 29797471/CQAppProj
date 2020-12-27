using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    public float rotateSpeed = 30f;
    public void Update()
    {
        Quaternion rot = Quaternion.Euler(0, 0, Time.time * rotateSpeed);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        GetComponent<Renderer>().material.SetMatrix("_TextureRotation", m);
    }
}
