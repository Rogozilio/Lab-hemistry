using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public GameObject Water;
    
    private int _countWaterDrop;

    public void AddWaterDrop()
    {
        _countWaterDrop++;
        if (_countWaterDrop >= 10)
        {
            Water.SetActive(true);
        }
    }
}
