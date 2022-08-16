using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class PictureViewData 
{
    List<PictureData> _pictures = new List<PictureData>(); 

	public PictureViewData (List<PictureData> pictures) 
	{
		this._pictures = pictures; 
	}

	public IReadOnlyList<PictureData> pictures => _pictures; 
}

}
