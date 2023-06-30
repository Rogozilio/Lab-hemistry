using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.IntegrityCheck
{

public static class StringLoader 
{

    //  Main  -------------------------------------------------------
    public static IEnumerator Load (MonoBehaviour owner, string path, Result<string> result) 
    {
        // load bytes 
        var resultBytes = new Result<byte[]>();
        var loading = Network.Load(path, resultBytes);
        yield return owner.StartCoroutine(loading);

        // check bytes 
        if (!resultBytes.success) 
        {
            result.Error(resultBytes.message);
            yield break;
        }

        // create and return string 
        try {
            string s = CreateString(resultBytes.data);
            result.Success(s);
        }
        catch (Exception e) 
        {
            string title = "не удалось создать строку из байт";
            string details = e.Message;
            result.Error(title, path, details);
        }
    }



    //  Creating data  ----------------------------------------------
    static string CreateString (byte [] bytes) 
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

}

}
