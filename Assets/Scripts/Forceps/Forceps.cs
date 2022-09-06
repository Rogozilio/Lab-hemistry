using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Forceps : MonoBehaviour
{
    [SerializeField] private GameObject takeObject;
    private Transform _leftHalfForceps;
    private Transform _rightHalfForceps;
    private bool _isTakeInForceps;
    private bool _isMagnesiumBurned;

    private MoveToPoint _rotateToLeftForceps;
    private MoveToPoint _rotateToRightForceps;

    public bool IsTakeInForceps => _isTakeInForceps;

    private void OnEnable()
    {
        _leftHalfForceps = transform.GetChild(0);
        _rightHalfForceps = transform.GetChild(1);
        _rotateToLeftForceps = new MoveToPoint(_leftHalfForceps);
        _rotateToRightForceps = new MoveToPoint(_rightHalfForceps);
    }

    public void StartTakeInForceps(bool isTakeInForceps = true)
    {
        if (_isTakeInForceps == isTakeInForceps)  return;

        _isTakeInForceps = isTakeInForceps;
        
        var delta = (isTakeInForceps) ? 4 : -4;
        
        var newRotateLeftForceps = Quaternion.Euler(_leftHalfForceps.rotation.eulerAngles.x,
            _leftHalfForceps.rotation.eulerAngles.y, _leftHalfForceps.rotation.eulerAngles.z + delta);
        var newRotateRightForceps = Quaternion.Euler(_rightHalfForceps.rotation.eulerAngles.x,
            _rightHalfForceps.rotation.eulerAngles.y, _rightHalfForceps.rotation.eulerAngles.z - delta);
        _rotateToLeftForceps.SetTargetPosition(_leftHalfForceps.position);
        _rotateToRightForceps.SetTargetPosition(_rightHalfForceps.position);
        _rotateToLeftForceps.SetTargetRotation(newRotateLeftForceps);
        _rotateToRightForceps.SetTargetRotation(newRotateRightForceps);

        StartCoroutine(_rotateToLeftForceps.StartAsync(2f));
        StartCoroutine(_rotateToRightForceps.StartAsync(2f));
        
        takeObject.SetActive(isTakeInForceps);
    }

    public void StartFireMagnesium()
    {
        StartCoroutine(FireMagnesium());
    }

    private IEnumerator FireMagnesium()
    {
        if (!_isTakeInForceps || _isMagnesiumBurned) yield break;

        yield return new WaitForSeconds(0.8f);

        var light = takeObject.GetComponent<Light>();
        var minIntensity = 0.04f;
        var maxIntensity = 0.22f;
        light.enabled = true;
        light.intensity = 0f;
        var time = 1.25f;

        while (light.intensity < minIntensity)
        {
            var delta = minIntensity / 60f;

            light.intensity += delta;

            yield return new WaitForSeconds(delta);
        }

        while (time > 0)
        {
            var delta = 0.01f;

            light.intensity = Random.Range(minIntensity, maxIntensity);

            yield return new WaitForSeconds(delta);
            time -= delta;
        }

        while (light.intensity > 0)
        {
            var delta = maxIntensity / 200f;
            light.intensity -= delta;

            yield return new WaitForSeconds(delta);
        }

        _isMagnesiumBurned = true;
        light.enabled = false;
    }
}