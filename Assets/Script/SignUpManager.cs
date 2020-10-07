using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour
{
    private readonly string path = "https://localhost:44386/api/SignUp";
    private bool posting = false;
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField address;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI message;

    public void SignUpButton()
    {
        if (!posting) 
            StartCoroutine(SignUp());
        else 
            message.text = "Waiting respond from server";
    }

    public IEnumerator SignUp()
    {
        posting = true;

        if(userName.text.Length< 6)
            message.text = "User name should be at least 6 charachters";        
        else if(address.text.Length< 6)        
            message.text = "Please, enter a valid address";
        else if(password.text.Length< 6)
            message.text = "password should be at least 6 charachters";
        else
        {
            Person person = new Person()
            {
                Name = userName.text,
                Address = address.text,
                Password = Security.HashPassword(password.text),
                Role = "User"

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
                SceneManager.LoadScene(3);


        }
        posting = false;
    }
   
}
