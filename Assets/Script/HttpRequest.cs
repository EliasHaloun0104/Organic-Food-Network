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
	
	public static readonly string API_Base = "https://localhost:44386/api/";

	
	private void LogMessage(string title, string message)
	{
#if UNITY_EDITOR
		EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
	}
    
	//Used for login & SignUp
	
	public static UnityWebRequest Post(string json, string addUrl)
    {
		UnityWebRequest request = new UnityWebRequest(API_Base + addUrl, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("content-Type", "application/json");
		request.SetRequestHeader("Accept", "application/json");
		return request;
	}
	
	public static UnityWebRequest Put(int id, string json, string addUrl)
    {
		UnityWebRequest request = new UnityWebRequest(API_Base + addUrl  + "/" + id, "PUT");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("content-Type", "application/json");
		request.SetRequestHeader("Accept", "application/json");
		return request;
	}

    
	public static UnityWebRequest Get(string addUrl)
	{
		
		UnityWebRequest request = new UnityWebRequest(API_Base + addUrl, "GET");
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("content-Type", "application/json");
		request.SetRequestHeader("Accept", "application/json");
		return request;			
		
	}

	public static bool Error(UnityWebRequest request)
	{
		return request.isNetworkError || request.isHttpError;

	}








}
