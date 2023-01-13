using System;
using System.Collections;
using System.Collections.Generic;
using Liquid;
using Mini_test_tube;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


public class MiniTestTubeScene1Sample1_2 : MiniTestTube, IRestart
{
    [Serializable]
    public enum StateMiniTestTubeS1E1_2
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

    public LevelLiquid Sediment;
    public GameObject BurnedLiquid;

    private StateMiniTestTubeS1E1_2 _state;

    private Color _originColorSediment;

    private Renderer _rendererSediment;
    private Renderer _rendererBurnLiquid;
    private LevelLiquid _levelBurnLiquid;
    private ActionAddLiquid<StateMiniTestTubeS1E1_2> _actionAddLiquid;

    private int _countNaOH;
    private int _countH2SO4;

    public StateMiniTestTubeS1E1_2 GetState => _state;
    public int CountLiquid => _countLiquid;
    public int CountH2SO4 => _countH2SO4;
    public int CountNaOH => _countNaOH;

    public override void SetStateMiniTestTube(int index)
    {
        _state = (StateMiniTestTubeS1E1_2)index;
    }

    private void Awake()
    {
        base.Awake();
        
        liquidFlowScript.SetUniqueActionInEnd = () => { _stepStageSystem.NextStep(); };
        _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E1_2>();
        _rendererSediment = Sediment.GetComponent<Renderer>();
        _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
        _rendererBurnLiquid = BurnedLiquid.GetComponent<Renderer>();
        _levelBurnLiquid = BurnedLiquid.GetComponent<LevelLiquid>();

        _actionAddLiquid.AddAction(
            StateMiniTestTubeS1E1_2.Empty, TypeLiquid.CuSO4, Operator.More, 0,
            StateMiniTestTubeS1E1_2.CuSO4, () =>
            {
                ChangeColorLiquid(new Color32(12, 58, 50, 80));
            });
        _actionAddLiquid.AddAction(
            StateMiniTestTubeS1E1_2.CuSO4, TypeLiquid.CuSO4, Operator.Equally, 8,
             () =>
            {
                _stepStageSystem.NextStep();
            });
        byte stepNaOH = 4;
        _actionAddLiquid.AddAction(StateMiniTestTubeS1E1_2.CuSO4, TypeLiquid.NaOH, Operator.More, 8,
            StateMiniTestTubeS1E1_2.CuSo4_NaOH, () =>
            {
                Sediment.level = _levelLiquid.level / 2f;
                _rendererSediment.material.SetFloat("_SidmentMultiply", 3f);
                ChangeColorLiquid(_rendererSediment, new Color32(2, 25, 51, 255), stepNaOH--);
            });
        _actionAddLiquid.AddAction(StateMiniTestTubeS1E1_2.CuSo4_NaOH, TypeLiquid.NaOH, Operator.LessEquals, 12,
            StateMiniTestTubeS1E1_2.CuSo4_NaOH,
            () =>
            {
                Sediment.level = _levelLiquid.level / 2f;
                ChangeColorLiquid(_rendererSediment,new Color32(2, 25, 51, 255), stepNaOH--);
                if (stepNaOH == 0)
                {
                    _stepStageSystem.NextStep();
                    stepNaOH = 4;
                }
            });
        _actionAddLiquid.AddAction(StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half, TypeLiquid.NaOH,
            Operator.More, 0,
            StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half_NaOH, () => { _countNaOH++;});
        _actionAddLiquid.AddAction(StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half_NaOH, TypeLiquid.NaOH,
            Operator.MoreEquals, 0, () => { _countNaOH++; if(_countNaOH == 15) _stepStageSystem.NextStep();});
        byte stepH2SO4 = 15;
        _actionAddLiquid.AddAction(StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half, TypeLiquid.H2SO4,
            Operator.More, 0,
            StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half_H2SO4, () =>
            {
                _countH2SO4++;
                ChangeOtherTestTube(StateMiniTestTubeS1E1_2.NotActive);
                ChangeColorLiquid(new Color(0.53f, 0.65f, 0.86f, 0.04f), stepH2SO4--);
            });
        _actionAddLiquid.AddAction(StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half_H2SO4, TypeLiquid.H2SO4,
            Operator.MoreEquals, 0, () =>
            {
                _countH2SO4++;
                ChangeColorLiquid(new Color(0.53f, 0.65f, 0.86f, 0.04f), stepH2SO4--);
                if (stepH2SO4 == 0)
                {
                    ChangeOtherTestTube(StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire_half_NaOH);
                    _stepStageSystem.NextStep();
                    stepH2SO4 = 15;
                }
            });
    }

    private void FixedUpdate()
    {
        PourOutLiquid(0.005f, _levelBurnLiquid.level / 2f);
    }

    public override void AddLiquid(LiquidDrop liquid)
    {
        base.AddLiquid(liquid);
        
        _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
    }

    private IEnumerator BurnLiquid()
    {
        yield return new WaitForSeconds(2f);

        if (!_levelBurnLiquid.gameObject.activeSelf)
            _levelBurnLiquid.gameObject.SetActive(true);

        while (_levelBurnLiquid.level < levelLiquid.level)
        {
            _levelBurnLiquid.level += Time.fixedDeltaTime * Random.Range(0.01f, 0.05f);
            yield return new WaitForFixedUpdate();
        }

        _state = StateMiniTestTubeS1E1_2.CuSO4_NaOH_Fire;
        _stepStageSystem.NextStep();

        rendererLiquid.material.SetColor("_LiquidColor"
            , _rendererBurnLiquid.material.GetColor("_LiquidColor"));
        BurnedLiquid.SetActive(false);
        Sediment.gameObject.SetActive(false);
    }

    public void StartBurnLiquid()
    {
        StartCoroutine(BurnLiquid());
    }
    
    private void ChangeOtherTestTube(StateMiniTestTubeS1E1_2 newState)
    {
        var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample1_2>();

        foreach (var miniTestTube in miniTestTubes)
        {
            if (miniTestTube == this) continue;
            miniTestTube.SetStateMiniTestTube((int)newState); 
            return;
        }
    }

    public void Restart()
    {
        RestartBase();
        _countNaOH = 0;
        _countH2SO4 = 0;
        Sediment.level = 0;
        _levelBurnLiquid.level = 0;
        _state = StateMiniTestTubeS1E1_2.Empty;
        Sediment.gameObject.SetActive(true);
        _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
    }
}