using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Forceps : MonoBehaviour
{
    public enum StateForceps
    {
        NotInForceps,
        InForceps,
        BurnedInForceps
    }

    [SerializeField] private GameObject takeObject;
    private Transform _leftHalfForceps;
    private Transform _rightHalfForceps;
    private StateForceps _stateForceps;
    private StateItem _stateItem;

    private MoveToPoint _rotateToLeftForceps;
    private MoveToPoint _rotateToRightForceps;

    public StateForceps stateForceps => _stateForceps;

    public int targetFrameRate = 60;

    private void OnEnable()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        _leftHalfForceps = transform.GetChild(0);
        _rightHalfForceps = transform.GetChild(1);
        _stateForceps = StateForceps.NotInForceps;
        _stateItem = GetComponent<StateItem>();
        _rotateToLeftForceps = new MoveToPoint(_leftHalfForceps);
        _rotateToRightForceps = new MoveToPoint(_rightHalfForceps);
        _rotateToLeftForceps.SetSpeedTRS = new Vector3(0f, 7f, 0f);
        _rotateToRightForceps.SetSpeedTRS = new Vector3(0f, 7f, 0f);
    }

    public void StartTakeInForceps(int stateForceps)
    {
        if (_stateForceps == (StateForceps)stateForceps) return;

        _stateForceps = (StateForceps)stateForceps;

        var delta = ((StateForceps)stateForceps == StateForceps.InForceps) ? 4 : -4;

        var newRotateLeftForceps = Quaternion.Euler(_leftHalfForceps.rotation.eulerAngles.x,
            _leftHalfForceps.rotation.eulerAngles.y, _leftHalfForceps.rotation.eulerAngles.z + delta);
        var newRotateRightForceps = Quaternion.Euler(_rightHalfForceps.rotation.eulerAngles.x,
            _rightHalfForceps.rotation.eulerAngles.y, _rightHalfForceps.rotation.eulerAngles.z - delta);

        _rotateToLeftForceps.SetTargetPosition(_leftHalfForceps.position);
        _rotateToRightForceps.SetTargetPosition(_rightHalfForceps.position);
        _rotateToLeftForceps.SetTargetRotation(newRotateLeftForceps);
        _rotateToRightForceps.SetTargetRotation(newRotateRightForceps);

        _stateItem.ChangeState(StateItems.Interacts);

        var linearValue = GetComponent<LinearMove>().linearValue;
        linearValue.edge = new Vector2(0, 100);

        StartCoroutine(_rotateToLeftForceps.StartAsync(() =>
        {
            _stateItem.ChangeState(StateItems.LinearMove, linearValue);
        }));
        StartCoroutine(_rotateToRightForceps.StartAsync(() =>
        {
            _stateItem.ChangeState(StateItems.LinearMove, linearValue);
        }));

        takeObject.SetActive((StateForceps)stateForceps == StateForceps.InForceps);
    }

    public void StartFireMagnesium()
    {
        StartCoroutine(FireMagnesium());
    }

    private IEnumerator FireMagnesium()
    {
        if (_stateForceps != StateForceps.InForceps) yield break;

        yield return new WaitForSeconds(0.8f);

        var light = takeObject.GetComponent<Light>();
        var minIntensity = 0.01f;
        var maxIntensity = 0.12f;
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

        _stateForceps = StateForceps.BurnedInForceps;
        light.enabled = false;
    }
}