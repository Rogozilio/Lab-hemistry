using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniTestTube : MonoBehaviour
{
    [Serializable]
    public enum StateMiniTestTube
    {
        Empty,
        CuSO4,
        [InspectorName("CuSO4+NaOH")] CuSo4_NaOH,
        [InspectorName("CuSO4+NaOH+Fire")] CuSO4_NaOH_Fire,
        [InspectorName("(CuSO4+NaOH+Fire):2")] CuSO4_NaOH_Fire_half
    }

    private StateMiniTestTube _state;

    private GameObject _bottomWater;
    private GameObject _topWater;
    private GameObject _sediment;
    private MeshRenderer _rendererSediment;

    private int _countLiquid;

    public StateMiniTestTube State => _state;
    public int CountLiquid => _countLiquid;

    private void Awake()
    {
        _bottomWater = transform.GetChild(0).gameObject;
        _topWater = transform.GetChild(1).gameObject;
        _sediment = transform.GetChild(2).gameObject;
        _rendererSediment = _sediment.GetComponent<MeshRenderer>();
    }

    public void AddLiquid(LiquidDrop liquid)
    {
        if (_countLiquid == 0)
            ChangeColor(liquid.GetColor, 180);

        _countLiquid++;

        switch (_countLiquid)
        {
            case 1:
                _bottomWater.SetActive(true);
                _bottomWater.transform.localScale = new Vector3(0.8f, 0.8f, 0.5f);
                _bottomWater.transform.localPosition = new Vector3(0f, 0f, -0.0243f);
                break;
            case 2:
                _bottomWater.transform.localScale = Vector3.one;
                _bottomWater.transform.localPosition = Vector3.zero;
                break;
            default:
                _topWater.SetActive(true);
                _topWater.transform.localScale += new Vector3(0f, 0f, 0.5f);
                _topWater.transform.localPosition += new Vector3(0f, 0f, 0.02049f);
                break;
        }

        var stateLiquid = liquid.typeLiquid;

        if (_state == StateMiniTestTube.Empty && stateLiquid == TypeLiquid.CuSO4 && _countLiquid == 8)
        {
            _state = StateMiniTestTube.CuSO4;
        }
        else if (stateLiquid == TypeLiquid.NaOH && _countLiquid > 8)
        {
            if(_state == StateMiniTestTube.CuSO4)
                ChangeColor(new Color32(42, 164,221, 150), 150, true);
            
            _state = StateMiniTestTube.CuSo4_NaOH;

            GetSediment();
        }
    }

    private void ChangeColor(Color32 newColor, byte alpha, bool isPlusColor = false)
    {
        var rendererBottomWater = _bottomWater.GetComponent<MeshRenderer>();
        var rendererTopWater = _topWater.GetComponent<MeshRenderer>();
        
        
        if (isPlusColor)
        {
            var deltaColor = newColor - rendererBottomWater.material.color;
            rendererBottomWater.material.color += deltaColor;
            rendererTopWater.material.color += deltaColor;
        }
        else
        {
            rendererBottomWater.material.color =
                new Color32(newColor.r, newColor.g, newColor.b, alpha);
            rendererTopWater.material.color =
                new Color32(newColor.r, newColor.g, newColor.b, alpha);
        }
    }

    private void GetSediment()
    {
        if (_sediment.activeSelf)
        {
            var a = _rendererSediment.material.GetFloat("_Smoothness");
            _rendererSediment.material.SetFloat("_Smoothness",
                _rendererSediment.material.GetFloat("_Smoothness") - 0.125f);
        }
        else
        {
            _sediment.SetActive(true);
            foreach (var render in _sediment.transform.GetComponentsInChildren<MeshRenderer>())
            {
                render.material.color = new Color32(7, 31,49, 255);
            }
            _rendererSediment.material.color = new Color32(7, 31,49, 255);
        }
    }
}