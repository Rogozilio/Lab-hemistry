using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using VirtualLab.OutlineNS;

public class MoveMouseItem : MouseItem
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Quaternion startRot;
    [SerializeField] private Vector3 startScale;

    [SerializeField] private bool returnToStartingPosition = true;

    private Rigidbody _rigidbody;

    private bool _isActive;
    private Vector3 _hitWall;
    private Vector3 _offsetCollision;
    private Vector3 _targetStartPosition;
    private Quaternion _targetStartRotate;
    private MoveToPoint _moveToRespawn;
    private MoveToPoint _moveToMouse;
    private Coroutine _useCoroutine;
    private Collider _collider;

    public bool IsActive => _isActive;

    public Vector3 SetHitWall
    {
        set => _hitWall = value;
    }

    private void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _moveToRespawn = new MoveToPoint(transform, transform.position,
            transform.rotation, transform.localScale, _rigidbody);
        _moveToMouse = new MoveToPoint(transform, default, 
            default, default, _rigidbody, _collider);
    }

    private void OnMouseDrag()
    {
        if (StateItem.State is StateItems.Drag or StateItems.BackToMouse
            && _hitWall != Vector3.zero)
        {
            MoveItem(_hitWall);
        }
    }

    private void OnMouseDown()
    {
        base.OnMouseDown();

        _targetStartPosition = transform.position + startPos;
        _targetStartRotate = transform.rotation * startRot;

        if (IsReadyToAction)
        {
            StateItem.ChangeState(StateItems.Drag);
            if (_useCoroutine != null)
            {
                StopCoroutine(_useCoroutine);
                _useCoroutine = null;
            }
        }

        _isActive = true;
    }

    private void OnMouseUp()
    {
        StateItem.ChangeState(StateItems.Idle);

        if (_useCoroutine == null)
        {
            _useCoroutine = StartCoroutine(_moveToRespawn.StartAsync(10f));
        }

        _isActive = false;
    }


    private void MoveItem(Vector3 position)
    {
        _offsetCollision = transform.position - _collider.bounds.min;

        _moveToMouse.SetPosition(position);
        _moveToMouse.SetRotation(_targetStartRotate);
        _moveToMouse.Start(10f);

        if (_moveToMouse.Distance < 0.1f)
        {
            StateItem.ChangeState(StateItems.Drag);
        }
    }
}