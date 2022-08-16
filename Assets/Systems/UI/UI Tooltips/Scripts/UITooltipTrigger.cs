using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.EventSystems; 



namespace VirtualLab.UITooltips 
{

public class UITooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler 
{
	[SerializeField] UITooltip tooltip; 



	//  Pointer enter  ---------------------------------------------- 
	public void OnPointerEnter (PointerEventData eventData)
	{
		tooltip.Show(eventData.position); 
	}

	public void OnPointerMove (PointerEventData eventData)
	{
		tooltip.SetPosition(eventData.position); 
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		tooltip.Hide(); 
	}



	//  Pointer down  ----------------------------------------------- 
	public void OnPointerDown (PointerEventData eventData)
	{
		tooltip.Hide(); 
	}

}

}
