using System;
using System.Collections;
using System.Collections.Generic;
using Cursor;
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

    private Vector3 _hitWall;
    private Vector3 _targetStartPosition;
    private Quaternion _targetStartRotate;
    private MoveToPoint _moveToRespawn;
    private MoveToPoint _moveToMouse;
    private Coroutine _useCoroutine;
    private Collider _collider;

    public Vector3 SetHitWall
    {
        set => _hitWall = value;
    }

    public void ResetPointRespawn()
    {
        if (IsMoveRigidbody)
        {
            _moveToRespawn = new MoveToPoint(transform, transform.position,
                transform.rotation, transform.localScale, _rigidbody);
        }
        else
        {
            _moveToRespawn = new MoveToPoint(transform, transform.position,
                transform.rotation, transform.localScale);
        }

        _moveToRespawn.SetSpeedTRS = new Vector3(10f, 10f, 10f);
    }


    public void BackToRespawnOrBackToMouse()
    {
        if (Input.GetMouseButton(0))
        {
            StateItem.ChangeState(StateItems.BackToMouse);
        }
        else
        {
            BackToRespawn();
        }
    }

    public void BackToRespawn()
    {
        StateItem.ChangeState(StateItems.BackToRespawn);
        
        StartCoroutine(_moveToRespawn.StartAsync(() =>
        {
            StateItem.ChangeState(StateItems.Idle);
        }));
    }


    private void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
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

        _moveToMouse.SetSpeedTRS = new Vector3(10f, 10f, 10f);
        _moveToRespawn.SetSpeedTRS = new Vector3(10f, 10f, 10f);
        ;
    }

    private void Update()
    {
        _isActive = StateItem.State != StateItems.Idle;

        if (Input.GetMouseButtonDown(0))
        {
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
        }

        if (Input.GetMouseButton(0))
        {
            if (StateItem.State is StateItems.Drag or StateItems.BackToMouse
                && _hitWall != Vector3.zero)
            {
                UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Hold, new Vector2(10, 10), CursorMode.Auto);
                MoveItem(_hitWall);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (StateItem.State == StateItems.Interacts) return;

            UnityEngine.Cursor.SetCursor(CursorSkin.Instance.Arrow, Vector2.zero, CursorMode.Auto);
            
            if (_useCoroutine == null)
            {
                StateItem.ChangeState(StateItems.BackToRespawn);
                
                _useCoroutine = StartCoroutine(_moveToRespawn.StartAsync(() =>
                {
                    StateItem.ChangeState(StateItems.Idle);
                }));
            }
        }
        
        if (!Input.GetMouseButton(0))
        {
            if (StateItem.State is not (StateItems.Interacts or StateItems.Idle))
            {
                BackToRespawn();
            }
        }
    }

    private void MoveItem(Vector3 position)
    {
        var offset = _collider.bounds.min;
        offset += new Vector3(IsExtentsX ? _collider.bounds.extents.x : 0,
            IsExtentsY ? _collider.bounds.extents.y : 0,
            IsExtentsZ ? _collider.bounds.extents.z : 0);
        _moveToMouse.SetOffsetTransform(offset);
        _moveToMouse.SetTargetPosition(position + startPos);
        _moveToMouse.SetTargetRotation(_targetStartRotate);
        _moveToMouse.Start();

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