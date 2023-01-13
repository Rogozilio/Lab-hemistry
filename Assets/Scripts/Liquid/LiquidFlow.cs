using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StateFlowLiquid
{
    NotPour,
    StarPour,
    Pour,
    EndPour
}

public class LiquidFlow : MonoBehaviour
{
    private Color _colorLiquid;
    private Color _colorSediment;
    private LevelLiquid _liquid;
    private LevelLiquid _sediment;
    
    public StateFlowLiquid stateFlowLiquid;
    public UnityAction actionInEnd;
    public float flowSpeed = 1f;
    public float step;
    public float stepSediment;
    public float howMach;

    public UnityAction SetUniqueActionInEnd
    {
        set
        {
            actionInEnd += value;
            if (actionInEnd.GetInvocationList().Length <= 1) return;
            for (var i = 0; i < actionInEnd.GetInvocationList().Length - 1; i++)
            {
                if (actionInEnd.GetInvocationList()[i].Method == value.Method) actionInEnd -= value;
            }
        }
    }

    public Color SetColor
    {
        set => _colorLiquid = value;
    }

    public Color SetColorSediment
    {
        set => _colorSediment = value;
    }

    public Color SetColorFlow
    {
        set => GetComponent<Renderer>().material.SetColor("_BaseColor", value);
    }

    public LevelLiquid SetLiquid
    {
        set => _liquid = value;
    }

    public LevelLiquid SetSediment
    {
        set => _sediment = value;
    }

    public void SetPositionStart(Vector3 start)
    {
        transform.position = start;
    }

    public void ChangeStateFlowLiquid(int index)
    {
        stateFlowLiquid = (StateFlowLiquid)index;
    }

    public void PourOutLiquid(LevelLiquid levelLiquid, LevelLiquid sediment = null)
    {
        if (howMach > 0)
        {
            levelLiquid.level -= (step < howMach) ? step : howMach;
            if (sediment) sediment.level -= stepSediment;

            PourInLiquid();

            howMach -= step;
        }
        else
        {
            stateFlowLiquid = StateFlowLiquid.EndPour;
        }
    }

    private void PourInLiquid()
    {
        if (!_liquid.gameObject.activeSelf)
            _liquid.gameObject.SetActive(true);

        _liquid.level += (step < howMach) ? step : howMach;

        _liquid.GetComponent<Renderer>().material.SetColor("_LiquidColor", _colorLiquid);

        if (!_sediment) return;

        _sediment.level += stepSediment;
        _sediment.GetComponent<Renderer>().material.SetColor("_LiquidColor", _colorSediment);
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

                    actionInEnd?.Invoke();
                    gameObject.SetActive(false);
                }

                break;
        }
    }
}