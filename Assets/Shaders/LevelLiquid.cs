using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OriginLevelLiquid
{
    Down,
    Center
}

public class LevelLiquid : MonoBehaviour
{
    private Vector3 _size;
    private Vector3 _originPlane;
    private Renderer _rendererTube;

    public Renderer Surface;
    public Transform Plane;
    public OriginLevelLiquid originLevelLiquid = OriginLevelLiquid.Center;
    public Vector3 originOffset;
    [Range(0, 1)] public float levelLiquid = 0.5f;

    public float size => levelLiquid;

    private void Awake()
    {
        _rendererTube = GetComponent<Renderer>();

        switch (originLevelLiquid)
        {
            case OriginLevelLiquid.Center:
                _size = GetComponent<MeshFilter>().mesh.bounds.extents;
                break;
            case OriginLevelLiquid.Down:
                _size = GetComponent<MeshFilter>().mesh.bounds.size;
                break;
        }

        _originPlane = Plane.transform.localPosition + originOffset;
        Surface.material.SetColor("_BaseColor", GetComponent<Renderer>().material.GetColor("_LiquidColor"));
    }

    // Update is called once per frame
    void Update()
    {
        var rotateParent = transform.parent.transform.localRotation;
        var level = (originLevelLiquid == OriginLevelLiquid.Center) ? -1 + (2 * levelLiquid) : levelLiquid;

        var signX = Math.Floor(rotateParent.eulerAngles.z / 180) % 2 == 0 ? 1 : -1;
        var signY = Math.Floor(rotateParent.eulerAngles.x / 180) % 2 == 0 ? 1 : -1;
        var signZ = Vector3.Dot(transform.parent.transform.forward, Vector3.up) >= 0 ? _size.z : -_size.z;

        var x = Vector3.Dot(transform.parent.transform.up, Vector3.forward) * _size.x * signX;
        var y = Vector3.Dot(transform.parent.transform.right, -Vector3.right) * _size.y * signY;
        var z = Vector3.Dot(transform.parent.transform.forward, Vector3.up) * _size.z;

        var center = new Vector3(x, y, z);

        var offset = center * level;
        Plane.localPosition = _originPlane + offset;
        _rendererTube.material.SetVector("_PlanePos", Plane.position);

        Plane.rotation = Quaternion.LookRotation(Vector3.forward);

        Surface.transform.position =
            new Vector3(Surface.transform.position.x, Plane.position.y, Surface.transform.position.z);
        Surface.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}