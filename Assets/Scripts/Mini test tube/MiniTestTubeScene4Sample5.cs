using System;
using System.Collections;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;

namespace Mini_test_tube
{
    public class MiniTestTubeScene4Sample5 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS4E5
        {
            Empty,
            KMnO4,
            KMnO4_H2SO4,
            KMnO4_H2SO4_K4_Fe_CN_6_,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS4E5> _actionAddLiquid;

        private PlayerMotion playerMotion;

        private StateMiniTestTubeS4E5 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public StateMiniTestTubeS4E5 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS4E5 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            playerMotion = FindObjectOfType<PlayerMotion>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS4E5>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS4E5.Empty, TypeLiquid.KMnO4, Operator.More, 0,
                StateMiniTestTubeS4E5.KMnO4, () => { ChangeColorLiquid(new Color32(104, 2, 76, 107)); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E5.KMnO4, TypeLiquid.KMnO4, Operator.Equally, 5,
                () => { _UIStagesControl.NextStep(); });
            byte step_H2SO4 = 5;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E5.KMnO4, TypeLiquid.H2SO4, Operator.More, 0,
                StateMiniTestTubeS4E5.KMnO4_H2SO4,
                () => { ChangeColorLiquid(_rendererSediment, new Color32(14, 1, 27, 175), step_H2SO4--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E5.KMnO4_H2SO4, TypeLiquid.H2SO4, Operator.More, 0, () =>
            {
                ChangeColorLiquid(new Color32(14, 1, 27, 175), step_H2SO4--);
                if (step_H2SO4 == 0)
                {
                    _UIStagesControl.NextStep();
                    step_H2SO4 = 5;
                }
            });
            byte step_K4_Fe_CN_6_ = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E5.KMnO4_H2SO4, TypeLiquid.K4_Fe_CN_6_, Operator.More, 0,
                StateMiniTestTubeS4E5.KMnO4_H2SO4_K4_Fe_CN_6_,
                () => { ChangeColorLiquid(new Color32(49, 64, 8, 64), step_K4_Fe_CN_6_--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E5.KMnO4_H2SO4_K4_Fe_CN_6_, TypeLiquid.K4_Fe_CN_6_,
                Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(new Color32(49, 64, 8, 64), step_K4_Fe_CN_6_--);
                    if (step_K4_Fe_CN_6_ == 0)
                    {
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS4E5.NotActive;
                        step_K4_Fe_CN_6_ = 6;
                    }
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS4E5.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}