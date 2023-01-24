using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tooltip : MonoBehaviour
{
    private TooltipScreenSpaceUI _tooltipUI;
    public Dictionary<int, string> Tooltips => _tooltipUI.Tooltips;
    public int index;
    public Vector3 offsetPosition;
    private void OnEnable()
    {
        _tooltipUI = FindObjectOfType<TooltipScreenSpaceUI>();
    }

    private void OnMouseEnter()
    {
        _tooltipUI.ShowTooltip(Tooltips[index]);
    }

    private void OnMouseOver()
    {
        _tooltipUI.SetPositionTooltip = transform.position + offsetPosition;
    }

    private void OnMouseExit()
    {
        _tooltipUI.HideTooltip();
    }
}
