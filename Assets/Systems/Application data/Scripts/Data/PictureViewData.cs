using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public class PictureViewData 
{
    List<PictureData> _pictures = new List<PictureData>(); 
	public IReadOnlyList<PictureData> pictures => _pictures; 


	public PictureViewData (List<PictureData> pictures) 
	{
		this._pictures = pictures; 
	}
    
}

}
