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
    

    public StateFlowLiquid stateFlowLiquid;
    public float flowSpeed = 1f;
    public float step;
    public float limit;

    public void SetPositionStart(Vector3 start)
    {
        transform.position = start;
    }

    public void ChangeStateFlowLiquid(int index)
    {
        stateFlowLiquid = (StateFlowLiquid)index;
    }

    public void PourOutLiquid(Renderer liquid, string name = "_Color")
    {
        var volumeLiquid = liquid.material.GetFloat(name);
        
        if (volumeLiquid > limit)
            liquid.material.SetFloat(name, volumeLiquid - step);
        else
            stateFlowLiquid = StateFlowLiquid.EndPour;
    }

    public void PourInLiquid(Renderer liquid)
    {
        if(stateFlowLiquid != StateFlowLiquid.Pour) return;
        
        var prevFillAmount = liquid.material.GetFloat("_FillAmount");
        liquid.material.SetFloat("_FillAmount", prevFillAmount + step);
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