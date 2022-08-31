using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Forceps : MonoBehaviour
{
    [SerializeField] private GameObject takeObject;
    private Transform _halfForceps;
    private bool _isTakeInForceps;
    private bool _isMagnesiumBurned;

    public bool IsTakeInForceps => _isTakeInForceps;

    private void OnEnable()
    {
        _halfForceps = transform.GetChild(0);
    }

    public void StartTakeInForceps()
    {
        StartCoroutine(TakeInForceps(true));
    }

    public void StartFireMagnesium()
    {
        StartCoroutine(FireMagnesium());
    }

    private IEnumerator TakeInForceps(bool isTakeInForceps)
    {
        if (_isTakeInForceps == isTakeInForceps) yield break;

        _isTakeInForceps = isTakeInForceps;
        
        var delta = 0.01f;
        var angle = (Math.Abs(_halfForceps.localRotation.eulerAngles.z) + 4) / (1 / delta);
        angle = (isTakeInForceps) ? angle : -angle;
        for (var i = 0f; i < 1; i += delta)
        {
            _halfForceps.rotation = Quaternion.Euler(_halfForceps.rotation.eulerAngles.x,
                _halfForceps.rotation.eulerAngles.y, _halfForceps.rotation.eulerAngles.z - angle);
            yield return null;
        }
        
        takeObject.SetActive(true);
    }
    
    private IEnumerator FireMagnesium()
    {
        if (!_isTakeInForceps || _isMagnesiumBurned) yield break;

        yield return new WaitForSeconds(0.8f);

        var light = takeObject.GetComponent<Light>();
        light.enabled = true;
        light.intensity = 0f;
        var time = 1.5f;
        
        while (light.intensity < 0.15f)
        {
            var delta = 0.007f;
            
            light.intensity += delta;

            yield return new WaitForSeconds(delta);
        }
        
        while (time > 0)
        {
            var delta = 0.01f;
            
            light.intensity = Random.Range(0.01f, 0.1f);

            yield return new WaitForSeconds(delta);
            time -= delta;
        }

        while (light.intensity > 0)
        {
            var delta = 0.001f;
            light.intensity -= delta;

            yield return new WaitForSeconds(delta);
        }

        _isMagnesiumBurned = true;
        light.enabled = false;
    }
}