using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using VirtualLab.OutlineNS;

public class MoveMouseItem : MouseItem
{
    [SerializeField] 
    private bool returnToStartingPosition = true;
    
    private Rigidbody _rigidbody;

    private bool _isActive;
    private Vector3 _hitWall;
    private Vector3 _offsetCollision;
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
        if (StateItem.State == StateItems.Drag
            && _hitWall != Vector3.zero)
        {
            MoveItem(_hitWall);
        }
    }

    private void OnMouseDown()
    {
        base.OnMouseDown();
        
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
        
        if(_useCoroutine == null)
            _useCoroutine = StartCoroutine(_moveToPoint.Start(2f));
        _isActive = false;
    }

    

    private void MoveItem(Vector3 position)
    {
        _offsetCollision = transform.position - _collider.bounds.min;
        _rigidbody.position = position + new Vector3(0,_offsetCollision.y, 0);
    }
}