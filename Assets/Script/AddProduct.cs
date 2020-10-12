using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddProduct : MonoBehaviour
{
    private bool posting = false;
    [SerializeField] private TMP_InputField productName;
    [SerializeField] private TMP_InputField quantityField;
    [SerializeField] private TMP_InputField unit;
    [SerializeField] private TextMeshProUGUI message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PostProductButton()
    {
        if (!posting)
            StartCoroutine(PostProduct());
        else
            message.text = "Waiting respond from server";
    }

    public IEnumerator PostProduct()
    {
        posting = true;
        Product product = new Product()
        {
            IsHidden = false,
            Quantity = int.Parse(quantityField.text),
            Unit = unit.text,
            Name = productName.text,
            PersonID = 8,
            DateCreated = DateTime.Today.AddDays(-1)
        };

        var jsonProduct = JsonConvert.SerializeObject(product);
        var request = HttpRequest.Post(jsonProduct, "Products");
        yield return request.SendWebRequest();
        if (HttpRequest.Error(request))
            message.text = request.error;
        else
            message.text = "Done!";


        
        posting = false;
    }
}
