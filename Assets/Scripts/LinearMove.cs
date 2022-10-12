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
    public float speed = 1f;

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
        base.OnEnable();
        _startPoint = transform.position;
        _index = (int)axis;
    }

    private void Update()
    {
        var range =  transform.position - _startPoint;
        
        if (GetInputValue() > 0)
        {
            _nextPosition[_index] = GetInputValue() / 200f * speed;
            if (GetNextPosition(_nextPosition, range) > EdgeMove.y)
                _nextPosition[_index] = EdgeMove.y - range[_index];
            transform.position += _nextPosition;
        }
        else if (GetInputValue() < 0)
        {
            _nextPosition[_index] = GetInputValue() / 200f * speed;
            if (GetNextPosition(_nextPosition, range) < EdgeMove.x)
                _nextPosition[_index] = EdgeMove.x - range[_index]; 
            transform.position += _nextPosition;
        }
      
        UpdateOriginInput();
    }
    
    private float GetNextPosition(Vector3 nextPositionFromInput, Vector3 range)
    {
        return (range + nextPositionFromInput)[_index];
    }

    private void LateUpdate()
    {
        if (_stateItem.State != StateItems.LinearMove)
        {
            enabled = false;
        }
    }
}