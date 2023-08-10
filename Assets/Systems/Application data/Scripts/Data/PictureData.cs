using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public class PictureData 
{
    public Picture picture { get; private set; } 
    public Tab tab 		   { get; private set; } 
    public int pageNumber  { get; private set; } 
    public int buttonX 	   { get; private set; } 
    public int buttonY 	   { get; private set; } 


    public PictureData (
        Picture picture, 
        Tab tab, 
        int pageNumber, 
        int buttonX, 
        int buttonY 
    ) {
        this.picture = picture; 
        this.tab = tab; 
        this.pageNumber = pageNumber; 
        this.buttonX = buttonX; 
        this.buttonY = buttonY; 
    }
}

}
