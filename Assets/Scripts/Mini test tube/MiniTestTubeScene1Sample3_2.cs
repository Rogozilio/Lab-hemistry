using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample3_2 : MiniTestTube
    {
        public enum StateMiniTestTubeS1E3_2
        {
            Empty,
            [InspectorName("AI2(SO4)3")]AI2SO43,
            [InspectorName("AI2(SO4)3+NaOH")] AI2SO43_NaOH,
            [InspectorName("AI2(SO4)3+NaOH/2")] AI2SO43_NaOH_half,
            [InspectorName("AI2(SO4)3+NaOH/2+HCI")] AI2SO43_NaOH_half_HCl,
            [InspectorName("AI2(SO4)3+NaOH/2+NaOH")] AI2SO43_NaOH_half_NaOH,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS1E3_2> _actionAddLiquid;
        private StateMiniTestTubeS1E3_2 _state;
        private int _countNaOH;
        private int _countHCI;

        public StateMiniTestTubeS1E3_2 GetState => _state;
        public int GetCountLiquid => _countLiquid;
        public int GetCountNaOH => _countNaOH;
        public int GetCountHCI => _countHCI;

        public LevelLiquid Sediment;

        public void Awake()
        {
            base.Awake();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E3_2>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.Empty, TypeLiquid.CrCl3, Operator.More, 0,
                StateMiniTestTubeS1E3_2.AI2SO43,
                () => { ChangeColorLiquid(new Color32(172, 198, 219, 30)); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43, TypeLiquid.NaOH, Operator.More, 10,
                StateMiniTestTubeS1E3_2.AI2SO43_NaOH,
                () =>
                {
                    Sediment.level = levelLiquid.level / 1.5f;
                    Sediment.GetComponent<Renderer>().material.SetFloat("_SedimentMultiply", 2);
                    Sediment.GetComponent<Renderer>().material.SetFloat("_IsWorldPosition", 1);
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(1, 1, 1, 0.9f));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH, TypeLiquid.NaOH, Operator.More, 10,
                () =>
                {
                    Sediment.level = levelLiquid.level / 1.1f;
                });

            byte stepNaOH = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half, TypeLiquid.NaOH, Operator.More, 0,
                StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_NaOH,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0), stepNaOH--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_NaOH, TypeLiquid.NaOH, Operator.More, 0,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0), stepNaOH--);
                });
            byte stepHCI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_HCl,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0), stepHCI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_HCl, TypeLiquid.HCI, Operator.More, 0,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0), stepHCI--);
                });
        }

        private void FixedUpdate()
        {
            PourOutLiquid(0.005f, _levelLiquid.level / 2f, Sediment);
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E3_2)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }
    }
}