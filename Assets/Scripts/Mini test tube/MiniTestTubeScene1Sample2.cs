using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample2 : MiniTestTube
    {
        public enum StateMiniTestTubeS1E2
        {
            Empty,
            NiCl2,
            [InspectorName("NiCl2+NaOH")] NiCl2_NaOH,
            [InspectorName("NiCl2+NaOH+HCl")] NiCl2_NaOH_HCl,
            [InspectorName("Bi(NO3)3")] Bi_NO3_3,
            [InspectorName("Bi(NO3)3+NaOH")] Bi_NO3_3_NaOH,
            [InspectorName("Bi(NO3)3+NaOH+HNO3")] Bi_NO3_3_NaOH_HNO3,
            NotActive
        }

        private StateMiniTestTubeS1E2 _state;
        private ActionAddLiquid<StateMiniTestTubeS1E2> _actionAddLiquid;

        public StateMiniTestTubeS1E2 GetState => _state;
        public int countLiquid => _countLiquid;
        public bool IsOnlyForNiCl2 => IsOnlyForState(StateMiniTestTubeS1E2.NiCl2);
        public bool IsOnlyForBiNO3 => IsOnlyForState(StateMiniTestTubeS1E2.Bi_NO3_3);

        public LevelLiquid Sediment;

        public void Awake()
        {
            base.Awake();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E2>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Empty, TypeLiquid.NiCl2,
                Operator.More, 0, StateMiniTestTubeS1E2.NiCl2,
                () => { ChangeColorLiquid(new Color32(26, 106, 59, 82)); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.NiCl2, TypeLiquid.NiCl2,
                Operator.More, 0, () => { ChangeColorLiquid(new Color32(26, 106, 59, 82)); });
            byte stepNaOH = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.NiCl2, TypeLiquid.NaOH,
                Operator.More, 6, StateMiniTestTubeS1E2.NiCl2_NaOH, () =>
                {
                    Sediment.level = _levelLiquid.level;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color32(52, 255, 137, 200), stepNaOH--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.NiCl2_NaOH, TypeLiquid.NaOH,
                Operator.More, 6, () =>
                {
                    Sediment.level = _levelLiquid.level;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(),new Color32(52, 255, 137, 200), stepNaOH--);
                });
            byte stepHCI = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.NiCl2_NaOH, TypeLiquid.HCI,
                Operator.More, 10, StateMiniTestTubeS1E2.NiCl2_NaOH_HCl,
                () =>
                {
                    Sediment.level = _levelLiquid.level;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepHCI);
                    ChangeColorLiquid(new Color32(26, 106, 59, 30), stepHCI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.NiCl2_NaOH_HCl, TypeLiquid.HCI,
                Operator.More, 10, () =>
                {
                    Sediment.level = _levelLiquid.level;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepHCI);
                    ChangeColorLiquid(new Color32(26, 106, 59, 30), stepHCI--);
                });

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Empty, TypeLiquid.Bi_NO3_3,
                Operator.More, 0, StateMiniTestTubeS1E2.Bi_NO3_3,
                () => { ChangeColorLiquid(new Color32(172, 198, 219, 30)); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Bi_NO3_3, TypeLiquid.Bi_NO3_3,
                Operator.More, 0, () => { ChangeColorLiquid(new Color32(172, 198, 219, 30)); });
            byte stepNaOH_2 = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Bi_NO3_3, TypeLiquid.NaOH,
                Operator.More, 6, StateMiniTestTubeS1E2.Bi_NO3_3_NaOH,
                () =>
                {
                    Sediment.level = _levelLiquid.level / 2f;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(1, 1, 1, 1f), stepNaOH_2--);
                    Sediment.GetComponent<Renderer>().material.SetFloat("_SidmentMultiply", 4f);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Bi_NO3_3_NaOH, TypeLiquid.NaOH,
                Operator.More, 6, () =>
                {
                    Sediment.level = _levelLiquid.level / 2f;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(1, 1, 1, 1f), stepNaOH_2--);
                });
            byte stepHNO3 = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Bi_NO3_3_NaOH, TypeLiquid.HNO3,
                Operator.More, 10, StateMiniTestTubeS1E2.Bi_NO3_3_NaOH_HNO3,
                () => { ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepHNO3--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E2.Bi_NO3_3_NaOH_HNO3, TypeLiquid.HNO3,
                Operator.More, 10,
                () => { ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepHNO3--); });
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E2)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }

        private bool IsOnlyForState(StateMiniTestTubeS1E2 state)
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample2>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube.GetState == state)
                {
                    return this == miniTestTube;
                }
            }

            return true;
        }
    }
}