using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using VirtualLab.Testing; 



namespace VirtualLab.OutlineNS 
{

public class Test_Outline : MonoBehaviour
{
	[SerializeField] Outline selectedObject; 
	[Space] 
	[SerializeField] TestAction showOutline; 
	[SerializeField] TestAction hideOutline; 



	void Update () 
	{
		if (showOutline.Read()) ShowOutline(); 
		if (hideOutline.Read()) HideOutline(); 
	}

	void ShowOutline () 
	{
		selectedObject.ShowOutline(); 
	}

	void HideOutline () 
	{
		selectedObject.HideOutline(); 
	}

}

}
