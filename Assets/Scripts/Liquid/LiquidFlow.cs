using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateFlowLiquid
{
    NotPour,
    StarPour,
    Pour,
    EndPour
}

public class LiquidFlow : MonoBehaviour
{
    private Color _colorLiquidOut;
    private Color _colorLiquidIn;

    public StateFlowLiquid stateFlowLiquid;
    public float flowSpeed = 1f;
    public float step;
    public float limit;

    public Color SetColorOut
    {
        set => _colorLiquidOut = value;
    }

    public Color SetColorIn
    {
        set => _colorLiquidIn = value;
    }

    public void SetPositionStart(Vector3 start)
    {
        transform.position = start;
    }

    public void ChangeStateFlowLiquid(int index)
    {
        stateFlowLiquid = (StateFlowLiquid)index;
    }

    public void PourOutLiquid(LevelLiquid levelLiquid)
    {
        if (levelLiquid.levelLiquid > limit)
            levelLiquid.levelLiquid -= step;
        else
            stateFlowLiquid = StateFlowLiquid.EndPour;
    }

    public void PourInLiquid(LevelLiquid levelLiquid)
    {
        if (stateFlowLiquid != StateFlowLiquid.Pour) return;
        
        if(!levelLiquid.gameObject.activeSelf)
            levelLiquid.gameObject.SetActive(true);

        levelLiquid.levelLiquid += step;

        if (_colorLiquidOut != _colorLiquidIn)
        {
            _colorLiquidIn = _colorLiquidOut;
            levelLiquid.GetComponent<Renderer>().material.SetColor("_LiquidColor", _colorLiquidIn);
        }
    }

    private void OnEnable()
    {
        stateFlowLiquid = StateFlowLiquid.StarPour;
    }

    private void FixedUpdate()
    {
        switch (stateFlowLiquid)
        {
            case StateFlowLiquid.StarPour:
                transform.localScale += new Vector3(0, 0, Time.fixedDeltaTime * flowSpeed);
                break;
            case StateFlowLiquid.EndPour:
                transform.position -= new Vector3(0, Time.fixedDeltaTime * flowSpeed, 0);
                transform.localScale -= new Vector3(0, 0, Time.fixedDeltaTime * flowSpeed);
                if (transform.localScale.z <= 0)
                {
                    stateFlowLiquid = StateFlowLiquid.NotPour;
                    gameObject.SetActive(false);
                }

                break;
        }
    }
}