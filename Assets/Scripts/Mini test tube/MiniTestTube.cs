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
        [InspectorName("(CuSO4+NaOH+Fire):2")] CuSO4_NaOH_Fire_half,
        [InspectorName("(CuSO4+NaOH+Fire):2+NaOH")] CuSO4_NaOH_Fire_half_NaOH,
        [InspectorName("(CuSO4+NaOH+Fire):2+H2SO4")] CuSO4_NaOH_Fire_half_H2SO4,
        NotActive
    }

    public GameObject Liquid;
    public GameObject Sediment;
    public GameObject BurnedLiquid;
    public GameObject LiquidFlow;
    public Transform StartLiquidFlow;

    private StateMiniTestTube _stateMiniTestTube = StateMiniTestTube.Empty;
    private StateItem _stateItem;

    private Renderer _rendererLiquid;
    private LevelLiquid _levelLiquid;
    private Renderer _rendererSediment;
    private Renderer _rendererBurnLiquid;
    private LevelLiquid _levelBurnLiquid;
    private LiquidFlow _liquidFlowScript;

    private int _countLiquid;
    private int _countNaOH;
    private int _countH2SO4;
    private float _step;

    public StateMiniTestTube stateMiniTestTube => _stateMiniTestTube;
    public int CountLiquid => _countLiquid;
    public int CountH2SO4 => _countH2SO4;
    public int CountNaOH => _countNaOH;

    public void SetStateMiniTestTube(int index)
    {
        _stateMiniTestTube = (StateMiniTestTube)index;
    }

    private void Awake()
    {
        _stateItem = GetComponent<StateItem>();
        _rendererLiquid = Liquid.GetComponent<Renderer>();
        _rendererSediment = Sediment.GetComponent<Renderer>();
        _rendererBurnLiquid = BurnedLiquid.GetComponent<Renderer>();
        _levelLiquid = Liquid.GetComponent<LevelLiquid>();
        _levelBurnLiquid = BurnedLiquid.GetComponent<LevelLiquid>();
        _liquidFlowScript = LiquidFlow.GetComponent<LiquidFlow>();
        _step = 1f / 30f;
    }

    private void FixedUpdate()
    {
        PourOutLiquid();
    }

    public void AddLiquid(LiquidDrop liquid)
    {
        _countLiquid++;

        if (liquid.typeLiquid == TypeLiquid.CuSO4)
        {
            var colorCuSO4 = new Color32(liquid.GetColor.r, liquid.GetColor.g, liquid.GetColor.b, 30);
            _rendererLiquid.material.SetColor("_LiquidColor", colorCuSO4);
        }

        if (_stateMiniTestTube == StateMiniTestTube.CuSo4_NaOH && liquid.typeLiquid == TypeLiquid.NaOH && _countLiquid < 12)
        {
            transform.GetChild(1).gameObject.SetActive(true);

            var alphaSediment = new Color32(0, 0, 0, 255 / 4);
            _rendererSediment.material.color += alphaSediment;
        }

        if(!_levelLiquid.gameObject.activeSelf)
            _levelLiquid.gameObject.SetActive(true);
        _levelLiquid.levelLiquid += _step;

        if (_stateMiniTestTube == StateMiniTestTube.Empty && liquid.typeLiquid == TypeLiquid.CuSO4 && _countLiquid == 8)
        {
            _stateMiniTestTube = StateMiniTestTube.CuSO4;
        }
        else if (_stateMiniTestTube == StateMiniTestTube.CuSO4 && liquid.typeLiquid == TypeLiquid.NaOH &&
                 _countLiquid > 8)
        {
            _stateMiniTestTube = StateMiniTestTube.CuSo4_NaOH;
        }
        else if (_stateMiniTestTube == StateMiniTestTube.CuSO4_NaOH_Fire_half && liquid.typeLiquid == TypeLiquid.NaOH)
        {
            _stateMiniTestTube = StateMiniTestTube.CuSO4_NaOH_Fire_half_NaOH;
            _countNaOH++;
        }
        else if (_stateMiniTestTube == StateMiniTestTube.CuSO4_NaOH_Fire_half && liquid.typeLiquid == TypeLiquid.H2SO4)
        {
            _stateMiniTestTube = StateMiniTestTube.CuSO4_NaOH_Fire_half_H2SO4;
            _countH2SO4++;
        }
        else if (_stateMiniTestTube == StateMiniTestTube.CuSO4_NaOH_Fire_half_H2SO4 && _countH2SO4 < 15)
        {
            _countH2SO4++;
            var delta = new Color(0.53f, 0.65f, 0.86f, 0.04f) - _rendererLiquid.material.GetColor("_LiquidColor");
            ChangeColorLiquid(delta / (15 - _countH2SO4), true);
        }
        else if (_stateMiniTestTube == StateMiniTestTube.CuSO4_NaOH_Fire_half_NaOH && _countNaOH < 15)
        {
            _countNaOH++;
            var delta = new Color(0.05f, 0.05f, 0.05f, 1f) - _rendererLiquid.material.GetColor("_LiquidColor");
            ChangeColorLiquid(delta / (15 - _countNaOH), true);
        }
    }

    private void ChangeColorLiquid(Color newColor, bool isPlus = false)
    {
        if (isPlus)
            newColor += _rendererLiquid.material.GetColor("_LiquidColor");

        _rendererLiquid.material.SetColor("_LiquidColor", newColor);
    }

    private IEnumerator BurnLiquid()
    {
        yield return new WaitForSeconds(1.5f);

        if(!_levelBurnLiquid.gameObject.activeSelf)
            _levelBurnLiquid.gameObject.SetActive(true);
        
        while (_levelBurnLiquid.levelLiquid < _levelLiquid.levelLiquid)
        {
            _levelBurnLiquid.levelLiquid += Time.fixedDeltaTime * Random.Range(0.01f, 0.1f);
            yield return new WaitForFixedUpdate();
        }

        _stateMiniTestTube = StateMiniTestTube.CuSO4_NaOH_Fire;

        _rendererLiquid.material.SetColor("_LiquidColor"
            , _rendererBurnLiquid.material.GetColor("_LiquidColor"));
        BurnedLiquid.SetActive(false);
        Sediment.SetActive(false);
    }

    public void StartBurnLiquid()
    {
        StartCoroutine(BurnLiquid());
    }

    private void PourOutLiquid()
    {
        if (_stateItem.State != StateItems.LinearRotate) return;

        if (transform.rotation.eulerAngles.x >= 352f)
        {
            if (!LiquidFlow.activeSelf && _levelBurnLiquid.levelLiquid > 0 &&
                _levelLiquid.levelLiquid > _liquidFlowScript.limit)
            {
                LiquidFlow.SetActive(true);
                _liquidFlowScript.SetColorOut = _rendererLiquid.material.GetColor("_LiquidColor");
                _liquidFlowScript.SetPositionStart(StartLiquidFlow.position);
                _liquidFlowScript.step = 0.005f;
                _liquidFlowScript.limit = _levelBurnLiquid.levelLiquid / 2f;
            }

            if (_liquidFlowScript.stateFlowLiquid == StateFlowLiquid.Pour)
            {
                _liquidFlowScript.PourOutLiquid(_levelLiquid);
            }
        }
        else
        {
            _liquidFlowScript.ChangeStateFlowLiquid(3);
        }
    }
}