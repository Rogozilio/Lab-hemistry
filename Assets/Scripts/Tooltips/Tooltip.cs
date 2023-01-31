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

    private StateItem _state;
    private void OnEnable()
    {
        _tooltipUI = FindObjectOfType<TooltipScreenSpaceUI>();
        TryGetComponent(out _state);
    }

    private void OnMouseEnter()
    {
        if(Input.GetMouseButton(0)) return;
        
        if(_state != null && _state.State != StateItems.Idle) return;
        _tooltipUI.ShowTooltip(Tooltips[index]);
    }

    private void OnMouseOver()
    {
        _tooltipUI.SetPositionTooltip = transform.position + offsetPosition;
        if(_state != null && _state.State != StateItems.Idle) _tooltipUI.HideTooltip();
    }

    private void OnMouseExit()
    {
        _tooltipUI.HideTooltip();
    }
}
