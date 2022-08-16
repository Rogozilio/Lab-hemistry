using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using VirtualLab.OutlineNS;

[RequireComponent(typeof(StateItem))]
public class MoveMouseItem : MonoBehaviour
{
    [SerializeField] 
    private bool returnToStartingPosition = true;

    private Outline _outline;

    private Rigidbody _rigidbody;

    private bool _isActive;
    private bool _isReadyToMove;
    private bool _isMouseDownOverItem;
    private Vector3 _hitWall;
    private Vector3 _offsetCollision;
    private MoveToPoint _moveToPoint;
    private StateItem _stateItem;
    private Coroutine _useCoroutine;
    private Collider _collider;

    public bool IsActive => _isActive;

    public Vector3 SetHitWall
    {
        set => _hitWall = value;
    }

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _stateItem = GetComponent<StateItem>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _moveToPoint = new MoveToPoint(transform, transform.position, transform.rotation, transform.localScale);
    }

    private void OnMouseEnter()
    {
        _isReadyToMove = true;
    }

    private void OnMouseDrag()
    {
        if (_stateItem.State == StateItems.Drag
            && _hitWall != Vector3.zero)
        {
            MoveItem(_hitWall);
        }
    }

    private void OnMouseExit()
    {
        _isReadyToMove = false;
        _outline.HideOutline();
    }

    private void OnMouseDown()
    {
        _outline.HideOutline();
        if (_isReadyToMove)
        {
            _isMouseDownOverItem = true;
            _stateItem.ChangeState(StateItems.Drag);
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
        _isMouseDownOverItem = false;
        _stateItem.ChangeState(StateItems.Idle);
        if(_useCoroutine == null)
            _useCoroutine = StartCoroutine(_moveToPoint.Start(2f));
        _isActive = false;
    }

    public void ShowOutline()
    {
        _outline.ShowOutline();
    }

    public void HideOutline()
    {
        _outline.HideOutline();
    }

    private void MoveItem(Vector3 position)
    {
        _offsetCollision = transform.position - _collider.bounds.min;
        _rigidbody.position = position + new Vector3(0,_offsetCollision.y, 0);
    }
}