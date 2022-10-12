using System;
using System.Collections;
using System.Collections.Generic;
using Cursor;
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

    private void OnDisable()
    {
        _isActive = false;
    }

    public void ShowOutline()
    {
        _isReadyToAction = true;
        if (!_outline.enabled)
        {
            _outline.enabled = true;

            if (GetType() == typeof(MoveMouseItem))
                UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Select, new Vector2(10, 10),CursorMode.Auto);
            else
                UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Click, new Vector2(10, 0), CursorMode.Auto);
        }
    }

    public void HideOutline()
    {
        _isReadyToAction = false;
        if (_outline.enabled)
        {
            _outline.enabled = false;
            
            UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Arrow, Vector2.zero, CursorMode.Auto);
        }
    }
}