using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class DataTable 
{
    // data 
    object [,] data; 
    


    public DataTable (int width, int height) 
    {
        data = new object[width, height]; 
    }



    //  Table info  ------------------------------------------------- 
    public int Width => data.GetLength(0); 
    public int Height => data.GetLength(1); 



    //  Data  ------------------------------------------------------- 
    public void Set (int x, int y, object value) 
    {
        data[x, y] = value; 
    }

    public object GetObject (int x, int y) 
    {
        return data[x, y]; 
    }

    public string GetString (int x, int y) 
    {
        return (string) data[x, y]; 
    }

    public int GetInt (int x, int y) 
    {
        return (int)(double) data[x, y]; 
    }

    public float GetFloat (int x, int y) 
    {
        return (float) (double) data[x, y]; 
    }

}

}
