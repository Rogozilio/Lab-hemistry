using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectTransform : MonoBehaviour
{
    struct TransformInfo
    {
        private readonly Transform _transform;
        private Vector3 _prevPosition;
        private Quaternion _prevRotation;
        private Quaternion _prevLocalRotation;
        private Vector3 _prevUp;
        private Vector3 _prevRight;
        private Vector3 _prevForward;

        public Transform transform => _transform;
        public Vector3 prevPosition => _prevPosition;
        public Quaternion prevRotation => _prevRotation;
        public Quaternion prevLocalRotation => _prevLocalRotation;
        public Vector3 prevUp => _prevUp;
        public Vector3 prevRight => _prevRight;
        public Vector3 prevForward => _prevForward;

        public bool IsMove => _prevPosition != _transform.position;
        public bool IsRotate => _prevRotation != _transform.rotation;
        public bool IsMoveOrRotate => IsMove || IsRotate;

        public TransformInfo(Transform transform)
        {
            _transform = transform;
            _prevPosition = transform.position;
            _prevRotation = transform.rotation;
            _prevLocalRotation = transform.localRotation;
            _prevUp = transform.up;
            _prevRight = transform.right;
            _prevForward = transform.forward;
        }

        public void RefreshPrevPosition()
        {
            _prevPosition = _transform.position;
        }

        public void RefreshPrevRotation()
        {
            _prevRotation = _transform.rotation;
        }

        public void RefreshPrevLocalRotation()
        {
            _prevLocalRotation = _transform.localRotation;
        }

        public void RefreshPrevUp()
        {
            _prevUp = _transform.up;
        }

        public void RefreshPrevRight()
        {
            _prevRight = _transform.right;
        }

        public void RefreshPrevForward()
        {
            _prevForward = _transform.forward;
        }

        public void RefreshAll()
        {
            RefreshPrevPosition();
            RefreshPrevRotation();
            RefreshPrevLocalRotation();
            RefreshPrevUp();
            RefreshPrevRight();
            RefreshPrevForward();
        }
    }

    public Transform target;

    private bool _isChangeCurrentObject;
    private bool _isChangeTargetObject;

    private TransformInfo _currentObject;
    private TransformInfo _targetObject;

    private void OnEnable()
    {
        _currentObject = new TransformInfo(transform);
        _targetObject = new TransformInfo(target);
    }

    private void Update()
    {
        IsObjectMove();
    }

    private void IsObjectMove()
    {
        if (_currentObject.IsMoveOrRotate)
        {
            if (_currentObject.IsMove)
                MoveTo(_currentObject, _targetObject);
            if (_currentObject.IsRotate)
                RotateTo(_currentObject, _targetObject);
        }
        else if (_targetObject.IsMoveOrRotate)
        {
            if (_targetObject.IsMove)
                MoveTo(_targetObject, _currentObject);
            if (_targetObject.IsRotate)
                RotateTo(_targetObject, _currentObject);
        }

        _currentObject.RefreshAll();
        _targetObject.RefreshAll();
    }

    private void MoveTo(TransformInfo moving, TransformInfo stay)
    {
        stay.transform.position = moving.transform.position
                                  + (stay.transform.position - moving.prevPosition);
    }

    private void RotateTo(TransformInfo moving, TransformInfo stay)
    {
        var dir = stay.transform.position - moving.transform.position;
        var rot = moving.transform.rotation * Quaternion.Inverse(moving.prevRotation);
        dir = rot * dir;
        stay.transform.position = moving.transform.position + dir;
        
        stay.transform.Rotate(rot.eulerAngles, Space.World);
    }
}