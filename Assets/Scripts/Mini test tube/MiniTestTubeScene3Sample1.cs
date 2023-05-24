using Granule;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;
using Wire;

namespace Mini_test_tube
{
    public class MiniTestTubeScene3Sample1 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS3E1
        {
            Empty,
            H2SO4,
            H2SO4_Zn,
            H2SO4_Zn_corrosion,
            H2SO4_Zn_corrosion_Cu,
            H2SO4_Zn_Cu_corrosion,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS3E1> _actionAddLiquid;
        private ActionAddGranule<StateMiniTestTubeS3E1> _actionAddGranule;
        private ActionAddWire<StateMiniTestTubeS3E1> _actionAddWire;

        private ClickMouseItem _clickMouseItem;

        private PlayerMotion _playerMotion;

        private StateMiniTestTubeS3E1 _state;

        public StateMiniTestTubeS3E1 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS3E1 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            _playerMotion = FindObjectOfType<PlayerMotion>();

            _clickMouseItem = GetComponent<ClickMouseItem>();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS3E1>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E1.Empty, TypeLiquid.H2SO4, Operator.More, 0,
                StateMiniTestTubeS3E1.H2SO4, (waterColor) => { ChangeColorLiquid(waterColor); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E1.H2SO4, TypeLiquid.H2SO4, Operator.Equally, 10,
                () =>
                {
                    FindObjectOfType<CupForGranule>().ActiveCupGranule(transform);
                    _UIStagesControl.NextStep();
                });

            _actionAddGranule = new ActionAddGranule<StateMiniTestTubeS3E1>();

            _actionAddGranule.AddAction(StateMiniTestTubeS3E1.H2SO4, TypeGranule.Zn, StateMiniTestTubeS3E1.H2SO4_Zn,
                (granule) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    granule.FixedGranuleIn(transform);
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    granule.PlayBubble();
                    StartSmoothlyAction(6f, (delta) =>
                    {
                        granule.ChangeBubbleRadius(Mathf.Lerp(0.1f, 0.4f, delta));
                        granule.ChangeBubbleMeshCountEmission(Mathf.Lerp(0, 1000, delta));
                    }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _state = StateMiniTestTubeS3E1.H2SO4_Zn_corrosion;
                        _UIStagesControl.NextStep();
                    });
                });
            _actionAddGranule.AddAction(StateMiniTestTubeS3E1.H2SO4_Zn_corrosion_Cu, TypeGranule.Zn,
                (granule) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(10f, (delta) =>
                    {
                        granule.ChangeBubbleSpeed(Mathf.Lerp(0.1f, 0.6f, delta));
                        granule.ChangeBubbleCountEmission(Mathf.Lerp(100f, 600f, delta));
                    }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _state = StateMiniTestTubeS3E1.H2SO4_Zn_Cu_corrosion;
                        _UIStagesControl.NextStep();
                    });
                });

            _actionAddWire = new ActionAddWire<StateMiniTestTubeS3E1>();

            _actionAddWire.AddAction(StateMiniTestTubeS3E1.H2SO4_Zn_corrosion, TypeWire.Cu,
                StateMiniTestTubeS3E1.H2SO4_Zn_corrosion_Cu,
                (wire) => { wire.FixedWireIn(transform); });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        public void AddGranule(Granule.Granule granule)
        {
            _actionAddGranule.Launch(ref _state, granule);
        }

        public void AddWire(Wire.Wire wire)
        {
            _actionAddWire.Launch(ref _state, wire);
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS3E1.Empty;
        }
    }
}