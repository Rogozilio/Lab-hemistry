using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Splinter : MonoBehaviour
{
    public enum StateFireSplinter
    {
        NotFire,
        BeginFire,
        Fire,
        EndFire,
        Smoke
    }

    private StateItem _stateItem;
    private StateFireSplinter _stateFireSplinter = StateFireSplinter.NotFire;

    public StateFireSplinter stateFireSplinter => _stateFireSplinter;

    public ParticleSystem fire;
    public ParticleSystem smoke;

    private void Awake()
    {
        _stateItem = GetComponent<StateItem>();
    }

    private void Update()
    {
        fire.transform.rotation = Quaternion.LookRotation(Vector3.up);

        if (_stateFireSplinter == StateFireSplinter.Fire)
        {
            var value = Random.Range(0.9f, 1f);
            fire.transform.localScale =
                new Vector3(value, value, value);
        }

        if (_stateItem.State == StateItems.Idle && _stateFireSplinter == StateFireSplinter.Fire)
        {
            _stateFireSplinter = StateFireSplinter.EndFire;
            StartCoroutine(PutOutFireSplinter());
        }
    }

    private IEnumerator SetFireSplinter()
    {
        fire.Play();
        _stateFireSplinter = StateFireSplinter.BeginFire;

        var alpha = 0f;
        var scale = 0f;
        var mainFire = fire.main;
        mainFire.startColor = new Color(mainFire.startColor.color.r, mainFire.startColor.color.g,
            mainFire.startColor.color.b, 0);
        fire.transform.localScale = Vector3.zero;
        while (mainFire.startColor.color.a < 1f)
        {
            alpha += Random.Range(0.02f, 0.03f);
            scale += Random.Range(0.02f, 0.03f);

            mainFire.startColor = new Color(mainFire.startColor.color.r, mainFire.startColor.color.g,
                mainFire.startColor.color.b, alpha);
            fire.transform.localScale =
                (fire.transform.localScale.x < 1f) ? new Vector3(scale, scale, scale) : Vector3.one;
            yield return new WaitForFixedUpdate();
        }

        _stateFireSplinter = StateFireSplinter.Fire;
    }

    private IEnumerator PutOutFireSplinter()
    {
        var alpha = 1f;
        var scale = 1f;
        var mainFire = fire.main;
        while (mainFire.startColor.color.a > 0f)
        {
            alpha -= Random.Range(0.02f, 0.03f);
            scale -= Random.Range(0.02f, 0.03f);

            mainFire.startColor = new Color(mainFire.startColor.color.r, mainFire.startColor.color.g,
                mainFire.startColor.color.b, alpha);
            fire.transform.localScale =
                (fire.transform.localScale.x > 0f) ? new Vector3(scale, scale, scale) : Vector3.zero;
            yield return new WaitForFixedUpdate();
        }

        _stateFireSplinter = StateFireSplinter.NotFire;
        fire.Stop();
    }

    public void StartSetFireSplinter()
    {
        StartCoroutine(SetFireSplinter());
    }

    public void StartFillWithSmoke()
    {
        _stateFireSplinter = StateFireSplinter.Smoke;
        StartCoroutine(PutOutFireSplinter());
        smoke.transform.rotation = Quaternion.LookRotation(Vector3.up);
        smoke.Play();
    }
}