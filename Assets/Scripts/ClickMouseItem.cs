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

    private void OnMouseDown()
    {
        base.OnMouseDown();

        if (IsReadyToAction && StateItem.State == StateItems.Idle)
        {
            StateItem.ChangeState(StateItems.Interacts);

            OnClicks[_numberClick++].Invoke();
            _numberClick = (_numberClick < OnClicks.Length) ? _numberClick: 0;
        }
    }

    private void OnMouseExit()
    {
        base.OnMouseExit();
    }
}