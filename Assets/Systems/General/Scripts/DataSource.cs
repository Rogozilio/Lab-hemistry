using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class DataSource <T> 
{
    Func<T> source; 
    T localData; 



    public DataSource (T data) 
    {
        localData = data; 
    }

    public DataSource (Func<T> source) 
    {
        this.source = source; 
    }



    public static implicit operator T (DataSource<T> dataSource) 
    {
        if (dataSource.source != null) 
        {
            return dataSource.source(); 
        }
        else 
        {
            return dataSource.localData; 
        }
    }

}



public class FloatSource : DataSource<float> 
{
    public FloatSource (float data) : base(data) {} 
    public FloatSource (Func<float> source) : base(source) {} 
}

}

