using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ClickMouseItem : MouseItem
{
    public UnityEvent[] OnClicks;

    private int _numberClick;

    public int NumberClick => _numberClick;
    
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
        if(!isActiveAndEnabled) return;

        if (OnClicks.Length > 0)
        {
            StateItem.ChangeState(StateItems.Interacts);

            OnClicks[_numberClick++].Invoke();
            _numberClick = (_numberClick < OnClicks.Length) ? _numberClick: 0;
        }
    }
}