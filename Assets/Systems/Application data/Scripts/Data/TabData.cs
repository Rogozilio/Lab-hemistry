using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public class TabData 
{
    public Tab tab { get; private set; } 

    public string name { get; private set; }

    List<Sprite> _pages = new List<Sprite>(); 
	public IReadOnlyList<Sprite> pages => _pages; 


    public TabData (Tab tab, string name, List<Sprite> pages) 
    {
        this.tab = tab; 
        this.name = name;
		this._pages.AddRange(pages); 
    }
    

    public override string ToString () 
    {
        return "Tab: " + name + ", page count: " + pages.Count;
    }

}

}
