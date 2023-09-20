using System;
using UnityEngine;

public class SpiritLamp : MonoBehaviour
{
    public ParticleSystem particleSystemFire;
    private bool _isBurn;

    public bool isBurn
    {
        get => _isBurn;
        set
        { 
            _isBurn = value;
            if(value)
                particleSystemFire.Play();
            else
                particleSystemFire.Stop();
        }     
    }

    private void OnEnable()
    {
        if(_isBurn) particleSystemFire.Play();
    }
}
