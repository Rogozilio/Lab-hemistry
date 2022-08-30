using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInput : MonoBehaviour
{
    private float _originInput;

    public Axis axisInput;

    public float GetOriginInput => _originInput;

    // Update is called once per frame
    public void UpdateOriginInput()
    {
        if (axisInput == Axis.X)
            _originInput = Input.mousePosition.x;
        else if (axisInput == Axis.Y)
            _originInput = Input.mousePosition.y;
    }

    public float GetInputValue()
    {
        if (axisInput == Axis.X)
            return Input.mousePosition.x - _originInput;
        if (axisInput == Axis.Y)
            return Input.mousePosition.y - _originInput;
        return 0f;
    }
}
