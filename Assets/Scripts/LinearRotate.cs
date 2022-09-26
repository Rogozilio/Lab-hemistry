using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class LinearRotate : LinearInput
{
    public Axis axis = Axis.Y;
    [MinMaxSlider(0, 360)]
    public Vector2 edgeRotate;

    public Vector3 offsetPosition;
    
    private int _index;
    private Vector3 _newPivot;
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
        _newPivot = transform.position + offsetPosition;
    }

    private void Update()
    {
        var angle = AngleBetweenVector3(transform.forward, Vector3.up);

        _nextRotate[_index] = GetInputValue();
        
        if (_nextRotate[_index] < 0)
        {
            _nextRotate[_index] = GetNextAngleRotate(Quaternion.Inverse(_nextRotate));
            if (_nextRotate[_index] + angle > edgeRotate.y) 
                _nextRotate[_index] = edgeRotate.y - angle;

            var dir = _newPivot - transform.position;
            dir = Quaternion.Euler(_nextRotate[0], _nextRotate[1], _nextRotate[2]) * dir;
            transform.position = _newPivot - dir;
            transform.Rotate(_nextRotate[0], _nextRotate[1], _nextRotate[2], Space.World);
        }
        else if (_nextRotate[_index] > 0)
        {
            _nextRotate[_index] = GetNextAngleRotate(_nextRotate);
            if (angle - _nextRotate[_index] < edgeRotate.x)
                _nextRotate[_index] = angle - edgeRotate.x;
            
            var dir = _newPivot - transform.position;
            dir = Quaternion.Euler(-_nextRotate[0], -_nextRotate[1], -_nextRotate[2]) * dir;
            transform.position = _newPivot - dir;
            transform.Rotate(-_nextRotate[0], -_nextRotate[1], -_nextRotate[2], Space.World);
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

    private float GetNextAngleRotate(Quaternion nextRotateFromInput)
    {
        var pos = transform.forward;
        pos = Quaternion.Euler(nextRotateFromInput[0], nextRotateFromInput[1], nextRotateFromInput[2]) * pos;
        var localAngle = AngleBetweenVector3(transform.forward, pos);
        return (localAngle > 100f) ? 360f - localAngle : localAngle;
    }

    private float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        var angle = Vector3.SignedAngle(vec1, vec2, Vector3.right);
        angle = (angle < -1) ? -angle : 360f - angle;
        return angle;
    }
}