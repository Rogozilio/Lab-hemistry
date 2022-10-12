using System;
using System.Collections;
using System.Collections.Generic;
using Cursor;
using UnityEngine;

public class LinearInput : MonoBehaviour
{
    private float _originInput;

    public Axis axisInput;

    public float GetOriginInput => _originInput;

    protected void OnEnable()
    {
        switch (axisInput)
        {
            case Axis.X:
                UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Horizontal, new Vector2(10 ,0), CursorMode.Auto);
                break;
            case Axis.Y:
                UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Vertical, new Vector2(0 ,10), CursorMode.Auto);
                break;
        }
    }

    private void OnDisable()
    {
        UnityEngine.Cursor.SetCursor(CursorSkin.Instance?.Arrow, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    public void UpdateOriginInput()
    {
        if (axisInput == Axis.X)
            _originInput = Input.mousePosition.x;
        else if (axisInput == Axis.Y)
            _originInput = Input.mousePosition.y;
    }

    public float GetInputValue(bool isForMove = true)
    {
        if (axisInput == Axis.X)
            return !isForMove ? Input.mousePosition.x - _originInput : -(Input.mousePosition.x - _originInput);
        if (axisInput == Axis.Y)
            return Input.mousePosition.y - _originInput;
        return 0f;
    }
}