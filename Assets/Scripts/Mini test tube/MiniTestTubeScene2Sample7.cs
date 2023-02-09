using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample7 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS2E7
        {
            Empty,
            MnSO4,
            [InspectorName("MnSO4+(NH4)2S")] MnSO4_NH4_2S,
            [InspectorName("MnSO4+(NH4)2S+HCl")] MnSO4_NH4_2S_HCl,
            SbCl3,
            [InspectorName("SbCl3+Na2S")] SbCl3_Na2S,
            [InspectorName("SbCl3+Na2S+HCl")] SbCl3_Na2S_HCl,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS2E7> _actionAddLiquid;

        private StateMiniTestTubeS2E7 _state;

        private Renderer _rendererSediment;
        private readonly Color _waterColor = new Color32(63, 63, 63, 72);
        private Color _originColorSediment;

        public StateMiniTestTubeS2E7 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS2E7 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            
            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E7>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.Empty, TypeLiquid.MnSO4, Operator.More, 0,
                StateMiniTestTubeS2E7.MnSO4, () =>
                {
                    SwitchStateNextMiniTestTube(StateMiniTestTubeS2E7.NotActive);
                    ChangeColorLiquid(_waterColor);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4, TypeLiquid.MnSO4, Operator.Equally, 3,
                () => { _stepStageSystem.NextStep(); });
            var colorSediment = new Color32(81, 72, 35, 200);
            var colorSedimentInvisible = new Color32(81, 72, 35, 0);
            byte step_NH4_2S = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4, TypeLiquid._NH4_2S, Operator.More, 0,
                StateMiniTestTubeS2E7.MnSO4_NH4_2S, () =>
                {
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 4);
                    sediment.level = levelLiquid.level * 0.9f;
                    ChangeColorLiquid(_rendererSediment, colorSediment, step_NH4_2S--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4_NH4_2S, TypeLiquid._NH4_2S, Operator.More, 0, () =>
            {
                ChangeColorLiquid(_rendererSediment, colorSediment, step_NH4_2S--);
                if (step_NH4_2S == 0)
                {
                    _stepStageSystem.NextStep();
                    step_NH4_2S = 3;
                }
            });
            byte step_HCI = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4_NH4_2S, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS2E7.MnSO4_NH4_2S_HCl,
                () => { ChangeColorLiquid(_rendererSediment, colorSedimentInvisible, step_HCI--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4_NH4_2S_HCl, TypeLiquid.HCI, Operator.More, 0, () =>
            {
                ChangeColorLiquid(_rendererSediment, colorSedimentInvisible, step_HCI--);
                if (step_HCI == 0)
                {
                    _stepStageSystem.NextStep();
                    SwitchStateNextMiniTestTube(StateMiniTestTubeS2E7.SbCl3);
                    step_HCI = 6;
                }
            });

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3, TypeLiquid.SbCl3, Operator.Equally, 3,
                () => { _stepStageSystem.NextStep(); });
            var colorSediment2 = new Color32(214, 47, 0, 255);
            byte stepNa2S = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3, TypeLiquid.Na2S, Operator.More, 0,
                StateMiniTestTubeS2E7.SbCl3_Na2S, () =>
                {
                    sediment.level = levelLiquid.level * 0.8f;
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 3);
                    ChangeColorLiquid(_rendererSediment, colorSediment2, stepNa2S--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3_Na2S, TypeLiquid.Na2S, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level * 0.8f;
                ChangeColorLiquid(_rendererSediment, colorSediment2, stepNa2S--);
                if (stepNa2S == 0)
                {
                    _stepStageSystem.NextStep();
                    stepNa2S = 3;
                }
            });
            byte step2_HCI = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3_Na2S, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS2E7.SbCl3_Na2S_HCl, () =>
                {
                    sediment.level = levelLiquid.level * 0.8f;
                    ChangeColorLiquid(colorSediment2, step2_HCI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3_Na2S_HCl, TypeLiquid.HCI, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level * 0.8f;
                ChangeColorLiquid(colorSediment2, step2_HCI--);
                if (step2_HCI == 0)
                {
                    _stepStageSystem.NextStep();
                    step2_HCI = 6;
                }
            });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }

        private void SwitchStateNextMiniTestTube(StateMiniTestTubeS2E7 state)
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene2Sample7>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (this == miniTestTube) continue;
                miniTestTube.SetState = state;
                miniTestTube.ChangeColorLiquid(_waterColor);
            }
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS2E7.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}