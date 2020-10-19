using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProductManager : MonoBehaviour
{
    private bool posting = false;
    Product[] products;
    [SerializeField] int currentIndex;

    [SerializeField] private TMP_InputField productName;
    [SerializeField] private TMP_InputField quantityField;
    [SerializeField] private TMP_InputField unit;
    [SerializeField] private TMP_InputField dateCreated;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Toggle isHidden;


    public void SwapButton(int addOrBack)
    {
        if(currentIndex + addOrBack >=0 && currentIndex + addOrBack < products.Length)
        {
            currentIndex += addOrBack;
            InterfaceProduct(products[currentIndex]);
        }
            
    }
    // Start is called before the first frame update

    void InterfaceProduct(Product product)
    {
        productName.text = product.Name;
        quantityField.text = product.Quantity + "";
        unit.text = product.Unit;
        dateCreated.text = product.DateCreated.ToString("MM / dd / yyyy");
        isHidden.isOn = product.IsHidden;
    }

    public Product GenerateProduct()
    {
        return new Product()
        {
            IsHidden = false,
            Quantity = int.Parse(quantityField.text),
            Unit = unit.text,
            Name = productName.text,
            PersonID = 8,
            DateCreated = DateTime.Today
        };         
    }
    void Start()
    {
        StartCoroutine(GetProducts());


        //Genereate DumpData
        /*
        var prod = new string[] { "Tomato", "Milk", "Onion", "Cucumber", "Lettuce", "Potato", "Herbs", "strawberry" };
        var unitKK = new string[] { "Kg", "L", "Kg", "Pcs", "Pcs", "Kg", "Pcs", "Box" };
        while (true)
        {
            posting = true;
            var rand = UnityEngine.Random.Range(0, 8);
            Product product = new Product()
            {
                IsHidden = false,
                Quantity = UnityEngine.Random.Range(1,8),
                Unit = unitKK[rand],
                Name = prod[rand],
                PersonID = 8,
                DateCreated = DateTime.Today.AddDays(UnityEngine.Random.Range(0,-365))
            };

            var jsonProduct = JsonConvert.SerializeObject(product);
            Debug.Log(jsonProduct);
            var request = HttpRequest.Post(jsonProduct, "Products");
            yield return request.SendWebRequest();
            if (HttpRequest.Error(request))
                message.text = request.error;
            else
                message.text = "Done!";
            posting = false;
            yield return new WaitForSeconds(0.5f);
        }
        */

    }
    
        public void ActionButton(string productAction)
    {

        
        if (!posting)
        {
            switch (productAction)
            {
                case "GET":
                    StartCoroutine(GetProducts());
                    break;
                case "POST":
                    StartCoroutine(PostProduct());
                    break;
                case "PUT":
                    StartCoroutine(PutProduct());
                    break;
                case "DEL":
                    StartCoroutine(DeleteProduct());
                    break;
                case "CLR":
                    productName.text = "";
                    quantityField.text = "";
                    unit.text = "";
                    dateCreated.text = "";
                    isHidden.isOn = false;
                    break;
            }
        }
        
        else
            message.text = "Waiting respond from server";
    }

    IEnumerator GetProducts()
    {
        var person = Utils.LoadPrefs();
        posting = true;
        var request = HttpRequest.Get("ProductsByUser" + "/" + person.Id);
        yield return request.SendWebRequest();

        if (HttpRequest.Error(request))
            Debug.LogError(request.error);
        else
        {
            products = JsonConvert.DeserializeObject<Product[]>(request.downloadHandler.text);
            InterfaceProduct(products[currentIndex]);

        }
        
        posting = false;
    }

    public IEnumerator PutProduct()
    {
        posting = true;
        products[currentIndex].Name = productName.text;
        products[currentIndex].Quantity = int.Parse(quantityField.text);
        products[currentIndex].Unit = unit.text;

        var jsonProduct = JsonConvert.SerializeObject(products[currentIndex]);
        var request = HttpRequest.Put(products[currentIndex].Id, jsonProduct, "Products");
        yield return request.SendWebRequest();
        if (HttpRequest.Error(request))
            message.text = request.error;
        else
        {                     
            message.text = "your product updated successfully";
        }
        posting = false;
    }

    

    public IEnumerator DeleteProduct()
    {
        posting = true;
        var request = HttpRequest.Delete(products[currentIndex].Id, "Products");
        yield return request.SendWebRequest();
        if (HttpRequest.Error(request))
            message.text = request.error;
        else
        {
            message.text = "your product deleted successfully";
            posting = false;
            StartCoroutine(GetProducts());           
        }
        
    }
 
    public IEnumerator PostProduct()
    {
        posting = true;
        var product = GenerateProduct();

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
