using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


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

    public GameObject Liquid;
    public GameObject Sediment;
    public GameObject BurnedLiquid;
    public GameObject LiquidFlow;
    public Transform StartLiquidFlow;

    private StateMiniTestTube _stateMiniTestTube;
    private StateItem _stateItem;
    private float _downMiniTube;
    private float _topMiniTube;

    private Renderer _rendererLiquid;
    private Renderer _rendererSediment;
    private Renderer _rendererBurnLiquid;
    private LiquidFlow _liquidFlowScript;

    private int _countLiquid;

    public StateMiniTestTube stateMiniTestTube => _stateMiniTestTube;
    public int CountLiquid => _countLiquid;

    public void SetStateMiniTestTube(int index)
    {
        _stateMiniTestTube = (StateMiniTestTube)index;
    }

    private void Awake()
    {
        _downMiniTube = 0.45f;
        _topMiniTube = 0.55f;

        _rendererLiquid = Liquid.GetComponent<Renderer>();
        _rendererSediment = Sediment.GetComponent<Renderer>();
        _rendererBurnLiquid = BurnedLiquid.GetComponent<Renderer>();
        _liquidFlowScript = LiquidFlow.GetComponent<LiquidFlow>();
        _stateItem = GetComponent<StateItem>();
    }

    private void FixedUpdate()
    {
        PourOutLiquid();
    }

    public void AddLiquid(LiquidDrop liquid)
    {
        _countLiquid++;

        var step = (_topMiniTube - _downMiniTube) / 30f;

        if (liquid.typeLiquid == TypeLiquid.CuSO4)
        {
            var colorCuSO4 = new Color32(liquid.GetColor.r, liquid.GetColor.g, liquid.GetColor.b, 30);

            _rendererLiquid.material.SetColor("_Colour", colorCuSO4);
        }

        if (liquid.typeLiquid == TypeLiquid.NaOH)
        {
            transform.GetChild(1).gameObject.SetActive(true);

            var alphaSediment = new Color32(0, 0, 0, 255 / 4);
            _rendererSediment.material.color += alphaSediment;
        }

        _rendererLiquid.material.SetFloat("_FillAmount", _downMiniTube + step * _countLiquid);

        if (_stateMiniTestTube == StateMiniTestTube.Empty && liquid.typeLiquid == TypeLiquid.CuSO4 && _countLiquid == 8)
        {
            _stateMiniTestTube = StateMiniTestTube.CuSO4;
        }
        else if (liquid.typeLiquid == TypeLiquid.NaOH && _countLiquid > 8)
        {
            _stateMiniTestTube = StateMiniTestTube.CuSo4_NaOH;
        }
    }

    private IEnumerator BurnLiquid()
    {
        var finish = _rendererLiquid.material.GetFloat("_FillAmount");
        var point = _rendererBurnLiquid.material.GetFloat("_FillAmount");

        yield return new WaitForSeconds(2f);

        while (point < finish)
        {
            point += Time.deltaTime * Random.Range(0.001f, 0.01f);
            _rendererBurnLiquid.material.SetFloat("_FillAmount", point);
            yield return null;
        }

        _stateMiniTestTube = StateMiniTestTube.CuSO4_NaOH_Fire;
        Liquid.SetActive(false);
        Sediment.SetActive(false);
    }

    public void StartBurnLiquid()
    {
        StartCoroutine(BurnLiquid());
    }

    public void PourOutLiquid()
    {
        if (_stateItem.State != StateItems.LinearRotate) return;
        
        if (transform.rotation.eulerAngles.x is >= 0f and <= 180f)
        {
            if (!LiquidFlow.activeSelf && _rendererBurnLiquid.material.GetFloat("_FillAmount") > _downMiniTube)
            {
                LiquidFlow.SetActive(true);
                _liquidFlowScript.SetPositionStart(StartLiquidFlow.position);
                _liquidFlowScript.step = 0.0002f;
                _liquidFlowScript.limit = _downMiniTube + ((_topMiniTube - _downMiniTube) / 30f * _countLiquid / 2f);
            }

            if (_liquidFlowScript.stateFlowLiquid == StateFlowLiquid.Pour)
            {
                _liquidFlowScript.PourOutLiquid(_rendererBurnLiquid, "_FillAmount");
            }
        }
        else
        {
            _liquidFlowScript.ChangeStateFlowLiquid(3);
        }
    }
}