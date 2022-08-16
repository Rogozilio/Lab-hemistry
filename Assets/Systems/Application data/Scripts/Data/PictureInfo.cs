using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

[System.Serializable]
public class PictureInfo 
{
    public Entry [] about; 
    public Entry [] theory; 
    public Entry [] instructions; 


    [System.Serializable]
    public class Entry 
    {
        public string pictureName; 
        public string pictureDescription; 
        public int pageNumber; 
        public int x; 
        public int y; 
    }



	public bool hasPictures => 
		about.Length != 0 || 
		theory.Length != 0 || 
		instructions.Length != 0; 

}

}
