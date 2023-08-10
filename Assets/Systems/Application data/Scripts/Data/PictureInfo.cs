using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

[System.Serializable]
public class PictureInfo 
{
    public Entry [] about; 
    public Entry [] theory; 
    public Entry [] instructions; 


    public PictureInfo () 
    {
        about        = new Entry[0];
        theory       = new Entry[0];
        instructions = new Entry[0];
    }


    public bool hasPictures => 
		about?.Length != 0 || 
		theory?.Length != 0 || 
		instructions?.Length != 0; 


    [System.Serializable]
    public class Entry 
    {
        public Tab tab;
        public string pictureName; 
        public string pictureDescription; 
        public int pageNumber; 
        public int x; 
        public int y; 
    }

}

}
