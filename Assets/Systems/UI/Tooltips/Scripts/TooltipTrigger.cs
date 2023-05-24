// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
//
//
// namespace VirtualLab.Tooltips 
// {
//
// public class TooltipTrigger : MonoBehaviour 
// {
//     [SerializeField] Tooltip tooltip; 
// 	[SerializeField] Transform tooltipPoint; 
// 	[SerializeField] bool _interactable = true; 
//     
//
//
// 	//  State  ------------------------------------------------------ 
// 	public bool interactable 
// 	{
// 		get => _interactable; 
// 		set {
// 			_interactable = value; 
// 			HideTooltip(); 
// 		}
// 	}
//
//
//
//     //  Tooltip  ---------------------------------------------------- 
//     void ShowTooltip () 
//     {
//         tooltip.Show(tooltipPoint.position); 
//     }
//
//     void UpdateTooltip () 
//     {
//         tooltip.UpdatePosition(tooltipPoint.position); 
//     }
//
//     void HideTooltip () 
//     {
//         tooltip.Hide(); 
//     }
//
//
//
// 	//  Mouse input  ------------------------------------------------ 
// 	void OnMouseEnter () 
// 	{
// 		if (interactable) ShowTooltip(); 
// 	}
//
// 	void OnMouseOver () 
// 	{
// 		if (interactable) UpdateTooltip(); 
// 	}
//
// 	void OnMouseExit () 
// 	{
// 		HideTooltip(); 
// 	}
//
// }
//
// }
