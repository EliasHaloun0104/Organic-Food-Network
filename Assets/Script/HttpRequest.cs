using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class HttpRequest : MonoBehaviour
{
	private readonly string basePath = "https://localhost:44386/api/People";
	private readonly string basePath2 = "https://localhost:44386/api/Products";
	
	bool posting = false;
	[SerializeField] UserManager userManager;
	private void LogMessage(string title, string message)
	{
#if UNITY_EDITOR
		EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
	}
    private void Start()
    {
		StartCoroutine(GetPeople(basePath));
    }

    
	public IEnumerator GetPeople(string uri)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();
			
			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			if (webRequest.isNetworkError || webRequest.isHttpError)
				Debug.LogError(webRequest.error);
			Person[] arr = JsonConvert.DeserializeObject<Person[]>(webRequest.downloadHandler.text);
			Debug.Log(arr[0].Name);
			userManager.Setter(arr[0]);
		}	
	}
	
	


	


	
}
