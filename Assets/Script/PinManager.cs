using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private Person person;
    private ArrayList products;
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
        distance = Vector3.Distance(transform.position, originCamera.transform.position);
        if (!posting) StartCoroutine(GetProducts());
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
        textMeshPro.text = person.Name + ", " + person.Address;
    }
}
