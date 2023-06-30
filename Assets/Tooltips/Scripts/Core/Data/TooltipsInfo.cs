using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

[System.Serializable]
public class TooltipsInfo 
{
    public Tooltip [] tooltips;


    public void Validate () 
    {
        if (tooltips == null) tooltips = new Tooltip[0];
        
        for (int i = 0; i < tooltips.Length; i++) 
        {
            if (tooltips[i] == null) tooltips[i] = new Tooltip();
            tooltips[i].Validate();
        }
    }

    public bool IsGood () 
    {
        foreach (var tooltip in tooltips) 
        {
            if (!tooltip.IsGood()) return false;
        }

        return true;
    }
    

    [System.Serializable]
    public class Tooltip 
    {
        public string name;
        public Data [] data;


        public void Validate () 
        {
            if (name == null) name = "";
            if (data == null) data = new Data[0];

            for (int i = 0; i < data.Length; i++) 
            {
                if (data[i] == null) data[i] = new Data();
            }
        }

        public bool IsGood () 
        {
            foreach (var d in data) 
            {
                if (!d.IsGood()) return false;
            }

            return true;
        }
    }


    [System.Serializable]
    public class Data 
    {
        public string type       = "";
        public string stringData = "";
        public float numberData  = 0;

        public bool IsGood () 
        {
            return type == "title" || type == "text" || type == "progress";
        }
    }
}

}
