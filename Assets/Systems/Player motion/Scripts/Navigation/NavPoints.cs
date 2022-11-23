using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VirtualLab.PlayerMotion
{
    public class NavPoints : AbstractNavPoints
    {
        [SerializeField] NavPoint[] points;

        //public const int Table = 1;


        public override int pointCount => points.Length;

        public override NavPoint GetPoint(int pointID)
        {
            return points[pointID - 1] != null ? points[pointID - 1] : throw new UnityException("Point ID is out of range");
            // switch (pointID) 
            // {
            // 	case Table:    return points; 
            // 	default: 	   throw new UnityException("Point ID is out of range"); 
            // }
        }
    }
}