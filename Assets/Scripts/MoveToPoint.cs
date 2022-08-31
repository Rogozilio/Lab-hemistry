using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Target
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public Vector3 Scale { get; }

    public Target(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}

public class MoveToPoint
{
    private Target _itemMoveMap;
    private Transform _transform;

    public MoveToPoint(Transform from, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        _transform = from;
        _itemMoveMap = new Target(position, rotation, scale);
    }

    public IEnumerator Start(float speed = 1f)
    {
        var data = _itemMoveMap;
        var distance = Vector3.Distance(_transform.position, data.Position);
        var angle = Quaternion.Angle(_transform.rotation, data.Rotation);

        while (distance > 0.01f || angle > 0.01f)
        {
            _transform.position =
                Vector3.MoveTowards(_transform.position, data.Position, distance * Time.fixedDeltaTime * 2f);
            _transform.rotation =
                Quaternion.RotateTowards(_transform.rotation, data.Rotation, angle * Time.fixedDeltaTime * 2f);

            distance = Vector3.Distance(_transform.position, data.Position);
            angle = Quaternion.Angle(_transform.rotation, data.Rotation);
            yield return null;
        }
    }
}