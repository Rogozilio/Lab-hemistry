using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleLiquid : MonoBehaviour
{
    [SerializeField] private TypeLiquid _typeLiquid;
    public GameObject liquidDrop;
    public Transform StartLiquidDrop;
    public float SpeedLiquidDrop;

    private Rigidbody _rigidbodyLiquid;
    private MeshRenderer _rendererLiquid;
    private LiquidDrop _liquidDropScript;
    private Color32 _color;

    public TypeLiquid TypeLiquid => _typeLiquid;
    public Color32 Color => _color;

    private StateItem _state;

    private void Awake()
    {
        _state = GetComponent<StateItem>();
        _rigidbodyLiquid = liquidDrop.GetComponent<Rigidbody>();
        _liquidDropScript = liquidDrop.GetComponent<LiquidDrop>();
        liquidDrop.GetComponent<SkinnedMeshRenderer>().material.color =
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
    }

    void FixedUpdate()
    {
        if (transform.rotation.eulerAngles.x > 88
            && transform.rotation.eulerAngles.x < 92
            && !liquidDrop.activeSelf && _state.State == StateItems.LinearRotate)
        {
            liquidDrop.SetActive(true);
            _liquidDropScript.typeLiquid = _typeLiquid;
            liquidDrop.transform.position = StartLiquidDrop.position;
            _rigidbodyLiquid.velocity = Vector3.down * (Time.fixedDeltaTime * SpeedLiquidDrop);
        }
    }
}