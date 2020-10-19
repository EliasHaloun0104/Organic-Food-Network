using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{  
    public enum UserManagerCase
    {
        SIGNUP, LOGIN, UPDATEINFO
    }
    private bool posting = false;
    [SerializeField] private TextMeshProUGUI subtitle;
    [SerializeField] private TMP_InputField id;
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField address;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI message;    
    [SerializeField] private TextMeshProUGUI actionButton;
    [SerializeField] private TextMeshProUGUI switchButton;
    [SerializeField] private UserManagerCase userManagerCase;
    [SerializeField] private CanvasGroup canvasID;
    [SerializeField] private CanvasGroup canvasAddress;
    private Person person;


    private void Start()
    {

        LoadScene();
    }


    private void LoadScene()
    {
        person = new Person();
        switch (userManagerCase)
        {
            case UserManagerCase.SIGNUP:
                subtitle.text = "Welcome";
                actionButton.text = "Sign Up";
                canvasID.alpha = 0f;
                canvasID.blocksRaycasts = false;
                canvasAddress.alpha = 1f;
                canvasAddress.blocksRaycasts = true;

                break;
            case UserManagerCase.LOGIN:
                subtitle.text = "Welcome";
                actionButton.text = "Login";
                canvasID.alpha = 0f;
                canvasID.blocksRaycasts = false;
                canvasAddress.alpha = 0f;
                canvasAddress.blocksRaycasts = false;
                LoadPersonFromPref();
                InterfacePerson();
                break;
            case UserManagerCase.UPDATEINFO:
                subtitle.text = "Update";
                actionButton.text = "Update";
                canvasID.alpha = 1f;
                canvasID.blocksRaycasts = true;
                canvasAddress.alpha = 1f;
                canvasAddress.blocksRaycasts = true;
                LoadPersonFromPref();
                InterfacePerson();
                break;
        }
    }

    public void ActionButton()
    {
        if (!posting)
        {
            switch (userManagerCase)
            {
                case UserManagerCase.SIGNUP:
                    StartCoroutine(SignUp());
                    break;
                case UserManagerCase.LOGIN:
                    StartCoroutine(Login());
                    break;
                case UserManagerCase.UPDATEINFO:
                    StartCoroutine(UpdateInfo());
                    break;
            }
        }

        else 
            message.text = "Waiting respond from server";
    }


    public void SwitchButton()
    {
        switch (userManagerCase)
        {
            case UserManagerCase.LOGIN:
                userManagerCase = UserManagerCase.SIGNUP;
                switchButton.text = "to update info click here";
                break;
            case UserManagerCase.SIGNUP:
                userManagerCase = UserManagerCase.UPDATEINFO;
                switchButton.text = "to login click here";                
                break;
            case UserManagerCase.UPDATEINFO:
                userManagerCase = UserManagerCase.LOGIN;
                switchButton.text = "to SignUp click here";
                break;
        }
        LoadScene();
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
            UpdatePersonObject();
            person.Password = Utils.HashPassword(password.text);
            var request = HttpRequest.Post(SerializePerson(), "SignUp");
            yield return request.SendWebRequest();
            if (HttpRequest.Error(request))
                message.text = request.error;
            else
            {
                var data = request.downloadHandler.text;
                DeserializePerson(data);
                person.Password = password.text; //Unhash to save in pref
                
                //Save user info in pref                
                Utils.SavePrefs(person);
                //Move to next scene
                yield return new WaitForSeconds(1);
                Utils.PortalScene();
            }
        }
        posting = false;
    }

    public IEnumerator Login()
    {
        posting = true;
        UpdatePersonObject();
        person.Password = Utils.HashPassword(password.text);
        var request = HttpRequest.Post(SerializePerson(), "Login");
        yield return request.SendWebRequest();
        if (HttpRequest.Error(request))
            message.text = request.error;
        else
        {
            //Save user info in pref
            var data = request.downloadHandler.text;
            Debug.Log(data);
            DeserializePerson(data);
            person.Password = password.text; //Unhash to save in pref
            Utils.SavePrefs(person);
            //Move to next scene
            yield return new WaitForSeconds(1);
            Utils.PortalScene();
        }

        posting = false;
    }




    public IEnumerator UpdateInfo()
    {
        posting = true;
        UpdatePersonObject();
        person.Password = Utils.HashPassword(password.text);
        var request = HttpRequest.Put(person.Id, SerializePerson(), "People");
        yield return request.SendWebRequest();
        if (HttpRequest.Error(request))
            message.text = request.error;
        else
        {
            person.Password = password.text; //Unhash to save in pref
            Utils.SavePrefs(person);
            message.text = "your info updated successfully";
            yield return new WaitForSeconds(1);
            //Move to next scene
            Utils.PortalScene();
        }

        posting = false;
    }

    

    public void UpdatePersonObject()
    {
        person = new Person()
        {
            Id = id.text.Length>0? int.Parse(id.text): 0,
            Name = userName.text,
            Address = address.text,
            Password = password.text,
            Role = "User"    
        };        
    }

    public string SerializePerson()
    {
        return JsonConvert.SerializeObject(person);
    }
    public void DeserializePerson(string json)
    {
        person = JsonConvert.DeserializeObject<Person>(json);
    }

    public void InterfacePerson()
    {
        id.text = person.Id + "";
        userName.text = person.Name;
        address.text = person.Address;
        password.text = person.Password;
    }

    public void LoadPersonFromPref()
    {
        if (Utils.IsDataExist())
        {
            person = Utils.LoadPrefs();
        }
    }

}
