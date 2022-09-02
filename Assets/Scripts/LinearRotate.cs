using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class LinearRotate : LinearInput
{
    public Axis axis = Axis.Y;
    [MinMaxSlider(0, 360)]
    public Vector2 edgeRotate;
    
    
    private int _index;
    private Quaternion _nextRotate;

    private StateItem _stateItem;


    private void Awake()
    {
        _stateItem = GetComponent<StateItem>();
    }

    private void OnEnable()
    {
        UpdateOriginInput();
        
        if (_stateItem.State != StateItems.LinearRotate)
        {
            enabled = false;
            return;
        }
        
        _index = (int)axis;
    }

    private void Update()
    {
        var angle = AngleBetweenVector3(transform.forward, Vector3.up);
        
        _nextRotate[_index] = -GetInputValue();
        if (angle >= edgeRotate.x && _nextRotate[_index] < 0)
        {
            transform.Rotate(_nextRotate[0], _nextRotate[1], _nextRotate[2], Space.World);
        }
        else if (angle <= edgeRotate.y && _nextRotate[_index] > 0)
        {
            transform.Rotate(_nextRotate[0], _nextRotate[1], _nextRotate[2], Space.World);
        }
        
        UpdateOriginInput();
    }

    private void LateUpdate()
    {
        if (_stateItem.State != StateItems.LinearRotate)
        {
            enabled = false;
        }
    }
    
    private float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        var angle = Vector3.SignedAngle(vec1, vec2, Vector3.right);
        angle = (angle < -1) ? -angle : 360f - angle;
        return angle;
    }
}