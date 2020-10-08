using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchAndZoom : MonoBehaviour
{
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }



    public void UpButton()
    {
        transform.position += Vector3.up;
    }
    
    public void DownButton()
    {
        transform.position += Vector3.down;
    }
    
    public void LeftButton()
    {
        transform.position += Vector3.left;
    }
    
    public void RighButton()
    {
        transform.position += Vector3.right;
    }

    public void ZoomIn()
    {
        if(cam.orthographicSize > 3)
            cam.orthographicSize --;
    }
    
    public void ZoomOut()
    {
        if(cam.orthographicSize <10)
            cam.orthographicSize ++;
    }
    
}
