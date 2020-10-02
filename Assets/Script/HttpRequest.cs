﻿using Newtonsoft.Json;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using Test_for_DB.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequest : MonoBehaviour
{
	private readonly string basePath = "https://localhost:44374/api/People";

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
		Get();
    }

    public void Get()
	{
		RestClient.Get(basePath).Then(res =>
		{
			Debug.Log(res.Text);
			Person[] arr = JsonConvert.DeserializeObject<Person[]>(res.Text);
			Debug.Log(arr[0].Address);
            

		}).Catch(err => Debug.LogError(err.Message));
	}
	/*
	public void Post()
	{

		// We can add default query string params for all requests
		RestClient.DefaultRequestParams["param1"] = "My first param";
		RestClient.DefaultRequestParams["param3"] = "My other param";

		currentRequest = new RequestHelper
		{
			Uri = basePath + "/posts",
			Params = new Dictionary<string, string> {
				{ "param1", "value 1" },
				{ "param2", "value 2" }
			},
			Body = new Post
			{
				title = "foo",
				body = "bar",
				userId = 1
			},
			EnableDebug = true
		};
		RestClient.Post<Post>(currentRequest)
		.Then(res => {

			// And later we can clear the default query string params for all requests
			RestClient.ClearDefaultParams();

			this.LogMessage("Success", JsonUtility.ToJson(res, true));
		})
		.Catch(err => this.LogMessage("Error", err.Message));
	}

	public void Put()
	{

		currentRequest = new RequestHelper
		{
			Uri = basePath + "/posts/1",
			Body = new Post
			{
				title = "foo",
				body = "bar",
				userId = 1
			},
			Retries = 5,
			RetrySecondsDelay = 1,
			RetryCallback = (err, retries) => {
				Debug.Log(string.Format("Retry #{0} Status {1}\nError: {2}", retries, err.StatusCode, err));
			}
		};
		RestClient.Put<Post>(currentRequest, (err, res, body) => {
			if (err != null)
			{
				this.LogMessage("Error", err.Message);
			}
			else
			{
				this.LogMessage("Success", JsonUtility.ToJson(body, true));
			}
		});
	}

	public void Delete()
	{

		RestClient.Delete(basePath + "/posts/1", (err, res) => {
			if (err != null)
			{
				this.LogMessage("Error", err.Message);
			}
			else
			{
				this.LogMessage("Success", "Status: " + res.StatusCode.ToString());
			}
		});
	}

	public void AbortRequest()
	{
		if (currentRequest != null)
		{
			currentRequest.Abort();
			currentRequest = null;
		}
	}

	public void DownloadFile()
	{

		var fileUrl = "https://raw.githubusercontent.com/IonDen/ion.sound/master/sounds/bell_ring.ogg";
		var fileType = AudioType.OGGVORBIS;

		RestClient.Get(new RequestHelper
		{
			Uri = fileUrl,
			DownloadHandler = new DownloadHandlerAudioClip(fileUrl, fileType)
		}).Then(res => {
			AudioSource audio = GetComponent<AudioSource>();
			audio.clip = ((DownloadHandlerAudioClip)res.Request.downloadHandler).audioClip;
			audio.Play();
		}).Catch(err => {
			this.LogMessage("Error", err.Message);
		});
	}
	*/
}
