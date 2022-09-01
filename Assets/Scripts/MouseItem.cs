using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateItem))]
public class MouseItem : MonoBehaviour
{
    private bool _isReadyToAction;
    private StateItem _stateItem;

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
        if (_stateItem.State == StateItems.Idle)
        {
            _isReadyToAction = true;
        }
        else
        {
            _isReadyToAction = false;
        }
    }

    protected void OnMouseDown()
    {
        HideOutline();
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