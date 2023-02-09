using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample3 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS2E3
        {
            Empty,
            K2CrO4,
            [InspectorName("K2CrO4+H2SO4")] K2CrO4_H2SO4,
            [InspectorName("K2CrO4+H2SO4+NaOH")] K2CrO4_H2SO4_NaOH,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS2E3> _actionAddLiquid;

        private StateMiniTestTubeS2E3 _state;

        public StateMiniTestTubeS2E3 GetState => _state;
        public int GetCountLiquid => _countLiquid;

        public void Awake()
        {
            base.Awake();

            var yellowColor = new Color32(56, 56, 0, 80);
            var orangeColor = new Color32(87, 16, 0, 80);

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E3>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E3.Empty, TypeLiquid.K2CrO4, Operator.More, 0,
                StateMiniTestTubeS2E3.K2CrO4, () => { ChangeColorLiquid(yellowColor); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E3.K2CrO4, TypeLiquid.K2CrO4, Operator.Equally, 3,
                () => { _stepStageSystem.NextStep(); });

            byte stepH2SO4 = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E3.K2CrO4, TypeLiquid.H2SO4, Operator.More, 0,
                StateMiniTestTubeS2E3.K2CrO4_H2SO4, () => { ChangeColorLiquid(orangeColor, stepH2SO4--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E3.K2CrO4_H2SO4, TypeLiquid.H2SO4, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(orangeColor, stepH2SO4--);
                    if (stepH2SO4 == 0)
                    {
                        _stepStageSystem.NextStep();
                        stepH2SO4 = 3;
                    }
                });

            byte stepNaOH = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E3.K2CrO4_H2SO4, TypeLiquid.NaOH, Operator.More, 0,
                StateMiniTestTubeS2E3.K2CrO4_H2SO4_NaOH, () => { ChangeColorLiquid(yellowColor, stepNaOH--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E3.K2CrO4_H2SO4_NaOH, TypeLiquid.NaOH, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(yellowColor, stepNaOH--);
                    if (stepNaOH == 0)
                    {
                        _stepStageSystem.NextStep();
                        stepNaOH = 3;
                    }
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS2E3.Empty;
        }
    }
}