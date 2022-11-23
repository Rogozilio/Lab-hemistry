using System;
using System.Collections;
using System.Collections.Generic;
using Cursor;
using UnityEngine;
public enum Axis
{
    X,
    Y,
    Z,
    localX,
    localY,
    localZ
}

public enum AxisInput
{
    X,
    Y,
    InvertX,
    InvertY
}
public class LinearInput : MonoBehaviour
{
    private float _originInput;

    public AxisInput axisInput;

    public float GetOriginInput => _originInput;

    protected void OnEnable()
    {
        switch (axisInput)
        {
            case AxisInput.X:
            case AxisInput.InvertX:
                CursorSkin.Instance?.UseHorizontal();
                break;
            case AxisInput.Y:
            case AxisInput.InvertY:
                CursorSkin.Instance?.UseVertical();
                break;
        }
    }

    // Update is called once per frame
    public void UpdateOriginInput()
    {
        switch (axisInput)
        {
            case AxisInput.X or AxisInput.InvertX:
                _originInput = Input.mousePosition.x;
                break;
            case AxisInput.Y or AxisInput.InvertY:
                _originInput = Input.mousePosition.y;
                break;
        }
    }

    public float GetInputValue()
    {
        switch (axisInput)
        {
            case AxisInput.X: 
                return Input.mousePosition.x - _originInput;
            case AxisInput.Y:
                return Input.mousePosition.y - _originInput;
            case AxisInput.InvertX:
                return (Input.mousePosition.x - _originInput) * -1;
            case AxisInput.InvertY:
                return (Input.mousePosition.y - _originInput) * -1;
            default:
                return 0f;
        }
    }
}