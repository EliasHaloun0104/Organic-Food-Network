using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    private readonly string path = "https://localhost:44386/api/Login";
    private bool posting = false;
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private SceneTransition sceneTransition;

    public void LoginButton()
    {
        if (!posting)
            StartCoroutine(Login());
        else
            message.text = "Waiting respond from server";
    }

    public IEnumerator Login()
    {
        posting = true;
       
        Person person = new Person()
        {
            Name = userName.text,
            Password = Security.HashPassword(password.text),

        };

        var jsonPerson = JsonConvert.SerializeObject(person);

        UnityWebRequest request = new UnityWebRequest(path, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPerson);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            message.text = request.error;
        else
            Debug.Log(request.downloadHandler.text);

        posting = false;
    }
}
