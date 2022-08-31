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
    private MoveToPoint _moveToPoint;
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
        _moveToPoint = new MoveToPoint(transform, transform.position, transform.rotation, transform.localScale);
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
            _useCoroutine = StartCoroutine(_moveToPoint.Start(2f));
        _isActive = false;
    }


    private void MoveItem(Vector3 position)
    {
        _offsetCollision = transform.position - _collider.bounds.min;
        _rigidbody.position = Vector3.MoveTowards(transform.position,
            position + new Vector3(0, _offsetCollision.y, 0), Time.fixedDeltaTime * 5f);
        _rigidbody.rotation =
            Quaternion.RotateTowards(_rigidbody.rotation, _targetStartRotate, Time.fixedDeltaTime * 30f);
        
        if(Vector3.Distance(transform.position, position + new Vector3(0, _offsetCollision.y, 0)) < 0.1f)
            StateItem.ChangeState(StateItems.Drag);
    }
}