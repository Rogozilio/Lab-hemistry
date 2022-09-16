using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TypeLiquid
{
    H2O,
    CuSO4,
    NaOH,
    H2SO4
}
public class LiquidDrop : MonoBehaviour
{
    private Color32 _color;
    
    [HideInInspector] public TypeLiquid typeLiquid;

    public Color32 GetColor => _color;

    private void Awake()
    {
        _color = GetComponent<SkinnedMeshRenderer>().material.color;
    }

    private void OnEnable()
    {
        StartCoroutine(LifeWaterDrop(2f));
    }

    private IEnumerator LifeWaterDrop(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
