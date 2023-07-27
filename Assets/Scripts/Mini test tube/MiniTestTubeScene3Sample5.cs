using Granule;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;
using Wire;

namespace Mini_test_tube
{
    public class MiniTestTubeScene3Sample5 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS3E5
        {
            Empty,
            CuSO4,
            CuSO4_steel,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS3E5> _actionAddLiquid;
        private ActionAddWire<StateMiniTestTubeS3E5> _actionAddWire;

        private ClickMouseItem _clickMouseItem;

        private PlayerMotion _playerMotion;

        private StateMiniTestTubeS3E5 _state;
        private int _countWireOnPaper;

        public StateMiniTestTubeS3E5 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS3E5 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            var countWireInTestTube = 0;

            _playerMotion = FindObjectOfType<PlayerMotion>();

            _clickMouseItem = GetComponent<ClickMouseItem>();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS3E5>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E5.CuSO4, TypeLiquid.CuSO4, Operator.Equally, 20,
                StateMiniTestTubeS3E5.CuSO4_steel, () => { _UIStagesControl.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E5.Empty, TypeLiquid.CuSO4, Operator.More, 0,
                StateMiniTestTubeS3E5.CuSO4, () =>
                {
                    ChangeColorLiquid(new Color32(12, 58, 50, 80));
                });

            _actionAddWire = new ActionAddWire<StateMiniTestTubeS3E5>();

            _actionAddWire.AddAction(StateMiniTestTubeS3E5.CuSO4_steel, TypeWire.steel,
                (wire) =>
                {
                    countWireInTestTube++;
                    
                    if(wire.GetComponent<ConnectTransform>().IsEnable)
                        wire.UnfixedWire();
                    wire.FixedWireIn(transform);
                    wire.StartWirePartEffect(1, "0");
                    UpTestTube();
                    if(countWireInTestTube != 2) return;
                    countWireInTestTube = 0;
                    
                    CursorSkin.Instance.isUseClock = true;
                    _state = StateMiniTestTubeS3E5.NotActive;
                    _UIStagesControl.NextStep();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(10f, (delta) => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
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

        public void AddWireOnPaper()
        {
            _countWireOnPaper++;
            if(_countWireOnPaper == 2)
                _UIStagesControl.NextStep();
        }

        public void Restart()
        {
            RestartBase();
            _countWireOnPaper = 0;
            _state = StateMiniTestTubeS3E5.Empty;
        }
    }
}