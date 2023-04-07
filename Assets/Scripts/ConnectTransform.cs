using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ConnectTransform : MonoBehaviour
{
    struct TransformInfo
    {
        private readonly Transform _transform;
        private Vector3 _originPosition;
        private Quaternion _originRotation;
        private Vector3 _prevPosition;
        private Quaternion _prevRotation;
        private Quaternion _prevLocalRotation;
        private Vector3 _prevUp;
        private Vector3 _prevRight;
        private Vector3 _prevForward;
        private StateItems _prevState;
        private StateItem _stateItem;

        public Transform transform => _transform;

        public Vector3 originPosition => _originPosition;
        public Vector3 prevPosition => _prevPosition;

        public Quaternion originRotation => _originRotation;
        public Quaternion prevRotation => _prevRotation;
        public Quaternion prevLocalRotation => _prevLocalRotation;

        public Vector3 prevUp => _prevUp;
        public Vector3 prevRight => _prevRight;
        public Vector3 prevForward => _prevForward;
        public StateItems prevState => _prevState;
        public StateItem stateItem => _stateItem;

        public bool IsMove => _prevPosition != _transform.position;
        public bool IsRotate => _prevRotation.eulerAngles != _transform.rotation.eulerAngles;
        public bool IsMoveOrRotate => IsMove || IsRotate;

        public TransformInfo(Transform transform)
        {
            _stateItem = transform.GetComponent<StateItem>();
            _transform = transform;
            _originPosition = transform.position;
            _originRotation = transform.rotation;
            _prevPosition = transform.position;
            _prevRotation = transform.rotation;
            _prevLocalRotation = transform.localRotation;
            _prevUp = transform.up;
            _prevRight = transform.right;
            _prevForward = transform.forward;
            _prevState = _stateItem.State;
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

        public void RefreshPrevState()
        {
            _prevState = _stateItem.State;
        }

        public void RefreshAll()
        {
            RefreshPrevPosition();
            RefreshPrevRotation();
            RefreshPrevLocalRotation();
            RefreshPrevUp();
            RefreshPrevRight();
            RefreshPrevForward();
            RefreshPrevState();
        }
    }

    public Transform target;

    private TransformInfo _currentObject;
    private TransformInfo _targetObject;

    private Vector3 _fromCurrentToTarget;
    private Vector3 _fromTargetToCurrent;

    private Quaternion _prevOriginCT;
    private Quaternion _prevOriginTC;

    public bool IsEnable => enabled;

    public Transform SetTarget
    {
        set => target = value;
    }

    private void OnEnable()
    {
        _currentObject = new TransformInfo(transform);
        _targetObject = new TransformInfo(target);

        _fromCurrentToTarget = target.position - transform.position;
        _fromTargetToCurrent = transform.position - target.position;
    }

    private void Update()
    {
        ChangeState();
        IsObjectMove();
    }

    private void IsObjectMove()
    {
        if (_currentObject.IsMoveOrRotate)
        {
            MoveAndRotate(_currentObject, _targetObject, _fromCurrentToTarget);
        }
        else if (_targetObject.IsMoveOrRotate)
        {
            MoveAndRotate(_targetObject, _currentObject, _fromTargetToCurrent);
        }

        _currentObject.RefreshAll();
        _targetObject.RefreshAll();
    }

    private void ChangeState()
    {
        if (_currentObject.prevState != _currentObject.stateItem.State)
        {
            if (_currentObject.stateItem.State == StateItems.LinearMove)
            {
                _targetObject.stateItem.ChangeState(_currentObject.stateItem.State, GetValueLinearMove(_currentObject));
            }
            else if (_currentObject.stateItem.State == StateItems.LinearRotate)
            {
                _targetObject.stateItem.ChangeState(_currentObject.stateItem.State, GetValueLinearRotate(_currentObject));
            }
            else
                _targetObject.stateItem.ChangeState(_currentObject.stateItem.State);
        }
        else if (_targetObject.prevState != _targetObject.stateItem.State)
        {
            if (_targetObject.stateItem.State == StateItems.LinearMove)
            {
                _currentObject.stateItem.ChangeState(_targetObject.stateItem.State, GetValueLinearMove(_targetObject));
            }
            else if (_targetObject.stateItem.State == StateItems.LinearRotate)
            {
                _currentObject.stateItem.ChangeState(_targetObject.stateItem.State, GetValueLinearRotate(_targetObject));
            }
            else
                _currentObject.stateItem.ChangeState(_targetObject.stateItem.State);
        }
    }

    private LinearValue GetValueLinearMove(TransformInfo transformInfo)
    {
        return new LinearValue()
        {
            axis = transformInfo.transform.GetComponent<LinearMove>().axis,
            axisInput = transformInfo.transform.GetComponent<LinearMove>().axisInput,
            edge = transformInfo.transform.GetComponent<LinearMove>().EdgeMove
        };
        ;
    }

    private LinearValue GetValueLinearRotate(TransformInfo transformInfo)
    {
        return new LinearValue()
        {
            axis = transformInfo.transform.GetComponent<LinearRotate>().axis,
            axisInput = transformInfo.transform.GetComponent<LinearRotate>().axisInput,
            edge = transformInfo.transform.GetComponent<LinearRotate>().edgeRotate
        };
        ;
    }

    private void MoveAndRotate(TransformInfo moving, TransformInfo stay, Vector3 dir)
    {
        var angle = moving.transform.rotation * Quaternion.Inverse(moving.originRotation);
        dir = angle * dir;
        stay.transform.position = moving.transform.position + dir;
        stay.transform.rotation = angle * stay.originRotation;
    }
}