using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private Person person;
    private Camera originCamera;
    private bool dragging = false;
    private bool putting = false;
    private float distance;
    bool posting;

    // Start is called before the first frame update
    void Start()
    {
        originCamera = FindObjectOfType<Camera>();
        posting = false;
    }

    private void OnMouseDown()
    {
        var userRole = Utils.LoadPrefs().Role;
        if (userRole == "Admin")
        {
            distance = Vector3.Distance(transform.position, originCamera.transform.position);
            dragging = true;
        }
        else
        {
            if (!posting) 
                StartCoroutine(GetProducts());
            
        }
        
        
        //dragging = true;
        
    }


    public IEnumerator GetProducts()
    {
        posting = true;
        var request = HttpRequest.Get("ProductsByUser" + "/" + person.Id);
        yield return request.SendWebRequest();

        if (HttpRequest.Error(request))
            Debug.LogError(request.error);
        else
        {
            /*
            var headers = request.GetResponseHeaders();
            var lines = headers.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            Debug.Log(string.Join(Environment.NewLine, lines));
            */
            
            
            Product[] arr = JsonConvert.DeserializeObject<Product[]>(request.downloadHandler.text);
            string productDetails = "";
            if (arr.Length < 1)
            {
                textMeshPro.text = person.Name + ", " + person.Address + "\nThis Seller don't have any product now";
            }
            else
            {
                textMeshPro.text = person.Name + ", " + person.Address + "\n";
                foreach (var item in arr)
                {
                    productDetails += item.Name + ", " + item.Quantity + item.Unit + "\n";                   

                }
                textMeshPro.text += "\n" + productDetails;
            }
            
        }
        posting = false;
    }
        

    

    private void OnMouseUp()
    {
        var userRole = Utils.LoadPrefs().Role;
        if(userRole == "Admin")
        {
            person.XCoordinate = transform.position.x;
            person.YCoordinate = transform.position.y;
            dragging = false;
        }
        else
        {
            if (!putting)
                StartCoroutine(UpdatePerson());
        }

        
        
        
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
        textMeshPro.text = person.Name + ", " + person.Address;
    }
}
