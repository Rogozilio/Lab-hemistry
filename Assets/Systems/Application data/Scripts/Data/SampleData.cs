using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SampleData 
{
	public int groupIndex; 
	public int id; 
	public Sprite sprite; 
	public string imageReportPath; 



	public SampleData (int groupIndex, int id, Sprite sprite) 
	{
		this.groupIndex = groupIndex; 
		this.id = id; 
		this.sprite = sprite; 
		this.imageReportPath = CreateImageReportPath(id); 
	}

	string CreateImageReportPath (int id) 
	{
		return "./Microscope report images/" + (groupIndex + 1) + " Группа/" + id + ".png"; 
	}



	public override string ToString () 
	{
		return "id: " + id + ",\tsprite: " + sprite.name; 
	}
}

}
