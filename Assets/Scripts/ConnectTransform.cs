using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ConnectTransform : MonoBehaviour
{
    public Transform target;

    private bool _isChangeCurrentObject;
    private bool _isChangeTargetObject;

    private Vector3 _newPositionCurrentObject;
    private Vector3 _newPositionTargetObject;
    private Quaternion _newRotateCurrentObject;
    private Quaternion _newRotateTargetObject;

    private Vector3 _originTransform;
    private Vector3 _originTarget;

    private float _distance => Vector3.Distance(_originTarget , _originTransform);
    private Vector3 _offset
    {
        get
        {
            if(_isChangeCurrentObject)
                return _originTarget - _originTransform;
            if (_isChangeTargetObject)
                return _originTransform - _originTarget;
            return Vector3.zero;
        }
    }

    private void Awake()
    {
        _newPositionCurrentObject = transform.position;
        _newPositionTargetObject = target.position;
        _newRotateCurrentObject = transform.rotation;
        _newRotateTargetObject = target.rotation;

        _originTransform = transform.position;
        _originTarget = target.position;
    }
    
    private void Update()
    {
        // Debug.Log(Matrix4x4.Rotate(transform.rotation));
        // Debug.Log(transform.localToWorldMatrix);
        // Debug.Log(transform.right);
        // Debug.Log(transform.up);
        // Debug.Log(transform.forward);
        _isChangeCurrentObject = IsObjectMove();
        _isChangeTargetObject = IsObjectMove(true);
        
        // Debug.Log(_isChangeCurrentObject);
        // Debug.Log(_isChangeTargetObject);

        // if (_isChangeCurrentObject)
        // {
        //     MoveTo(transform, target);
        //     _newPositionTargetObject = target.position;
        // }
        // else if (_isChangeTargetObject)
        // {
        //     MoveTo(target, transform);
        //     _newPositionCurrentObject = transform.position;
        // }
    }

    private bool IsObjectMove(bool isTarget = false)
    {
        if (!isTarget)
        {
            if (_newPositionCurrentObject == transform.position &&
                _newRotateCurrentObject == transform.rotation) return false;
            if (_newPositionCurrentObject != transform.position)
                MoveTo(transform, target);
            else if(_newRotateCurrentObject != transform.rotation)
                RotateTo(transform, target);
            _newPositionCurrentObject = transform.position;
            _newRotateCurrentObject = transform.rotation;
            _newPositionTargetObject = target.position;
            _newRotateTargetObject = target.rotation;
            return true;
        }
        else
        {
            if (_newPositionTargetObject == target.position &&
                _newRotateTargetObject == target.rotation) return false;

            MoveTo(target, transform);
            _newPositionCurrentObject = transform.position;
            _newRotateCurrentObject = transform.rotation;
            _newPositionTargetObject = target.position;
            _newRotateTargetObject = target.rotation;
            return true;
        }

        return false;
    }

    private void MoveTo(Transform moving, Transform stay)
    {
        var point = stay.position - _newPositionCurrentObject;
        point = new Vector3(point.x / moving.localScale.x, point.y / moving.localScale.y,
            point.z / moving.localScale.z);
        stay.position = moving.localToWorldMatrix.MultiplyPoint3x4(point);
    }

    private void RotateTo(Transform moving, Transform stay)
    {
        var point = stay.position - moving.position;
        point = new Vector3(point.x / moving.localScale.x, point.y / moving.localScale.y,
            point.z / moving.localScale.z);
        stay.position = moving.localToWorldMatrix.MultiplyPoint3x4(point);

        stay.rotation = moving.rotation;
    }

    public Vector3 GetRelativePosition(Transform origin, Vector3 position) {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
    
}
