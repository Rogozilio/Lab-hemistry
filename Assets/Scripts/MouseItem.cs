using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateItem))]
[RequireComponent(typeof(Outline))]
public class MouseItem : MonoBehaviour
{
    protected bool _isActive;
    protected bool _isReadyToAction;
    private StateItem _stateItem;

    public bool IsActive => _isActive;
    public bool IsReadyToAction
    {
        get => _isReadyToAction;
        set => _isReadyToAction = value;
    }

    public StateItem StateItem => _stateItem;

    private Outline _outline;

    protected void Awake()
    {
        _outline = GetComponent<Outline>();
        _stateItem = GetComponent<StateItem>();
    }

    protected void OnMouseOver()
    {
        _isReadyToAction = _stateItem.State == StateItems.Idle;
    }

    protected void OnMouseDown()
    {
        HideOutline();
        _isActive = true;
    }
    
    protected void OnMouseUp()
    {
        _isActive = false;
    }

    protected void OnMouseExit()
    {
        _isReadyToAction = false;

        HideOutline();
    }

    public void ShowOutline()
    {
        if (!_outline.enabled)
            _outline.enabled = true;
    }

    public void HideOutline()
    {
        if (_outline.enabled)
            _outline.enabled = false;
    }
}