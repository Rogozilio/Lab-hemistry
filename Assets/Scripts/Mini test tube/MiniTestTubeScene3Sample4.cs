using Granule;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;
using Wire;

namespace Mini_test_tube
{
    public class MiniTestTubeScene3Sample4 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS3E4
        {
            Empty,
            H2SO4,
            H2SO4_steel,
            H2SO4_steel_corrosion,
            HNO3,
            HNO3_steel,
            HNO3_steel_passivation,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS3E4> _actionAddLiquid;
        private ActionAddGranule<StateMiniTestTubeS3E4> _actionAddGranule;
        private ActionAddWire<StateMiniTestTubeS3E4> _actionAddWire;

        private ClickMouseItem _clickMouseItem;

        private PlayerMotion _playerMotion;

        private StateMiniTestTubeS3E4 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;
        private TimerTooltip _timerTooltip;

        public StateMiniTestTubeS3E4 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS3E4 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            _playerMotion = FindObjectOfType<PlayerMotion>();

            _clickMouseItem = GetComponent<ClickMouseItem>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _rendererSediment.material.SetFloat("_SedimentMultiply", 4);
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            _timerTooltip = GetComponent<TimerTooltip>();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS3E4>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E4.Empty, TypeLiquid.H2SO4, Operator.More, 0,
                StateMiniTestTubeS3E4.H2SO4, (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E4.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E4.H2SO4, TypeLiquid.H2SO4, Operator.Equally, 15,
                StateMiniTestTubeS3E4.H2SO4_steel, () => { _UIStagesControl.NextStep(); });

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E4.HNO3, TypeLiquid.HNO3, Operator.Equally, 1,
                (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E4.NotActive,
                        StateMiniTestTubeS3E4.H2SO4_steel_corrosion);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E4.HNO3, TypeLiquid.HNO3, Operator.Equally, 15,
                StateMiniTestTubeS3E4.HNO3_steel, () => { _UIStagesControl.NextStep(); });

            _actionAddWire = new ActionAddWire<StateMiniTestTubeS3E4>();

            _actionAddWire.AddAction(StateMiniTestTubeS3E4.H2SO4_steel, TypeWire.steel,
                StateMiniTestTubeS3E4.H2SO4_steel_corrosion, (wire) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    wire.FixedWireIn(transform);
                    if(wire.typePassivate == TypePassivate.No)
                        wire.PlayExistEffects();
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(5f, (delta) => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
                        if(wire.typePassivate == TypePassivate.No)
                            SetStateOtherMiniTestTube(StateMiniTestTubeS3E4.HNO3);
                    });
                });

            _actionAddWire.AddAction(StateMiniTestTubeS3E4.HNO3_steel, TypeWire.steel,
                StateMiniTestTubeS3E4.HNO3_steel_passivation, (wire) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    wire.FixedWireIn(transform);
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    _timerTooltip.StartTimerTooltip(wire.transform, "Стальная проволока", "Процесс пассивация");
                    StartSmoothlyAction(15f, delta => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
                        wire.typePassivate = TypePassivate.Passivated;
                        wire.SetMoveMouseItemEnable = true;
                        wire.SwitchOnLinearState();
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
            _state = StateMiniTestTubeS3E4.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }

        private void SetStateOtherMiniTestTube(StateMiniTestTubeS3E4 state, params StateMiniTestTubeS3E4[] excepts)
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample4>();

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

        public void LastMiniTestInH2SO4()
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample4>();

            foreach (var tube in tubes)
            {
                if (tube.GetState == StateMiniTestTubeS3E4.NotActive)
                    tube.SetState = StateMiniTestTubeS3E4.Empty;
            }
        }
    }
}