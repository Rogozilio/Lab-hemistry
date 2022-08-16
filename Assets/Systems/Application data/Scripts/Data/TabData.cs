using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class TabData 
{
    public Tab tab { get; private set; } 



    public TabData (Tab tab, List<Sprite> pages) 
    {
        this.tab = tab; 
		this._pages.AddRange(pages); 
		// Debug.Log(tab + "  " + pages.Count + "  " + this.pages.Count); 
    }



	//  Pages  ------------------------------------------------------ 
    List<Sprite> _pages = new List<Sprite>(); 

	public IReadOnlyList<Sprite> pages => _pages; 

}

}
