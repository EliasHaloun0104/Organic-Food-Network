using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AdminFeature : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private TextMeshProUGUI data;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        var request = HttpRequest.Get("People");
        yield return request.SendWebRequest();

        if (HttpRequest.Error(request))
            Debug.LogError(request.error);
        else
        {
            Person[] arr = JsonConvert.DeserializeObject<Person[]>(request.downloadHandler.text);
            InstantiateData("Number of members: " + arr.Length);

            request = HttpRequest.Get("Products");
            yield return request.SendWebRequest();
            if (HttpRequest.Error(request))
                Debug.LogError(request.error);
            else
            {
                var quantityDictionary = new Dictionary<string, List<Product>>();
                Product[] products = JsonConvert.DeserializeObject<Product[]>(request.downloadHandler.text);
                if (products.Length > 0) InstantiateData("Total Products");
                foreach (var product in products)
                {
                    if (quantityDictionary.ContainsKey(product.Name))
                    {
                        quantityDictionary[product.Name].Add(product);
                    }
                    else
                    {
                        var list = new List<Product>();
                        list.Add(product);
                        quantityDictionary.Add(product.Name,list);
                        
                    }
                }


                foreach (KeyValuePair<string, List<Product>> entry in quantityDictionary)
                {
                    float totalAmount = 0;
                    string unit;
                    unit = entry.Value[0].Unit;
                    var monthProduction = new float[12];
                    for (int i = 0; i < 12; i++)
                    {
                        monthProduction[i] = 0f;
                    }
                    foreach (var product in entry.Value)
                    {
                        totalAmount += product.Quantity;
                        monthProduction[product.DateCreated.Month-1] += product.Quantity;                        
                    }

                    InstantiateData(entry.Key + ", " + totalAmount + " " + unit);
                    string info = "";
                    for (int i = 0; i < 12; i++)
                    {
                        info += (i+1) + ": " + monthProduction[i] + ", ";
                        if (i == 5) info += "\n";
                    }
                    info = info.Substring(0, info.Length - 2);
                    InstantiateData("Production per month");
                    InstantiateData(info);
                }

            }

        }
    }


    void InstantiateData(string text)
    {
        var clone = Instantiate(data);
        clone.rectTransform.SetParent(container);
        clone.rectTransform.localScale = Vector3.one;
        clone.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
