using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Target
{
   public Vector3 Position { get; }
   public Quaternion Rotation{ get; }
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
      var delta = Time.deltaTime * speed;
      var data = _itemMoveMap;

      while (Vector3.Distance(_transform.position, data.Position) > 0.01)
      {
         _transform.position = Vector3.Lerp(_transform.position, data.Position, delta);
         _transform.rotation = Quaternion.Lerp(_transform.rotation, data.Rotation, delta);
         yield return null;
      }
   }
}
