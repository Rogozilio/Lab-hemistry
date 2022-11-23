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
    [SerializeField] private GameObject pieceMagnesium;
    private Transform _leftHalfForceps;
    private Transform _rightHalfForceps;
    private StateForceps _stateForceps;
    private StateItem _stateItem;

    private MoveToPoint _rotateToLeftForceps;
    private MoveToPoint _rotateToRightForceps;

    public StateForceps stateForceps => _stateForceps;

    private void OnEnable()
    {
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

        //takeObject.SetActive((StateForceps)stateForceps == StateForceps.InForceps);
        takeObject.SetActive(true);
    }

    public void EnableLayerWhite()
    {
        _leftHalfForceps.GetChild(0).gameObject.SetActive(true);
        _rightHalfForceps.GetChild(0).gameObject.SetActive(true);
        takeObject.transform.GetChild(2).gameObject.SetActive(true);
        takeObject.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void EnablePieceMagnesium()
    {
        pieceMagnesium.SetActive(true);
        pieceMagnesium.transform.position = takeObject.transform.position;
        pieceMagnesium.transform.forward = Vector3.down;
    }

    public void StartFireMagnesium()
    {
        StartCoroutine(FireMagnesium());
    }

    private IEnumerator FireMagnesium()
    {
        if (_stateForceps != StateForceps.InForceps) yield break;

        yield return new WaitForSeconds(1.3f);

        var smoke = takeObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        var light = takeObject.transform.GetChild(1).GetComponent<Light>();
        var minIntensity = 0.05f;
        var maxIntensity = 0.1f;
        light.enabled = true;
        light.intensity = 0f;
        var time = 2f;

        smoke.Play();

        while (light.intensity < minIntensity)
        {
            var delta = minIntensity / 60f;

            light.intensity += delta;

            yield return new WaitForSeconds(delta);
        }

        GetComponent<MoveMap>().StartToMove(2);

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

        smoke.Stop();

        if (FindObjectOfType<Cup>().IsHaveShavingsPiece)
        {
            GetComponent<MoveMouseItem>().BackToRespawnOrBackToMouse();
        }
        else
        {
            GetComponent<StateItem>().ChangeState(StateItems.LinearMove, new LinearValue()
            {
                axisInput = AxisInput.Y,
                axis = Axis.Y,
                edge = new Vector2(-0.115f, 100f)
            });
        }

        _stateForceps = StateForceps.BurnedInForceps;
        light.enabled = false;
    }
}