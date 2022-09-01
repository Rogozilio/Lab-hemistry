using System.Collections;
using System.Collections.Generic;
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
    private Target _toPoint;
    private Transform _transform;

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

    public void SetPosition(Vector3 newPosition)
    {
        _toPoint.Position = newPosition;
    }

    public void SetRotation(Quaternion newRotation)
    {
        _toPoint.Rotation = newRotation;
    }

    public void SetScale(Vector3 newScale)
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
        var newPosition =
            Vector3.MoveTowards(_transform.position, _toPoint.Position, _distance * speed * Time.fixedDeltaTime);
        var newRotation =
            Quaternion.RotateTowards(_transform.rotation, _toPoint.Rotation, _angle * speed * Time.fixedDeltaTime);

        if (_rigidbody)
        {
            _rigidbody.position = newPosition;
            _rigidbody.rotation = newRotation;
            if (_collider)
            {
                var offsetCollision = _toPoint.Position - _collider.bounds.min;
                _rigidbody.position += new Vector3(0, offsetCollision.y, 0);
            }
        }
        else
        {
            _transform.position = newPosition;
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