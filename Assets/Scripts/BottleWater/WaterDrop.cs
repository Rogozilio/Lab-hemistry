using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(LifeWaterDrop(2f));
    }

    private IEnumerator LifeWaterDrop(float seconde)
    {
        yield return new WaitForSeconds(seconde);
        gameObject.SetActive(false);
    }
    
    
}
