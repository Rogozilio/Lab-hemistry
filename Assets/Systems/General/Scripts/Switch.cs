using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class Switch : MonoBehaviour 
{
	[SerializeField] GameObject objectON; 
	[SerializeField] GameObject objectOFF; 
	[SerializeField] bool _on; 



	void OnValidate () 
	{
		UpdateUI(); 
	}



	//  State  ------------------------------------------------------ 
	public bool on 
	{
		get => _on; 
		set {
			_on = value; 
			UpdateUI(); 
		}
	}

	public void Toggle () 
	{
		on = !on; 
	}



	//  UI  --------------------------------------------------------- 
	void UpdateUI () 
	{
		objectON?.SetActive(on); 
		objectOFF?.SetActive(!on); 
	}

}

}

