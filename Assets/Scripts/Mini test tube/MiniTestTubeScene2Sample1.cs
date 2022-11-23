using System;
using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample1 : MiniTestTube
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
            NotActive
        }

        public LevelLiquid Sediment;

        private Renderer _rendererSediment;

        private byte _indexSort = 0;
        private byte _countFeCl3 = 0;
        private byte _countNH4CNS = 0;

        private ActionAddLiquid<StateMiniTestTubeS2E1> _actionAddLiquid;
        private ActionAddPowder<StateMiniTestTubeS2E1> _actionAddPowder;

        private StateMiniTestTubeS2E1 _state;

        public StateMiniTestTubeS2E1 GetState => _state;

        public int countLiquid => _countLiquid;
        public byte IndexSort => _indexSort;
        public bool IsIndexSortFeCl3 => CheckSortIndex((int)TypeLiquid.FeCl3);
        public bool IsIndexSortNH4CNS => CheckSortIndex((int)TypeLiquid.NH4CNS);
        public bool IsIndexSortNH4CL => CheckSortIndex(3);
        public byte countFeCL3 => _countFeCl3;
        public byte countNH4CNS => _countNH4CNS;
        public byte countPowder => (byte)_countPowder;

        public void Awake()
        {
            base.Awake();

            _rendererSediment = Sediment.GetComponent<Renderer>();
            liquidFlowScript.actionInEnd += () => { QuarterLastTestTube(); };

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E1>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.Empty, TypeLiquid.FeCl3, Operator.More, 0,
                StateMiniTestTubeS2E1.FeCl3, () =>
                {
                    NotActiveRestTestTube();
                    ChangeColorLiquid(new Color32(87, 77, 13, 103));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3, TypeLiquid.NH4CNS, Operator.More, 0,
                StateMiniTestTubeS2E1.FeCl3_NH4CNS, () => { ChangeColorLiquid(new Color32(10, 0, 0, 230)); });
            byte stepH2O = 18;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS, TypeLiquid.H2O, Operator.More, 0,
                StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O,
                () => { ChangeColorLiquid(new Color32(30, 0, 0, 80), stepH2O--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O, TypeLiquid.H2O, Operator.Less, 36,
                () => { ChangeColorLiquid(new Color32(30, 0, 0, 80), stepH2O--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O, TypeLiquid.H2O, Operator.MoreEquals, 36,
                StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_half,
                () => { ChangeColorLiquid(new Color32(30, 0, 0, 80), stepH2O--); });
            byte stepFeCI3 = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter, TypeLiquid.FeCl3, Operator.More,
                0,
                StateMiniTestTubeS2E1.FeCl3x4_NH4CNS_H2O, () =>
                {
                    _countFeCl3++;
                    _indexSort = (byte)TypeLiquid.FeCl3;
                    ChangeColorLiquid(new Color32(10, 0, 0, 90), stepFeCI3--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3x4_NH4CNS_H2O, TypeLiquid.FeCl3, Operator.More, 0,
                () =>
                {
                    _countFeCl3++;
                    ChangeColorLiquid(new Color32(10, 0, 0, 90), stepFeCI3--);
                });
            byte stepNH4CNS = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter, TypeLiquid.NH4CNS, Operator.More,
                0,
                StateMiniTestTubeS2E1.FeCl3_NH4CNSx4_H2O, () =>
                {
                    _countNH4CNS++;
                    _indexSort = (byte)TypeLiquid.NH4CNS;
                    ChangeColorLiquid(new Color32(7, 0, 0, 90), stepNH4CNS--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNSx4_H2O, TypeLiquid.NH4CNS, Operator.More, 0,
                () =>
                {
                    _countNH4CNS++;
                    ChangeColorLiquid(new Color32(7, 0, 0, 90), stepNH4CNS--);
                });

            _actionAddPowder = new ActionAddPowder<StateMiniTestTubeS2E1>();
            
            _actionAddPowder.AddAction(StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter, TypePowder.NH4CI, Operator.More,
                0, StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_NH4CI, () =>
                {
                    _indexSort = 3;
                    _levelLiquid.level += _step;
                    Sediment.level += _step;
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 4f);
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
        }

        private void NotActiveRestTestTube()
        {
            var objects = FindObjectsOfType<MiniTestTubeScene2Sample1>();
            foreach (var obj in objects)
            {
                if (obj == this) continue;

                obj._state = StateMiniTestTubeS2E1.NotActive;
            }
        }

        private void QuarterLastTestTube()
        {
            var count = 0;
            var index = 0;
            var objects = FindObjectsOfType<MiniTestTubeScene2Sample1>();
            for (var i = 0; i < objects.Length; i++)
            {
                if (objects[i]._state == StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter)
                {
                    count++;
                }
                else
                {
                    index = i;
                }
            }

            if (count == objects.Length - 1)
                objects[index]._state = StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter;
        }

        private bool CheckSortIndex(int index)
        {
            if (_state < StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_quarter ||
                _state > StateMiniTestTubeS2E1.FeCl3_NH4CNS_H2O_NH4CI) return false;
            if (_indexSort == index) return true;

            var objects = FindObjectsOfType<MiniTestTubeScene2Sample1>();
            foreach (var obj in objects)
            {
                if (obj.IndexSort == index)
                {
                    return false;
                }
            }

            return _indexSort == 0;
        }
    }
}