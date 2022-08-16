using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace VirtualLab.HandSystem 
{

public class HandSurface : MonoBehaviour
{
    [SerializeField] bool drawGizmos = false; 
    new Camera camera; 



    void Awake () 
    {
        camera = Camera.main; 
    }



    //  Geometry  --------------------------------------------------- 
    public void SetSurface (Vector3 pointOnSurface) 
    {
        Vector3 ab = (pointOnSurface - transform.position).normalized; 
        Vector3 toCamera = (camera.transform.position - transform.position).normalized; 
        Vector3 ac = Vector3.Cross(ab, toCamera); 

        if (ac != Vector3.zero) 
        {
            transform.forward = Vector3.Cross(ac, ab); 
        }
        else 
        {
            transform.forward = toCamera; 
        }
    }

    public Vector3 GetPoint (Vector2 pointScreenSpace) 
    {
        Plane plane = new Plane(transform.forward, transform.position); 
        Ray ray = camera.ScreenPointToRay(pointScreenSpace); 
        
        plane.Raycast(ray, out float enter); 
        return ray.GetPoint(enter); 
    }



    //  Tech  ------------------------------------------------------- 
    void OnDrawGizmos () 
    {
        if (!drawGizmos) return; 

        Gizmos.color = Color.white; 
        DrawMe(); 
    }

    void OnDrawGizmosSelected () 
    {
        if (!drawGizmos) return; 

        Gizmos.color = Color.green; 
        DrawMe(); 
    }

    void DrawMe () 
    {
        // data 
        Vector3 center = transform.position; 
        Vector3 right = transform.right; 
        Vector3 up = transform.up; 

        Vector3 pLD = center - right - up; 
        Vector3 pLU = center - right + up; 
        Vector3 pRD = center + right - up; 
        Vector3 pRU = center + right + up; 

        int sectionsCount = 10; 

        // horizontal lines 
        Vector3 p0 = pLD; 
        Vector3 p1 = pRD; 
        Vector3 step = (pLU - pLD) / sectionsCount; 

        for (int ix = 0; ix <= sectionsCount; ix++) 
        {
            Gizmos.DrawLine(p0, p1); 
            p0 += step; 
            p1 += step; 
        }

        // vertical lines 
        p0 = pLD; 
        p1 = pLU; 
        step = (pRU - pLU) / sectionsCount; 

        for (int ix = 0; ix <= sectionsCount; ix++) 
        {
            Gizmos.DrawLine(p0, p1); 
            p0 += step; 
            p1 += step; 
        }
    }

}

}
