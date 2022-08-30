using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forceps : MonoBehaviour
{
    [SerializeField] private GameObject takeObject;
    private Transform _halfForceps;
    private bool _isTakeInForcep;

    private void OnEnable()
    {
        _halfForceps = transform.GetChild(0);
    }

    public void StartTakeInForceps()
    {
        StartCoroutine(TakeInForceps(true));
    }

    private IEnumerator TakeInForceps(bool isTakeInForcep)
    {
        if (_isTakeInForcep == isTakeInForcep) yield break;

        _isTakeInForcep = isTakeInForcep;
        
        var delta = 0.01f;
        var angle = (Math.Abs(_halfForceps.localRotation.eulerAngles.z) + 4) / (1 / delta);
        angle = (isTakeInForcep) ? angle : -angle;
        for (var i = 0f; i < 1; i += delta)
        {
            _halfForceps.rotation = Quaternion.Euler(_halfForceps.rotation.eulerAngles.x,
                _halfForceps.rotation.eulerAngles.y, _halfForceps.rotation.eulerAngles.z - angle);
            yield return null;
        }
        
        takeObject.SetActive(true);
    }
}