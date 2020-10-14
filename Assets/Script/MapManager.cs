using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour
{
    [SerializeField] private PinManager pin;
    [SerializeField] private Person person;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        person = Utils.LoadPrefs();
		var request = HttpRequest.Get("People");
		yield return request.SendWebRequest();
		
        if (HttpRequest.Error(request))
			Debug.LogError(request.error);
        else
        {            
            Person[] arr = JsonConvert.DeserializeObject<Person[]>(request.downloadHandler.text);            
            foreach (var item in arr)
            {
                PinManager clone = Instantiate(pin);
                clone.SetPerson(item);
                
                    
            }
        }		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
