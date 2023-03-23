using System;
using System.Collections;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;

namespace Mini_test_tube
{
    public class MiniTestTubeScene4Sample4 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS4E4
        {
            Empty,
            CuSO4,
            CuSO4_K4_Fe_CN_6_,
            CuSO4_K4_Fe_CN_6_smooth,
            FeCl3,
            FeCl3_K4_Fe_CN_6_,
            FeCl3_K4_Fe_CN_6_smooth,
            NotActive
        }
        
        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS4E4> _actionAddLiquid;

        private PlayerMotion playerMotion;

        private StateMiniTestTubeS4E4 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public StateMiniTestTubeS4E4 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS4E4 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            playerMotion = FindObjectOfType<PlayerMotion>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _rendererSediment.material.SetFloat("_SedimentMultiply", 3f);
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS4E4>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.Empty, TypeLiquid.CuSO4, Operator.More, 0,
                StateMiniTestTubeS4E4.CuSO4, () =>
                {
                    SetStateOtherMiniTestTube(StateMiniTestTubeS4E4.NotActive);
                    ChangeColorLiquid(new Color32(12, 58, 50, 80));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.CuSO4, TypeLiquid.CuSO4, Operator.Equally, 5,
                () => { _stepStageSystem.NextStep(); });
            byte step_K4_Fe_CN_6_ = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.CuSO4, TypeLiquid.K4_Fe_CN_6_, Operator.More, 0,
                StateMiniTestTubeS4E4.CuSO4_K4_Fe_CN_6_, () =>
                {
                    ChangeColorLiquid(new Color32(13, 3, 2, 80), step_K4_Fe_CN_6_--);
                    ChangeColorLiquid(_rendererSediment, new Color32(5, 1, 1, 0));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.CuSO4_K4_Fe_CN_6_, TypeLiquid.K4_Fe_CN_6_, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level;
                ChangeColorLiquid(new Color32(13, 3, 2, 80), step_K4_Fe_CN_6_--);
                _rendererSediment.material.SetFloat("_SedimentMultiply", 5f);
                if (step_K4_Fe_CN_6_ == 0)
                {
                    _stepStageSystem.NextStep();
                    _state = StateMiniTestTubeS4E4.CuSO4_K4_Fe_CN_6_smooth;
                    sediment.level = levelLiquid.level;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(new Color32(13, 3, 2, 200), 5f);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(5, 1, 1, 255), 5f, () =>
                    {
                        _stepStageSystem.NextStep();
                        _state = StateMiniTestTubeS4E4.NotActive;
                        SetStateOtherMiniTestTube(StateMiniTestTubeS4E4.FeCl3);
                    });
                    step_K4_Fe_CN_6_ = 3;
                }
            });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.FeCl3, TypeLiquid.FeCl3, Operator.Equally, 6,
                () => { _stepStageSystem.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.FeCl3, TypeLiquid.FeCl3, Operator.More, 0, 
                () => { ChangeColorLiquid(new Color32(87, 77, 13, 103)); });
            byte step_K4_Fe_CN_6_2 = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.FeCl3, TypeLiquid.K4_Fe_CN_6_, Operator.More, 0, 
                StateMiniTestTubeS4E4.FeCl3_K4_Fe_CN_6_, () =>
                {
                    ChangeColorLiquid(new Color32(9, 12, 32, 255), step_K4_Fe_CN_6_2--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E4.FeCl3_K4_Fe_CN_6_, TypeLiquid.K4_Fe_CN_6_, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(new Color32(9, 12, 32, 255), step_K4_Fe_CN_6_2);
                    ChangeColorLiquid(_rendererSediment, new Color32(0, 1, 8, 0), step_K4_Fe_CN_6_2--);
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 6f);
                    if (step_K4_Fe_CN_6_2 == 0)
                    {
                        _stepStageSystem.NextStep();
                        _state = StateMiniTestTubeS4E4.FeCl3_K4_Fe_CN_6_smooth;
                        sediment.level = levelLiquid.level;
                        playerMotion.MoveToPoint(transform, 10);
                        StartSmoothlyChangeColor(_rendererSediment, new Color32(0, 1, 8, 255), 
                            5f, () =>
                        {
                            _stepStageSystem.NextStep();
                            _state = StateMiniTestTubeS4E4.NotActive;
                        });
                        step_K4_Fe_CN_6_2 = 3;
                    }
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        private void SetStateOtherMiniTestTube(StateMiniTestTubeS4E4 state)
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene4Sample4>();

            foreach (var tube in tubes)
            {
                if (tube != this) tube.SetState = state;
            }
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS4E4.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
            _rendererSediment.material.SetFloat("_SedimentMultiply", 3f);
        }
    }
}