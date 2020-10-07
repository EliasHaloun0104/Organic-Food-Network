using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Security
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
}
