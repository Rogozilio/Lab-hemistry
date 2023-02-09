using System;
using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample1 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS2E1
        {
            Empty,
            FeCl3,
            [InspectorName("FeCl3+NH4SCN")] FeCl3_NH4CNS,
            [InspectorName("FeCl3+NH4SCN+H2O")] FeCl3_NH4CNS_H2O,
            [InspectorName("mixtur")] FeCl3_NH4CNS_H2O_half,
            [InspectorName("mixtur/4")] FeCl3_NH4CNS_H2O_quarter,
            [InspectorName("mixtur/4+FeCl3")] FeCl3x4_NH4CNS_H2O,
            [InspectorName("mixtur/4+NH4SCN")] FeCl3_NH4CNSx4_H2O,
            [InspectorName("mixtur/4+NH4CL")] FeCl3_NH4CNS_H2O_NH4CI,
            ForFlowLiquid,
            NotActive
        }

        public LevelLiquid Sediment;

        private Renderer _rendererSediment;
        
        private byte _countFeCl3 = 0;
        private byte _countNH4CNS = 0;

        private ActionAddLiquid<StateMiniTestTubeS2E1> _actionAddLiquid;
        private ActionAddPowder<StateMiniTestTubeS2E1> _actionAddPowder;

        private StateMiniTestTubeS2E1 _state;
        private Color _originColorSediment;

        public StateMiniTestTubeS2E1 GetState => _state;

        public int countLiquid => _countLiquid;
        public byte countFeCL3 => _countFeCl3;
        public byte countNH4CNS => _countNH4CNS;
        public byte countPowder => (byte)_countPowder;


        public void Awake()
        {
            base.Awake();

            _rendererSediment = Sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            liquidFlowScript.SetUniqueActionInEnd = LastFlowLiquidChangeStateTestTube;

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E1>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.Empty, TypeLiquid.FeCl3, Operator.More, 0,
                StateMiniTestTubeS2E1.FeCl3, () =>
                {
                    _stepStageSystem.NextStep();
                    ChangeStateRestTestTube(StateMiniTestTubeS2E1.NotActive);
                    ChangeColorLiquid(new Color32(87, 77, 13, 103));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3, TypeLiquid.NH4CNS, Operator.More, 0,
                StateMiniTestTubeS2E1.FeCl3_NH4CNS, () =>
                {
                    _stepStageSystem.NextStep();
                    ChangeColorLiquid(new Color32(10, 0, 0, 230));
                });
            byte stepH2O = 18;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS, TypeLiquid.H2O, Operator.More, 0,
                StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O,
                () => { ChangeColorLiquid(new Color32(30, 0, 0, 80), stepH2O--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O, TypeLiquid.H2O, Operator.Less, 36,
                () => { ChangeColorLiquid(new Color32(30, 0, 0, 80), stepH2O--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O, TypeLiquid.H2O, Operator.MoreEquals, 36,
                StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_half,
                () =>
                {
                    _stepStageSystem.NextStep();
                    ChangeStateRestTestTube(StateMiniTestTubeS2E1.ForFlowLiquid);
                    ChangeColorLiquid(new Color32(30, 0, 0, 80), stepH2O--);
                });
            byte stepFeCI3 = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3x4_NH4CNS_H2O, TypeLiquid.FeCl3, Operator.More,
                0, () =>
                {
                    _countFeCl3++;
                    ChangeStateWithState(StateMiniTestTubeS2E1.FeCl3x4_NH4CNS_H2O,
                        StateMiniTestTubeS2E1.NotActive);
                    ChangeColorLiquid(new Color32(10, 0, 0, 90), stepFeCI3--);
                    if (stepFeCI3 == 0)
                    {
                        _stepStageSystem.NextStep();
                        ChangeStateWithState(StateMiniTestTubeS2E1.NotActive,
                                                    StateMiniTestTubeS2E1.FeCl3_NH4CNSx4_H2O);
                        stepFeCI3 = 4;
                    }
                });
            byte stepNH4CNS = 4;
        
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNSx4_H2O, TypeLiquid.NH4CNS, Operator.More, 0,
                () =>
                {
                    _countNH4CNS++;
                    ChangeStateWithState(StateMiniTestTubeS2E1.FeCl3_NH4CNSx4_H2O, StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_NH4CI);
                    ChangeColorLiquid(new Color32(7, 0, 0, 90), stepNH4CNS--);
                    if (stepNH4CNS == 0)
                    {
                        _stepStageSystem.NextStep();
                        stepNH4CNS = 4;
                    }
                });

            _actionAddPowder = new ActionAddPowder<StateMiniTestTubeS2E1>();
            
            _actionAddPowder.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_NH4CI, TypePowder.NH4CI, Operator.More,
                0, () =>
                {
                    _levelLiquid.level += _step;
                    Sediment.level += _step;
                    _stepStageSystem.NextStep();
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 4f);
                    ChangeStateWithState(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_NH4CI, StateMiniTestTubeS2E1.NotActive);
                    ChangeColorLiquid(_rendererSediment, new Color32(176, 56, 31, 150));
                });
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS2E1)index;
        }

        public override void Stir(Transform stick)
        {
            base.Stir(stick);

            if(!_isStir) return;
            var stepSediment = (byte)(_rendererSediment.material.GetColor("_LiquidColor").a * 255 * 4);
            ChangeColorLiquid(new Color32(30, 3, 0, 80), stepSediment);
            
            ChangeColorLiquid(_rendererSediment, new Color32(176, 56, 31, 0), stepSediment);

            if (stepSediment == 0)
            {
                _countPowder = 0;
                _stepStageSystem.NextStep();
                _state = StateMiniTestTubeS2E1.NotActive;
                stick.GetComponent<StateItem>().ChangeState(StateItems.BackToMouse);
                stick.GetComponent<MoveMap>().StartToMove(3);
            }
        }

        private void FixedUpdate()
        {
            PourOutLiquid(0.005f, 0.15f);
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }

        public override void AddPowder(PowderDrop powder)
        {
            base.AddPowder(powder);

            _actionAddPowder.Launch(ref _state, powder.typePowder, _countPowder);
            _countLiquid++;
        }

        private void ChangeStateRestTestTube(StateMiniTestTubeS2E1 newState)
        {
            var objects = FindObjectsOfType<MiniTestTubeScene2Sample1>();
            foreach (var obj in objects)
            {
                if (obj == this) continue;

                obj._state = newState;
            }
        }

        private void ChangeStateWithState(StateMiniTestTubeS2E1 oldState, StateMiniTestTubeS2E1 newState)
        {
            var objects = FindObjectsOfType<MiniTestTubeScene2Sample1>();
            foreach (var obj in objects)
            {
                if (obj == this || obj._state != oldState) continue;

                obj._state = newState;
            }
        }

        private void LastFlowLiquidChangeStateTestTube()
        {
            _stepStageSystem.NextStep();
            var count = 0;
            var objects = FindObjectsOfType<MiniTestTubeScene2Sample1>();
            foreach (var t in objects)
            {
                if (t._state == StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter)
                {
                    count++;
                }
            }

            if (count != objects.Length - 1) return;
            
            foreach (var obj in objects)
            {
                obj._state = StateMiniTestTubeS2E1.FeCl3x4_NH4CNS_H2O;
                obj._countLiquid = 0;
            }
        }
        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS2E1.Empty;
            Sediment.level = 0;
            _countFeCl3 = 0;
            _countNH4CNS = 0;
            Sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}