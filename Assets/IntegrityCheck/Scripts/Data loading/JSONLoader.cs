using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.IntegrityCheck
{

public static class JSONLoader 
{

    //  Main  -------------------------------------------------------
    public static IEnumerator Load (MonoBehaviour owner, string path, string typeName, Result result) 
    {
        // get desired type 
        Type type = FindTypeByName(typeName);

        // load string 
        var resultString = new Result<string>();
        var loading = StringLoader.Load(owner, path, resultString);
        yield return owner.StartCoroutine(loading);

        // check string 
        if (!resultString.success) 
        {
            result.Error(resultString.message);
            yield break;
        }

        // create and return object 
        try {
            object obj = JsonUtility.FromJson(resultString.data, type);
            result.Success(obj);
        }
        catch (Exception e) 
        {
            string title = "не удалось создать объект из JSON строки";
            string details = e.Message;
            result.Error(title, path, details);
        }
    }

    public static IEnumerator Load<T> (MonoBehaviour owner, string path, Result<T> result)
    {
        // load string 
        var resultString = new Result<string>();
        var loading = StringLoader.Load(owner, path, resultString);
        yield return owner.StartCoroutine(loading);

        // check string 
        if (!resultString.success) 
        {
            result.Error(resultString.message);
            yield break;
        }

        // create and return object 
        try {
            T obj = JsonUtility.FromJson<T>(resultString.data);
            result.Success(obj);
        }
        catch (Exception e) 
        {
            string title = "не удалось создать объект из JSON строки";
            string details = e.Message;
            result.Error(title, path, details);
        }
    }



    //  Reflection  -------------------------------------------------
    static Type FindTypeByName (string name) 
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(name);
            if (type != null)
            {
                return type;
            }
        }

        return null;
    }

}

}
