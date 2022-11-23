using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample3_1 : MiniTestTube
    {
        public enum StateMiniTestTubeS1E3_1
        {
            Empty,
            CrCl3,
            [InspectorName("CrCl3+NaOH")] CrCl3_NaOH,
            [InspectorName("CrCl3+NaOH/2")] CrCl3_NaOH_half,
            [InspectorName("CrCl3+NaOH/2+HCI")] CrCl3_NaOH_half_HCl,
            [InspectorName("CrCl3+NaOH/2+NaOH")] CrCl3_NaOH_half_NaOH,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS1E3_1> _actionAddLiquid;
        private StateMiniTestTubeS1E3_1 _state;
        private int _countNaOH;
        private int _countHCI;

        public StateMiniTestTubeS1E3_1 GetState => _state;
        public int GetCountLiquid => _countLiquid;
        public int GetCountNaOH => _countNaOH;
        public int GetCountHCI => _countHCI;

        public LevelLiquid Sediment;

        public void Awake()
        {
            base.Awake();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E3_1>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.Empty, TypeLiquid.CrCl3, Operator.More, 0,
                StateMiniTestTubeS1E3_1.CrCl3,
                () => { ChangeColorLiquid(new Color32(0, 2, 31, 83)); });
            byte stepNaOH = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3, TypeLiquid.NaOH, Operator.More, 10,
                StateMiniTestTubeS1E3_1.CrCl3_NaOH,
                () =>
                {
                    Sediment.level = levelLiquid.level;
                    Sediment.GetComponent<Renderer>().material.SetFloat("_IsWorldPosition", 1);
                    ChangeColorLiquid(new Color32(7, 12, 7, 183));
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0.115f, 0.115f, 0.115f, 1f), stepNaOH--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH, TypeLiquid.NaOH, Operator.More, 10,
                () =>
                {
                    Sediment.level = levelLiquid.level;
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(),  new Color(0.115f, 0.115f, 0.115f, 1f), stepNaOH--);
                });
            byte stepNaOHhalf = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half, TypeLiquid.NaOH, Operator.More, 0,
                StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_NaOH,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(new Color32(3,8,3,100));
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepNaOHhalf--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_NaOH, TypeLiquid.NaOH, Operator.More, 0,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(new Color32(3,8,3,100));
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepNaOHhalf--);
                });
            byte stepHCI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_HCl,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(new Color32(5, 6, 19, 100));
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0f), stepHCI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_HCl, TypeLiquid.HCI, Operator.More, 0,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(new Color32(5, 6, 19, 100));
                    ChangeColorLiquid(Sediment.GetComponent<Renderer>(), new Color(0, 0, 0, 0), stepHCI--);
                });
        }

        private void FixedUpdate()
        {
            PourOutLiquid(0.005f, _levelLiquid.level / 2f, Sediment);
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E3_1)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }
    }
}