using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    private Person person;
    private ArrayList products;
    private Camera originCamera;
    private bool dragging = false;
    private bool putting = false;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        originCamera = FindObjectOfType<Camera>();
    }

    private void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, originCamera.transform.position);
        dragging = true;
    }

    private void OnMouseUp()
    {
        person.XCoordinate = transform.position.x;
        person.YCoordinate = transform.position.y;
        if (!putting)
            StartCoroutine(UpdatePerson());
        dragging = false;
    }

    private IEnumerator UpdatePerson()
    {
        putting = true;
        var jsonPerson = JsonConvert.SerializeObject(person);
        var request = HttpRequest.Put(person.Id, jsonPerson, "People");
        yield return request.SendWebRequest();
        if (HttpRequest.Error(request))
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Updated");
        }
        putting = false;

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

    public void SetPerson(Person person)
    {
        this.person = person;
        transform.position = new Vector3(person.XCoordinate, person.YCoordinate);
    }
}
