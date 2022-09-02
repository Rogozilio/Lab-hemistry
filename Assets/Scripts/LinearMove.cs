using System;
using System.Collections;
using System.Collections.Generic;
using GD.MinMaxSlider;
using UnityEditor;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}

public class LinearMove : LinearInput
{
    public Axis axis = Axis.Y;
    
    [MinMaxSlider(-100, 100)]public Vector2 EdgeMove;

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
        _index = (int)axis;
    }

    private void Update()
    {
        var range =  transform.position - _startPoint;
        if (range[_index] > EdgeMove.x && GetInputValue() < 0)
        {
            _nextPosition[_index] = GetInputValue() / 200f;
            transform.position += _nextPosition;
            UpdateOriginInput();
        }
        if (range[_index] < EdgeMove.y && GetInputValue() > 0)
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