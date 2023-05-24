using UnityEngine;
using UnityEngine.Events;

public class MoveMouseItem : MouseItem
{
    [SerializeField] private StateItems _state = StateItems.Drag;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Quaternion startRot;
    [SerializeField] private Vector3 startScale;

    [SerializeField] private bool returnToStartingPosition = true;

    [Space] [HideInInspector] public bool IsMoveRigidbody;
    [HideInInspector] public bool IsExtentsX;
    [HideInInspector] public bool IsExtentsY;
    [HideInInspector] public bool IsExtentsZ;

    [Space] [HideInInspector] public bool IsRotateToCameraOnAwake;
    [HideInInspector] public bool isRotateToCamera;
    [HideInInspector] public bool IsLockX;
    [HideInInspector] public bool IsLockY;
    [HideInInspector] public bool IsLockZ;
    [HideInInspector] public bool IsRight;
    [HideInInspector] public bool IsUp;
    [HideInInspector] public bool IsForward = true;
    [HideInInspector] public bool IsInverse;

    [HideInInspector] public UnityEvent OnMouseDown;
    [HideInInspector] public UnityEvent OnMouseUp;

    [HideInInspector] public OutlineMap outlineMap;

    private LinearValue _linearValue;
    private Vector3 _offsetLinearValue;
    private Rigidbody _rigidbody;

    private Vector3 _hitWall;

    private Vector3 _targetStartPosition;
    private Quaternion _targetStartRotate;
    private MoveToPoint _moveToRespawn;
    private MoveToPoint _moveToMouse;
    private Coroutine _useCoroutine;
    private Collider _collider;
    private Quaternion _originStartRotation;

    public Quaternion StartRotation
    {
        get => startRot;
        set => startRot = value;
    }

    public bool IsReturnToRespawn
    {
        get => returnToStartingPosition;
        set => returnToStartingPosition = value;
    }

    public bool IsRotateToCamera
    {
        get => isRotateToCamera;
        set => isRotateToCamera = value;
    }

    public bool IsUseEventOnMouse { get; set; } = true;

    public Vector3 SetHitWall
    {
        set => _hitWall = value;
    }

    public void SetState(StateItems newState, LinearValue value = default, Vector3 offset = default)
    {
        _state = newState;
        _linearValue = value;
        _offsetLinearValue = offset;
    }

    public void ResetState()
    {
        _state = StateItems.Drag;
        startRot = _originStartRotation;
    }
    
    
    public void ResetPointRespawnByTransform(Transform toTransform)
    {
        if (IsMoveRigidbody)
        {
            _moveToRespawn = new MoveToPoint(transform, toTransform.position,
                toTransform.rotation, toTransform.localScale, _rigidbody);
        }
        else
        {
            _moveToRespawn = new MoveToPoint(transform, toTransform.position,
                toTransform.rotation, toTransform.localScale);
        }

        _moveToRespawn.SetSpeedTRS = new Vector3(15f, 15f, 15f);
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

        _moveToRespawn.SetSpeedTRS = new Vector3(15f, 15f, 15f);
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
        if (!returnToStartingPosition)
        {
            StateItem.ChangeState(StateItems.Idle);
            return;
        }

        StateItem.ChangeState(StateItems.BackToRespawn);

        StartCoroutine(_moveToRespawn.StartAsync(() => { StateItem.ChangeState(StateItems.Idle); }));
    }


    private void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _originStartRotation = startRot;

        if (IsRotateToCameraOnAwake)
            transform.rotation = RotateToCamera();
    }

    private void OnEnable()
    {
        ResetPointRespawn();

        if (IsMoveRigidbody)
        {
            _moveToMouse = new MoveToPoint(transform, default,
                default, default, _rigidbody, _collider);
        }
        else
        {
            _moveToMouse = new MoveToPoint(transform);
        }

        _moveToMouse.SetSpeedTRS = new Vector3(15f, 15f, 15f);
    }

    private void Update()
    {
        _isActive = StateItem.State != StateItems.Idle;

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsReadyToAction) return;

            _targetStartPosition = transform.position + startPos;
            _targetStartRotate = transform.rotation * startRot;

            StateItem.ChangeState(_state, _linearValue, _offsetLinearValue);

            if (OnMouseDown.GetPersistentEventCount() > 0 && IsUseEventOnMouse)
            {
                OnMouseDown.Invoke();
                //return;
            }

            if (_useCoroutine != null)
            {
                StopCoroutine(_useCoroutine);
                _useCoroutine = null;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (StateItem.State is StateItems.Drag or StateItems.BackToMouse
                && _hitWall != Vector3.zero)
            {
                CursorSkin.Instance.UseHold();
                outlineMap.Show(transform.position);
                MoveItem(_hitWall);
            }
            else if (StateItem.State == StateItems.Interacts)
                outlineMap.Clear();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (StateItem.State is StateItems.Interacts or StateItems.Idle || !IsActive) return;
            outlineMap.Clear();
            if (OnMouseUp.GetPersistentEventCount() > 0 && IsUseEventOnMouse)
            {
                OnMouseUp.Invoke();
                return;
            }

            if (_useCoroutine == null)
            {
                _useCoroutine = StartCoroutine(_moveToRespawn.StartAsync(() =>
                {
                    StateItem.ChangeState(StateItems.Idle);
                }));
            }

            BackToRespawn();
        }

        if (!Input.GetMouseButton(0))
        {
            if (StateItem.State is not (StateItems.Interacts or StateItems.Idle or StateItems.BackToRespawn))
                BackToRespawn();
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
        _moveToMouse.SetTargetRotation(IsRotateToCamera ? RotateToCamera() : _targetStartRotate);
        _moveToMouse.Start();

        if (_moveToMouse.Distance < 0.15f)
        {
            StateItem.ChangeState(_state, _linearValue, _offsetLinearValue);
        }
    }

    private Quaternion RotateToCamera()
    {
        var faceAxis = new Vector3
        {
            x = IsRight ? 1 : 0,
            y = IsUp ? 1 : 0,
            z = IsForward ? 1 : 0
        };
        faceAxis = IsInverse ? faceAxis * -1 : faceAxis;
        var lookPos = Camera.main.transform.position - transform.position;
        lookPos.x = (IsLockX) ? 0 : lookPos.x;
        lookPos.y = (IsLockY) ? 0 : lookPos.y;
        lookPos.z = (IsLockZ) ? 0 : lookPos.z;
        var q = Quaternion.LookRotation(lookPos) * Quaternion.FromToRotation(faceAxis, Vector3.forward);
        return q;
    }
}