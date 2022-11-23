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
    H2SO4,
    HCI,
    FeCl3,
    NH4CNS,
    Phenolphthalein,
    NiCl2,
    [InspectorName("Bi(NO3)3")]Bi_NO3_3,
    HNO3,
    CrCl3,
    NH4CI,
    Na2CO3,
    KI,
    Pb_NO3_2,
    K2CrO4,
    AgNO3,
    NH4OH,
    MgCI2,
    CH3COOH,
    methylOrange,
    MnSO4,
    [InspectorName("(NH4)2S")]_NH4_2S,
    SbCl3,
    Na2S
}
public class LiquidDrop : MonoBehaviour
{
    private Color32 _color;
    
    [HideInInspector] public TypeLiquid typeLiquid;

    public TypeLiquid TypeLiquid => typeLiquid;
    public Color32 GetColor => GetComponent<SkinnedMeshRenderer>().material.GetColor("_BaseColor");

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