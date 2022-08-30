using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}

public class LinearMove : LinearInput
{
    

    [SerializeField] Axis _axis = Axis.Y;
    [SerializeField] private float max = Mathf.Infinity;
    [SerializeField] private float min = Mathf.NegativeInfinity;
    
    private Vector3 _startPoint;
    
    private int _index;
    private Vector3 _nextPosition;

    private StateItem _stateItem;


    private void Awake()
    {
        _stateItem = GetComponent<StateItem>();
    }

    private void OnEnable()
    {
        if (_stateItem.State != StateItems.LinearMove)
        {
            enabled = false;
            return;
        }

        UpdateOriginInput();
        _startPoint = transform.position;
        _index = (int)_axis;
    }

    private void Update()
    {
        var range =  transform.position - _startPoint;
        if (range[_index] > min && GetInputValue() < 0)
        {
            _nextPosition[_index] = GetInputValue() / 200f;
            transform.position += _nextPosition;
            UpdateOriginInput();
        }
        if (range[_index] < max && GetInputValue() > 0)
        {
            _nextPosition[_index] = GetInputValue() / 200f;
            transform.position += _nextPosition;
            UpdateOriginInput();
        }
    }

    private void LateUpdate()
    {
        if (_stateItem.State != StateItems.LinearMove)
        {
            enabled = false;
        }
    }
}