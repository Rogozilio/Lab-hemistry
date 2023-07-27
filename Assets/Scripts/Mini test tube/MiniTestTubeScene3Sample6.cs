﻿using Granule;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;
using Wire;

namespace Mini_test_tube
{
    public class MiniTestTubeScene3Sample6 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS3E6
        {
            Empty,
            Hg_NO3_2,
            Hg_NO3_2_Al,
            H2O,
            H20_Al,
            H20_Al_amalgam,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS3E6> _actionAddLiquid;
        private ActionAddWire<StateMiniTestTubeS3E6> _actionAddWire;

        private PlayerMotion _playerMotion;

        private StateMiniTestTubeS3E6 _state;

        public StateMiniTestTubeS3E6 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS3E6 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            var countWireInTestTube = 0;

            _playerMotion = FindObjectOfType<PlayerMotion>();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS3E6>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E6.Hg_NO3_2, TypeLiquid.Hg_NO3_2, Operator.Equally, 10,
                StateMiniTestTubeS3E6.Hg_NO3_2_Al, () =>
                {
                    _UIStagesControl.NextStep();
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E6.H2O);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E6.Empty, TypeLiquid.Hg_NO3_2, Operator.More, 0,
                StateMiniTestTubeS3E6.Hg_NO3_2, (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E6.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E6.H2O, TypeLiquid.H2O, Operator.Equally, 10
                , () => { _UIStagesControl.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E6.H2O, TypeLiquid.H2O, Operator.Equally, 1
                , (bottleColor) => { ChangeColorLiquid(bottleColor); });

            _actionAddWire = new ActionAddWire<StateMiniTestTubeS3E6>();

            _actionAddWire.AddAction(StateMiniTestTubeS3E6.Hg_NO3_2_Al, TypeWire.Al,
                (wire) =>
                {
                    if (wire.isFormedAmalgam)
                    {
                        var isfixed = false;
                        StartSmoothlyAction(1f, (delta) =>
                        {
                            if (wire.GetStateItem == StateItems.Idle && !isfixed)
                            {
                                wire.FixedWireIn(transform);
                                wire.SetMoveMouseItemEnable = true;
                                isfixed = true;
                            }
                        }, () => { });
                        return;
                    }

                    wire.FixedWireIn(transform);
                    CursorSkin.Instance.isUseClock = true;
                    UpTestTube();
                    _UIStagesControl.NextStep();
                    _playerMotion.MoveToPoint(transform, 10);
                    _state = StateMiniTestTubeS3E6.NotActive;
                    wire.GetComponent<TimerTooltip>()
                        .StartTimerTooltip(wire.transform, "Полоска алюминия", "Образование амальгамы");
                    wire.StartWirePartEffect("0");
            
                    StartSmoothlyAction(30f, (delta) => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
                        wire.isFormedAmalgam = true;
                        wire.SetMoveMouseItemEnable = true;
                        wire.SwitchOnLinearState();
                        SetStateOtherMiniTestTube(StateMiniTestTubeS3E6.H20_Al_amalgam);
                    });
                });

            _actionAddWire.AddAction(StateMiniTestTubeS3E6.H20_Al, TypeWire.Al,
                (wire) =>
                {
                    wire.FixedWireIn(transform);
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(8f, (delta) => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS3E6.NotActive;
                    });
                });

            _actionAddWire.AddAction(StateMiniTestTubeS3E6.H20_Al_amalgam, TypeWire.Al,
                (wire) =>
                {
                    wire.FixedWireIn(transform);
                    wire.PlayExistEffects();
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(8f, (delta) => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS3E6.NotActive;
                        SetStateOtherMiniTestTube(StateMiniTestTubeS3E6.H20_Al, StateMiniTestTubeS3E6.NotActive);
                    });
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        public void AddWire(Wire.Wire wire)
        {
            _actionAddWire.Launch(ref _state, wire);
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS3E6.Empty;
        }

        private void SetStateOtherMiniTestTube(StateMiniTestTubeS3E6 state, params StateMiniTestTubeS3E6[] excepts)
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample6>();

            foreach (var tube in tubes)
            {
                var isExceptState = false;
                if (tube == this) continue;
                foreach (var except in excepts)
                {
                    if (except != tube.GetState) continue;
                    isExceptState = true;
                }

                if (isExceptState)
                {
                    continue;
                }

                tube.SetState = state;
            }
        }
    }
}