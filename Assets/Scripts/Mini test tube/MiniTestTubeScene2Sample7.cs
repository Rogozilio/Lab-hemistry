using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample7 : MiniTestTube
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
        
        private readonly Color _waterColor = new Color32(63, 63, 63, 72);

        public StateMiniTestTubeS2E7 GetState => _state;
        public int getCountLiquid => _countLiquid;
        
        public StateMiniTestTubeS2E7 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

         
            var sedimentColor = new Color32(130, 130, 130, 130);

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E7>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.Empty, TypeLiquid.MnSO4, Operator.More, 0,
                StateMiniTestTubeS2E7.MnSO4, () =>
                {
                    SwitchStateNextMiniTestTube(StateMiniTestTubeS2E7.SbCl3);
                    ChangeColorLiquid(_waterColor);
                });
            var colorSediment = new Color32(81, 72, 35, 200);
            var colorSedimentInvisible = new Color32(81, 72, 35, 0);
            byte step_NH4_2S = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4, TypeLiquid._NH4_2S, Operator.More, 0,
                StateMiniTestTubeS2E7.MnSO4_NH4_2S, () =>
                {
                    sediment.GetComponent<Renderer>().material.SetFloat("_SedimentMultiply", 4);
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),colorSediment, step_NH4_2S--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4_NH4_2S, TypeLiquid._NH4_2S, Operator.More, 0, () =>
                {
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),colorSediment, step_NH4_2S--);
                });
            byte step_HCI = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4_NH4_2S, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS2E7.MnSO4_NH4_2S_HCl, () =>
                {
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),colorSedimentInvisible, step_HCI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.MnSO4_NH4_2S_HCl, TypeLiquid.HCI, Operator.More, 0, () =>
            {
                ChangeColorLiquid(sediment.GetComponent<Renderer>(),colorSedimentInvisible, step_HCI--);
            });
            
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.Empty, TypeLiquid.SbCl3, Operator.More, 0,
                StateMiniTestTubeS2E7.SbCl3, () =>
                {
                    SwitchStateNextMiniTestTube(StateMiniTestTubeS2E7.MnSO4);
                    ChangeColorLiquid(_waterColor);
                });
            var colorSediment2 = new Color32(214, 47, 0, 255);
            byte stepNa2S = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3, TypeLiquid.Na2S, Operator.More, 0,
                StateMiniTestTubeS2E7.SbCl3_Na2S, () =>
                {
                    sediment.level = levelLiquid.level * 0.8f;
                    sediment.GetComponent<Renderer>().material.SetFloat("_SedimentMultiply", 3);
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),colorSediment2, stepNa2S--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E7.SbCl3_Na2S, TypeLiquid.Na2S, Operator.More, 0, () =>
            {
                sediment.level = levelLiquid.level * 0.8f;
                ChangeColorLiquid(sediment.GetComponent<Renderer>(),colorSediment2, stepNa2S--);
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
                if(this == miniTestTube) continue;
                miniTestTube.SetState = state;
                miniTestTube.ChangeColorLiquid(_waterColor);
            }
        }
    }
}