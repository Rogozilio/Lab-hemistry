using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public GameObject Water;
    public GameObject Magnesium;
    
    private int _countWaterDrop;

    public int CountWaterDrop => _countWaterDrop;
    public bool IsHaveShavingsPiece => Magnesium.activeSelf;

    public void AddWaterDrop()
    {
        _countWaterDrop++;
        if (_countWaterDrop >= 10)
        {
            Water.SetActive(true);
        }
    }

    public void AddPieceMagnesium(Transform target)
    {
        Magnesium.SetActive(true);
        Magnesium.transform.position = target.position;
        Magnesium.transform.rotation = target.rotation;
        Magnesium.transform.localScale = target.localScale;
    }
}
