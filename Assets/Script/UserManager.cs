using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField id;
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField address;
    [SerializeField] private TMP_InputField password;


    public void Setter(Person person)
    {
        this.id.text = person.Id + "";
        this.userName.text = person.Name;
        this.address.text = person.Address;
        this.password.text = person.Password;
    }

    public Person GetPerson()
    {
        var person = new Person
        {
            Name = userName.text,
            Address = address.text,
            Password = password.text,
            Role = "User"
        };
        return person;  
    }
}
