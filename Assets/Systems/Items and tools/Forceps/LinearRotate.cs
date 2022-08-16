using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearRotate : MonoBehaviour
{
    [SerializeField] Axis _axis = Axis.Y;
    [SerializeField] private float from = Mathf.NegativeInfinity;
    [SerializeField] private float to = Mathf.Infinity;
    
    
    private Vector3 _startRotation;
    private Vector3 _nextPosition;
    private Vector3 _axisAngle;

    private float _originY;
    private int _index;
    private Quaternion _nextRotate;

    private StateItem _stateItem;


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

        _originY = Input.mousePosition.y;
        //_startRotation = transform.forward;
        _startRotation = Vector3.right;
        _nextPosition = transform.forward;
        _index = (int)_axis;
        _axisAngle = new Vector3();
        _axisAngle[_index] = 1f;
    }

    private void Update()
    {
        var angle = AngleBetweenVector3(_startRotation, _nextPosition);
        Debug.Log(angle);
        _nextRotate[_index] = (Input.mousePosition.y - _originY);
        _nextPosition = Quaternion.Euler(_axisAngle * angle) * _nextPosition;
        if (angle >= from && angle <= to)
        {
            //transform.Rotate(_nextRotate[0], _nextRotate[1], _nextRotate[2], Space.World);
            transform.Rotate(_nextPosition, Space.World);
        }
        _originY = Input.mousePosition.y;
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
        var angle = Vector3.SignedAngle(vec1, vec2, Vector3.up);
        angle = (angle < -1) ? -angle : 360f - angle; 
        return angle;
    }
}