using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ItemMoveMap
{
    public Transform target;
    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public Vector3 offsetScale;
}


[Serializable]
public struct LinearValue
{
    public Axis axisInput;
    public Axis axis;
    public Vector2 edge;
}

[Serializable]
public class ItemMap
{
    public ItemMoveMap move;
    public StateItems nextState;

    public LinearValue linearMoveValue;

    public LinearValue linearRotateValue;

    public UnityEvent onEventInEnd;

    public ItemMap()
    {
        move = new ItemMoveMap();
    }
}

public class MoveMap : MonoBehaviour
{
    public List<ItemMap> datas;

    private StateItem _stateItem;
    private MoveToPoint _moveToMapPoint;

    private void Awake()
    {
        _stateItem = GetComponent<StateItem>();
    }

    public void StartToMove(int index)
    {
        var data = datas[index].move;

        _moveToMapPoint = new MoveToPoint(transform, data.target.position + data.offsetPos,
            data.target.rotation * data.offsetRot);
        _moveToMapPoint.SetSpeedTRS = new Vector3(10f, 10f, 10f);
        StartCoroutine(MoveToPoint(index));
    }

    private IEnumerator MoveToPoint(int index)
    {
        while (_moveToMapPoint.Distance > 0.001f || _moveToMapPoint.Angle > 0.001f)
        {
            if (_stateItem?.State == StateItems.Idle)
                yield break;

            _moveToMapPoint.Start();

            yield return new WaitForFixedUpdate();
        }

        switch (datas[index].nextState)
        {
            case StateItems.LinearMove:
                _stateItem?.ChangeState(datas[index].nextState, datas[index].linearMoveValue);
                break;
            case StateItems.LinearRotate:
                _stateItem?.ChangeState(datas[index].nextState, datas[index].linearRotateValue);
                break;
            default:
                _stateItem?.ChangeState(datas[index].nextState);
                break;
        }

        datas[index].onEventInEnd?.Invoke();
    }
}