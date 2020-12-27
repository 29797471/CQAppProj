using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AutoView3D : MonoBehaviour 
{
    public Camera ca;
    public const int srcHeight = 720;
    public const int srcWidth = 1280;

    public int Screen_height;
    public int Screen_width;
    public int pixelWidth;
    public int pixelHeight;

    public bool bl;
    // Use this for initialization
    void Start () 
    {
        Debug.Log("Screen.height="+Screen.height+"\nScreen.width="+Screen.width);
        Debug.Log("pixelWidth" + ca.pixelWidth + "pixelHeight" + ca.pixelHeight);
	}
	
	// Update is called once per frame
	void Update ()
    {
        pixelWidth = ca.pixelWidth;
        pixelHeight = ca.pixelHeight;
        Screen_height = Screen.height;
        Screen_width = Screen.width;
        if( (Screen.width * srcHeight  > srcWidth * Screen.height)== bl)
        {
            transform.localScale = new Vector3(1, 1, 0) * (Screen.width * 1.0f / srcWidth); 
        }else
        {
            transform.localScale = new Vector3(1, 1, 0) * (Screen.height * 1.0f / srcHeight);
        }
    }
}
