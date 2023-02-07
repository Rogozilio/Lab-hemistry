using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearRotate : LinearInput
{
    public Axis axis = Axis.Y;

    //[MinMaxSlider(0, 360)]
    public Vector2 edgeRotate;

    public Vector3 offsetPosition;

    private int _index;
    private Space _space;
    private Vector3 _newPivot;
    private Vector3 _transformDir;
    private Vector3 _aroundAxis;
    private Vector3 _nextRotate;

    private Vector3 _originDir;

    private StateItem _stateItem;

    public LinearValue linearValue => new LinearValue()
    {
        axis = axis,
        axisInput = axisInput,
        edge = edgeRotate
    };


    private void Awake()
    {
        _stateItem = GetComponent<StateItem>();
    }

    private void OnEnable()
    {
        if (_stateItem.State != StateItems.LinearRotate)
        {
            enabled = false;
            return;
        }

        base.OnEnable();
        UpdateOriginInput();

        RefreshBeginValue();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _originDir);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + _transformDir);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _aroundAxis);
    }

    private void Update()
    {
        Rotate(GetInputValue());

        UpdateOriginInput();
    }

    public void LaunchRotate(float speed)
    {
        StartCoroutine(RotateAround(speed));
    }

    private void Rotate(float speed)
    {
        _nextRotate[_index] = Mathf.Clamp(speed, -10f, 10f);

        if (_nextRotate[_index] == 0) return;

        switch (axis)
        {
            case Axis.localX:
                _transformDir = transform.up;
                GetNextRotateClampEdge();
                break;
            case Axis.localY:
                _transformDir = transform.forward;
                GetNextRotateClampEdge();
                break;
            case Axis.localZ:
                _transformDir = transform.right;
                GetNextRotateClampEdge();
                break;
            default:
                GetNextRotateClampEdge();
                var axis = new Vector3 { [_index] = 1f };
                _transformDir = (Quaternion.AngleAxis(_nextRotate[_index], axis) * _transformDir).normalized;
                break;
        }

        transform.Rotate(_nextRotate[0], _nextRotate[1], _nextRotate[2], _space);

        var dir = _newPivot - transform.position;
        dir = Quaternion.Euler(_space == Space.World ? _nextRotate : -_nextRotate) * dir;
        transform.position = _newPivot - dir;
    }
    private IEnumerator RotateAround(float speed)
    {
        RefreshBeginValue();
        while (_stateItem.State == StateItems.Interacts)
        {
            Rotate(speed);
            yield return new WaitForFixedUpdate();
        }
    }

    private void RefreshBeginValue()
    {
        _newPivot = transform.position + offsetPosition;
        _index = (axis < Axis.localX) ? (int)axis : (int)axis - 3;
        _space = axis > Axis.Z ? Space.Self : Space.World;
        switch (axis)
        {
            case Axis.X:
                _originDir = Vector3.up;
                _transformDir = _originDir;
                _aroundAxis = Vector3.right;
                break;
            case Axis.Y:
                _originDir = Vector3.forward;
                _transformDir = _originDir;
                _aroundAxis = Vector3.up;
                break;
            case Axis.Z:
                _originDir = Vector3.right;
                _transformDir = _originDir;
                _aroundAxis = Vector3.forward;
                break;
            case Axis.localX:
                _originDir = transform.up;
                _aroundAxis = transform.right;
                break;
            case Axis.localY:
                _originDir = transform.forward;
                _aroundAxis = transform.up;
                break;
            case Axis.localZ:
                _originDir = transform.right;
                _aroundAxis = transform.forward;
                break;
        }
    }

    private void GetNextRotateClampEdge()
    {
        var angle = Vector3.SignedAngle(_originDir, _transformDir, _aroundAxis);
        _nextRotate[_index] = Mathf.Clamp(_nextRotate[_index], edgeRotate.x - angle, edgeRotate.y - angle); 
    }

    private void LateUpdate()
    {
        if (_stateItem.State != StateItems.LinearRotate)
        {
            enabled = false;
        }
    }
}