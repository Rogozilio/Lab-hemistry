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
        if (transform.rotation.eulerAngles.x > 88 
            && transform.rotation.eulerAngles.x < 92
            && !waterDrop.activeSelf && _state.State == StateItems.LinearRotate)
        {
            waterDrop.SetActive(true);
            waterDrop.transform.position = transform.position;
            waterDrop.GetComponent<Rigidbody>().velocity = Vector3.down * (Time.fixedDeltaTime * SpeedWaterDrop);
        }
    }
}