using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleWater : MonoBehaviour
{
    public GameObject waterDrop;
    public float SpeedWaterDrop;

    private StateItem _state;

    private void Start()
    {
        _state = GetComponent<StateItem>();
    }

    void FixedUpdate()
    {
        if (transform.rotation.eulerAngles.y > 178 
            && transform.rotation.eulerAngles.y < 182
            && !waterDrop.activeSelf && _state.State == StateItems.LinearRotate)
        {
            Debug.Log("asd");
            waterDrop.SetActive(true);
            waterDrop.transform.position = transform.position;
            waterDrop.GetComponent<Rigidbody>().velocity = Vector3.down * (Time.fixedDeltaTime * SpeedWaterDrop);
        }
    }
}