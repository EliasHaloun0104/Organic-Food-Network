using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Camera originCamera;
    private bool dragging = false;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        originCamera = FindObjectOfType<Camera>();
    }

    private void OnMouseDown()
    {
        Debug.Log("MouseDown");
        distance = Vector3.Distance(transform.position, originCamera.transform.position);
        dragging = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("MouseUp");
        dragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Ray ray = originCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            rayPoint.z = 0;
            transform.position = rayPoint;
        }

    }
}
