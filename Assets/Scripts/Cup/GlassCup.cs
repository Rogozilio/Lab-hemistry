using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassCup : MonoBehaviour
{
    public enum StateGlassCup
    {
        Empty,
        Marble,
        Marble_HCI,
        Marble_HCI_CO2,
        Smoke,
        NotActive
    }

    private StateGlassCup _stateGlassCup = StateGlassCup.Empty;

    private int _countLiquidHCI;

    public float timeReleaseCO2 = 180f;
    public Slider sliderCO2;
    public GameObject Cap;
    public GameObject fewPieceMarble;
    public GameObject powder;
    public GameObject bubble;
    

    public StateGlassCup stateGlassCup => _stateGlassCup;

    public int countLiquidHCI => _countLiquidHCI;

    private IEnumerator Timer()
    {
        var timer = 0f;
        while (timer < timeReleaseCO2)
        {
            timer++;
            sliderCO2.value = timer / timeReleaseCO2;
            yield return new WaitForSeconds(1f);
        }

        sliderCO2.gameObject.SetActive(false);
        bubble.SetActive(false);
        _stateGlassCup = StateGlassCup.Marble_HCI_CO2;
        Cap.GetComponent<ClickMouseItem>().enabled = true;
    }

    public void StartReleaseCO2()
    {
        if (_stateGlassCup == StateGlassCup.Marble_HCI)
        {
            Cap.GetComponent<ClickMouseItem>().enabled = false;
            sliderCO2.gameObject.SetActive(true);
            bubble.SetActive(true);
            StartCoroutine(Timer());
        }
    }
    

    public void ChangeStateGlassCup(int index)
    {
        _stateGlassCup = (StateGlassCup)index;
    }

    public void SetTransformFewPieceMarble(Transform transform)
    {
        fewPieceMarble.SetActive(true);
        fewPieceMarble.transform.position = transform.position;
        fewPieceMarble.transform.rotation = transform.rotation;
    }

    public void AddLiquidHCI()
    {
        _countLiquidHCI++;

        if (!powder.activeSelf)
            powder.SetActive(true);

        powder.transform.localScale = 
            new Vector3(powder.transform.localScale.x, powder.transform.localScale.y, _countLiquidHCI * 0.1f);
    }
}