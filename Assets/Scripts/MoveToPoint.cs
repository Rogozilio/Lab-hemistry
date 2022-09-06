using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Target
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }

    public Target(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}

public class MoveToPoint
{
    private Transform _transform;
    private Target _toPoint;

    private Vector3 _offsetTransform;

    private float _distance =>
        (_collider)
            ? Vector3.Distance(_rigidbody.position,
                _toPoint.Position + new Vector3(0, _rigidbody.position.y - _collider.bounds.min.y, 0))
            : Vector3.Distance(_transform.position, _toPoint.Position);

    private float _angle => Quaternion.Angle(_transform.rotation, _toPoint.Rotation);
    private bool _isMoveNext => _distance > 0.01f || _angle > 0.01f;

    private Rigidbody _rigidbody;
    private Collider _collider;

    public float Distance => _distance;
    public float Angle => _angle;

    public void SetOffsetTransform(Vector3 offsetTransform)
    {
        _offsetTransform = (_rigidbody == default) ? Vector3.zero : offsetTransform;
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        _toPoint.Position = newPosition;
    }

    public void SetTargetRotation(Quaternion newRotation)
    {
        _toPoint.Rotation = newRotation;
    }

    public void SetTargetScale(Vector3 newScale)
    {
        _toPoint.Scale = newScale;
    }

    public MoveToPoint(Transform from, Vector3 toPosition = default, Quaternion toRotation = default,
        Vector3 toScale = default, Rigidbody rigidbody = null, Collider collider = null)
    {
        _transform = from;
        _toPoint = new Target(toPosition, toRotation, toScale);
        _rigidbody = rigidbody;
        _collider = collider;
    }


    private void MoveTo(float speed = 1f)
    {
        var transformPos = _offsetTransform == default ? _transform.position : _offsetTransform;
        var newPosition =
            Vector3.MoveTowards(transformPos, _toPoint.Position,
                _distance * speed * Time.fixedDeltaTime);
        var newRotation =
            Quaternion.RotateTowards(_transform.rotation, _toPoint.Rotation, _angle * speed * Time.fixedDeltaTime);
        
        if (_rigidbody)
        {
            _rigidbody.position = _offsetTransform == default
                ? newPosition
                : newPosition + _rigidbody.position - _offsetTransform;
            _rigidbody.rotation = newRotation;
        }
        else
        {
            _transform.position = _offsetTransform == default
                ? newPosition
                : newPosition + _transform.position - _offsetTransform;;
            _transform.rotation = newRotation;
        }
    }

    public IEnumerator StartAsync(float speed = 1f)
    {
        while (_isMoveNext)
        {
            MoveTo(speed);

            yield return null;
        }
    }

    public void Start(float speed = 1f)
    {
        MoveTo(speed);
    }
}