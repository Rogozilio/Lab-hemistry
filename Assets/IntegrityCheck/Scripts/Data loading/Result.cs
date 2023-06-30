using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.IntegrityCheck
{

public class Result<T>
{
    public T data;
    public bool success;
    public string message;


    public void Success (T data) 
    {
        this.data = data;
        success = true;
    }


    public void Error (string message) 
    {
        success = false;
        this.message = message;
    }

    public void Error (string title, string path) 
    {
        success = false;
        this.message = 
            "Ошибка: " + title + "\n" + 
            "Файл: "   + path  + "\n" + 
            "---------\n";
    }

    public void Error (string title, string path, string details) 
    {
        success = false;
        this.message = 
            "<b>Ошибка:</b> "   + title   + "\n" + 
            "<b>Файл:</b> "     + path    + "\n" + 
            "<b>Описание:</b> " + details + "\n" + 
            "---------\n";
    }

}


public class Result : Result<object>
{

}

}