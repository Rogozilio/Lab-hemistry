using UnityEngine;


public class LinearMove : LinearInput
{
    public Axis axis = Axis.Y;

    public Vector2 EdgeMove;
    public float speed = 1f;

    private Space _space;
    private Vector3 _startPoint;

    private int _index;
    private Vector3 _nextPosition;

    private StateItem _stateItem;

    public LinearValue linearValue => new LinearValue()
    {
        axis = axis,
        axisInput = axisInput,
        edge = EdgeMove
    };

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

        UpdateOriginInput();
        base.OnEnable();
        _startPoint = transform.position;
        _space = (axis < Axis.localX) ? Space.World : Space.Self;
        _index = (axis < Axis.localX) ? (int)axis : (int)axis - 3;
    }

    private void Update()
    {
        var inputDir = GetInputValue();

        if (inputDir == 0) return;

        var distance = (transform.position[_index] > _startPoint[_index])
            ? Vector3.Distance(transform.position, _startPoint)
            : -Vector3.Distance(transform.position, _startPoint);

        _nextPosition[_index] = inputDir / 200f * speed;

        _nextPosition[_index] =
            Mathf.Clamp(_nextPosition[_index], EdgeMove.x - distance, EdgeMove.y - distance);

        transform.Translate(_nextPosition, _space);

        UpdateOriginInput();
    }

    private float GetNextPosition(Vector3 nextPositionFromInput, Vector3 distance)
    {
        return (distance + nextPositionFromInput)[_index];
    }

    private void LateUpdate()
    {
        if (_stateItem.State != StateItems.LinearMove)
        {
            enabled = false;
        }
    }
}