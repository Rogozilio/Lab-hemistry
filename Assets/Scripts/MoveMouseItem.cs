using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using VirtualLab.OutlineNS;

public class MoveMouseItem : MouseItem
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Quaternion startRot;
    [SerializeField] private Vector3 startScale;

    [SerializeField] private bool returnToStartingPosition = true;
    [Space] public bool IsMoveRigidbody;
    [HideInInspector] public bool IsExtentsX;
    [HideInInspector] public bool IsExtentsY;
    [HideInInspector] public bool IsExtentsZ;

    private Rigidbody _rigidbody;

    private bool _isActive;
    private Vector3 _hitWall;
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
        if (IsMoveRigidbody)
        {
            _moveToRespawn = new MoveToPoint(transform, transform.position,
                transform.rotation, transform.localScale, _rigidbody);
            _moveToMouse = new MoveToPoint(transform, default,
                default, default, _rigidbody, _collider);
        }
        else
        {
            _moveToRespawn = new MoveToPoint(transform, transform.position,
                transform.rotation, transform.localScale);
            _moveToMouse = new MoveToPoint(transform);
        }
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
        _isActive = false;
        
        if(StateItem.State == StateItems.Interacts) return;
            
        StateItem.ChangeState(StateItems.Idle);

        if (_useCoroutine == null)
        {
            _useCoroutine = StartCoroutine(_moveToRespawn.StartAsync(10f));
        }
    }

    private void LateUpdate()
    {
        if (!_isActive && StateItem.State != StateItems.Interacts)
        {
            StartCoroutine(_moveToRespawn.StartAsync(10f));
            
            StateItem.ChangeState(StateItems.Idle);
        }
    }


    private void MoveItem(Vector3 position)
    {
        var offset = _collider.bounds.min;
        offset += new Vector3(IsExtentsX ? _collider.bounds.extents.x : 0,
            IsExtentsY ? _collider.bounds.extents.y : 0,
            IsExtentsZ ? _collider.bounds.extents.z : 0);
        _moveToMouse.SetOffsetTransform(offset);
        _moveToMouse.SetTargetPosition(position);
        _moveToMouse.SetTargetRotation(_targetStartRotate);
        _moveToMouse.Start(10f);

        if (_moveToMouse.Distance < 0.15f)
        {
            StateItem.ChangeState(StateItems.Drag);
        }
    }
}

[CustomEditor(typeof(MoveMouseItem))]
public class MoveMouseItemEditor : Editor
{
    private bool _isUseBoundCollider = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var moveMouseItem = target as MoveMouseItem;
        if (moveMouseItem.IsMoveRigidbody)
        {
            _isUseBoundCollider = EditorGUILayout.Foldout(_isUseBoundCollider, "Bounds Extension", true);

            if (_isUseBoundCollider)
            {
                var width = 15f;
                var space = 20f;
                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                GUILayout.Label("Use Extents", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                moveMouseItem.IsExtentsX = EditorGUILayout.Toggle(moveMouseItem.IsExtentsX, GUILayout.Width(width));
                GUILayout.Label("X", GUILayout.Width(width));
                moveMouseItem.IsExtentsY = EditorGUILayout.Toggle(moveMouseItem.IsExtentsY, GUILayout.Width(width));
                GUILayout.Label("Y", GUILayout.Width(width));
                moveMouseItem.IsExtentsZ = EditorGUILayout.Toggle(moveMouseItem.IsExtentsZ, GUILayout.Width(width));
                GUILayout.Label("Z", GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
        }
    }
}