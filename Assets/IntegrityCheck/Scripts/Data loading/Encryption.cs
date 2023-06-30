using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.IntegrityCheck
{

public static class Encryption 
{
    
    //  Interface  --------------------------------------------------
    public static string Encode (string data, string key) 
    {
        if (data.Length == 0) return data;
        if (key .Length == 0) return data;

        byte [] dataBytes = ToBytes(data);
        byte [] keyBytes  = ToBytes(key);

        byte [] encodedBytes = XOR(dataBytes, keyBytes);

        string data64 = ToBase64(encodedBytes);

        return data64;
    }

    public static string Decode (string data64, string key) 
    {
        if (data64.Length == 0) return data64;
        if (key   .Length == 0) return data64;

        byte [] encodedBytes = FromBase64(data64);
        byte [] keyBytes     = ToBytes(key);

        byte [] dataBytes = XOR(encodedBytes, keyBytes);

        string data = FromBytes(dataBytes);
        
        return data;
    }



    //  XOR  --------------------------------------------------------
    static byte [] XOR (byte [] data, byte [] key) 
    {
        int iterationCount = Mathf.CeilToInt((float) data.Length / (float) key.Length);
        byte [] output = new byte[data.Length];

        for (int i = 0; i < output.Length; i++)
        {
            byte d = i < data.Length ? data[i] : (byte) 0;
            byte k = key[i % key.Length];
            output[i] = (byte) (d ^ k);
        }

        return output;
    }



    //  Base64  -----------------------------------------------------
    static string ToBase64 (byte [] bytes) 
    {
        return System.Convert.ToBase64String(bytes);
    }

    static byte [] FromBase64 (string data) 
    {
        return System.Convert.FromBase64String(data);
    }



    //  Bytes  ------------------------------------------------------
    static byte [] ToBytes (string data) 
    {
        return System.Text.Encoding.UTF8.GetBytes(data);
    }

    static string FromBytes (byte [] data) 
    {
        string s = System.Text.Encoding.UTF8.GetString(data);
        // s = RemoveNullCharactersInTheEnd(s);
        return s;
    }

    static string RemoveNullCharactersInTheEnd (string data) 
    {
        int nullCount = 0;

        for (int i = data.Length - 1; i >= 0; i--) 
        {
            if (data[i] == '\0') nullCount++;
            else                 break;
        }

        data = data.Substring(0, data.Length - nullCount);
        return data;
    }

}

}
