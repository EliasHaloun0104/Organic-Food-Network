using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Utils
{    
    public static string HashPassword(string inputString)
    {
        byte[] hash;
        using (HashAlgorithm algorithm = SHA256.Create())
            hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hash)
            sb.Append(b.ToString("X2"));
        return sb.ToString();
    }

    public static void SavePrefs(Person person)
    {
        var jsonPerson = JsonConvert.SerializeObject(person);
        PlayerPrefs.SetString("UserInfo", jsonPerson);        
    }
    
    public static Person LoadPrefs()
    {        
        var jsonPerson = PlayerPrefs.GetString("UserInfo");
        Person person = JsonConvert.DeserializeObject<Person>(jsonPerson);
        return person;        
    }


    public static bool IsDataExist()
    {
        return PlayerPrefs.HasKey("UserInfo");
    }
}
