using System;
using System.Collections;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;

namespace Mini_test_tube
{
    public class MiniTestTubeScene4Sample6 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS4E6
        {
            Empty,
            AgNO3,
            AgNO3_NaCl,
            AgNO3_NaCl_smooth,
            AgNO3_NaCl_NH4OH,
            AgNO3_NaCl_NH4OH_smooth,
            AgNO3_NaCl_NH4OH_KI,
            AgNO3_NaCl_NH4OH_KI_smooth,
            AgNO3_NaCl_Na2S2O3,
            AgNO3_NaCl_Na2S2O3_smooth,
            AgNO3_NaCl_Na2S2O3_KI,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS4E6> _actionAddLiquid;

        private PlayerMotion playerMotion;

        private StateMiniTestTubeS4E6 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public bool isUsedNH4OH;

        public StateMiniTestTubeS4E6 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS4E6 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            playerMotion = FindObjectOfType<PlayerMotion>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _rendererSediment.material.SetFloat("_SedimentMultiply", 5f);
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS4E6>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.Empty, TypeLiquid.AgNO3, Operator.More, 0,
                StateMiniTestTubeS4E6.AgNO3, (bottleLiquid) =>
                {
                    ChangeColorLiquid(bottleLiquid);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS4E6.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3, TypeLiquid.AgNO3, Operator.Equally, 6,
                () => { _stepStageSystem.NextStep(); });
            byte step_NaCl = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3, TypeLiquid.NaCl, Operator.More, 0,
                StateMiniTestTubeS4E6.AgNO3_NaCl, () =>
                {
                    sediment.level = levelLiquid.level;
                    ChangeColorLiquid(_rendererSediment, new Color32(157, 214, 199, 10), step_NaCl--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3_NaCl, TypeLiquid.NaCl, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level;
                ChangeColorLiquid(_rendererSediment, new Color32(157, 214, 199, 10), step_NaCl--);
                if (step_NaCl == 0)
                {
                    _stepStageSystem.NextStep();
                    _state = StateMiniTestTubeS4E6.AgNO3_NaCl_smooth;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(157, 214, 199, 200), 4f, () =>
                    {
                        _stepStageSystem.NextStep();
                        if (CheckTestTubeWithNH4OH())
                        {
                            _state = StateMiniTestTubeS4E6.AgNO3_NaCl_Na2S2O3;
                        }
                        else
                        {
                            _state = StateMiniTestTubeS4E6.AgNO3_NaCl_NH4OH;
                            isUsedNH4OH = true;
                        }
                    });
                    step_NaCl = 2;
                }
            });
            byte step_NH4OH = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3_NaCl_NH4OH, TypeLiquid.NH4OH, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level;
                ChangeColorLiquid(_rendererSediment, new Color32(157, 214, 199, 50), step_NH4OH--);
                if (step_NH4OH == 0)
                {
                    _stepStageSystem.NextStep();
                    _state = StateMiniTestTubeS4E6.AgNO3_NaCl_NH4OH_smooth;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(157, 214, 199, 0), 2.5f, () =>
                    {
                        _stepStageSystem.NextStep();
                        _state = StateMiniTestTubeS4E6.AgNO3_NaCl_NH4OH_KI;
                    });
                    step_NH4OH = 2;
                }
            });
            byte step_KI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3_NaCl_NH4OH_KI, TypeLiquid.KI, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level / 3;
                _rendererSediment.material.SetFloat("_SedimentMultiply", 2f);
                _rendererSediment.material.SetFloat("_CellDensity", 11.05f);
                ChangeColorLiquid(new Color32(17, 10, 2, 80), step_KI);
                ChangeColorLiquid(_rendererSediment, new Color32(2, 1, 1, 10), step_KI--);
                if (step_KI == 0)
                {
                    _stepStageSystem.NextStep();
                    _state = StateMiniTestTubeS4E6.AgNO3_NaCl_NH4OH_KI_smooth;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(new Color32(17, 10, 2, 158), 3f);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(2, 1, 1, 255), 3f, () =>
                    {
                        _stepStageSystem.NextStep();
                        _state = StateMiniTestTubeS4E6.NotActive;
                        SetStateOtherMiniTestTube(StateMiniTestTubeS4E6.Empty);
                    });
                    step_KI = 2;
                }
            });
            byte step_Na2S2O3 = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3_NaCl_Na2S2O3, TypeLiquid.Na2S2O3, Operator.More, 0,
                () =>
                {
                    sediment.level = levelLiquid.level;
                    ChangeColorLiquid(_rendererSediment, new Color32(157, 214, 199, 50), step_Na2S2O3--);
                    if (step_Na2S2O3 == 0)
                    {
                        _stepStageSystem.NextStep();
                        _state = StateMiniTestTubeS4E6.AgNO3_NaCl_Na2S2O3_smooth;
                        playerMotion.MoveToPoint(transform, 10);
                        StartSmoothlyChangeColor(_rendererSediment, new Color32(157, 214, 199, 0), 2.5f, () =>
                        {
                            _stepStageSystem.NextStep();
                            _state = StateMiniTestTubeS4E6.AgNO3_NaCl_Na2S2O3_KI;
                        });
                        step_Na2S2O3 = 2;
                    }
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E6.AgNO3_NaCl_Na2S2O3_KI, TypeLiquid.KI, Operator.Equally, 12,
                () =>
                {
                    _state = StateMiniTestTubeS4E6.NotActive;
                    _stepStageSystem.NextStep();
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        private void SetStateOtherMiniTestTube(StateMiniTestTubeS4E6 state)
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene4Sample6>();

            foreach (var tube in tubes)
            {
                if (tube != this) tube.SetState = state;
            }
        }

        private bool CheckTestTubeWithNH4OH()
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene4Sample6>();

            foreach (var tube in tubes)
            {
                if (tube.isUsedNH4OH) return true;
            }

            return false;
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS4E6.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
            _rendererSediment.material.SetFloat("_SedimentMultiply", 5f);
            _rendererSediment.material.SetFloat("_CellDensity", 104f);
            isUsedNH4OH = false;
        }
    }
}