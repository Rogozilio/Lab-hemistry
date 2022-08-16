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

public class LinearMove : MonoBehaviour
{
    

    [SerializeField] Axis _axis = Axis.Y;
    [SerializeField] private float max = Mathf.Infinity;
    [SerializeField] private float min = Mathf.NegativeInfinity;
    
    private Vector3 _startPoint;

    private float _originY;
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

        _originY = Input.mousePosition.y;
        _startPoint = transform.position;
        _index = (int)_axis;
    }

    private void Update()
    {
        var range =  transform.position - _startPoint;
        if (range[_index] > min && Input.mousePosition.y - _originY < 0)
        {
            _nextPosition[_index] = (Input.mousePosition.y - _originY) / 200f;
            transform.position += _nextPosition;
            _originY = Input.mousePosition.y;
        }
        if (range[_index] < max && Input.mousePosition.y - _originY > 0)
        {
            _nextPosition[_index] = (Input.mousePosition.y - _originY) / 200f;
            transform.position += _nextPosition;
            _originY = Input.mousePosition.y;
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