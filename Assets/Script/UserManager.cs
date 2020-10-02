using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField id;
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField address;
    [SerializeField] private TMP_InputField password;


    public void Setter(int id, string userName, string address, string password)
    {
        this.id.text = id + "";
        this.userName.text = userName;
        this.address.text = address;
        this.password.text = password;
    }
}
