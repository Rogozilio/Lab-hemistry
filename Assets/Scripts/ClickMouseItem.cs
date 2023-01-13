using System;
using System.Collections;
using System.Collections.Generic;
using Cursor;
using UnityEngine;
using UnityEngine.Events;


public class ClickMouseItem : MouseItem
{
    public UnityEvent[] OnClicks;

    private int _numberClick;

    public int NumberClick
    {
        set => _numberClick = value;
        get => _numberClick;
    }

    private void Update()
    {
        _isActive = StateItem.State != StateItems.Idle;

        if (Input.GetMouseButtonDown(0) && IsReadyToAction && !IsActive)
        {
            MouseClick();
        }
    }

    private void MouseClick()
    {
        if (OnClicks.Length == 0) return;

        StateItem.ChangeState(StateItems.Interacts);

        OnClicks[_numberClick++].Invoke();
        _numberClick = (_numberClick < OnClicks.Length) ? _numberClick : 0;
    }

    public void ExecuteMouseClickOnIndex(int numberClick)
    {
        if(_numberClick == numberClick)
            MouseClick();
    }
}