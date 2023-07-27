using Granule;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;
using Wire;

namespace Mini_test_tube
{
    public class MiniTestTubeScene3Sample3 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS3E3
        {
            Empty,
            Pb_NO3_2,
            Pb_NO3_2_KI,
            Pb_NO3_2_KI_smooth,
            CH3COOH,
            CH3COOH_KI,
            CH3COOH_KI_Pb,
            CH3COOH_KI_Pb_corrosion,
            CH3COOH_KI_PbZn,
            CH3COOH_KI_PbZn_corrosion,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS3E3> _actionAddLiquid;
        private ActionAddGranule<StateMiniTestTubeS3E3> _actionAddGranule;
        private ActionAddWire<StateMiniTestTubeS3E3> _actionAddWire;

        private PlayerMotion _playerMotion;

        private StateMiniTestTubeS3E3 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public StateMiniTestTubeS3E3 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS3E3 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            _playerMotion = FindObjectOfType<PlayerMotion>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _rendererSediment.material.SetFloat("_SedimentMultiply", 4);
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS3E3>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.Empty, TypeLiquid.Pb_NO3_2, Operator.More, 0,
                StateMiniTestTubeS3E3.Pb_NO3_2, (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E3.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.Pb_NO3_2, TypeLiquid.Pb_NO3_2, Operator.Equally, 6,
                () => { _UIStagesControl.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.Pb_NO3_2, TypeLiquid.KI, Operator.More, 0,
                StateMiniTestTubeS3E3.Pb_NO3_2_KI, () =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    UpTestTube();
                    _UIStagesControl.NextStep();
                    sediment.level = levelLiquid.level / 1.2f;
                    _playerMotion.MoveToPoint(transform, 10);
                    ChangeColorLiquid(_rendererSediment, new Color32(188, 160, 0, 55));
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(188, 160, 0, 255), 5f, () =>
                    {
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS3E3.Pb_NO3_2_KI_smooth;
                        SetStateOtherMiniTestTube(StateMiniTestTubeS3E3.CH3COOH);
                        CursorSkin.Instance.isUseClock = false;
                    });
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.CH3COOH, TypeLiquid.CH3COOH, Operator.Equally, 1,
                (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E3.NotActive,
                        StateMiniTestTubeS3E3.Pb_NO3_2_KI_smooth,
                        StateMiniTestTubeS3E3.CH3COOH_KI_Pb_corrosion);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.CH3COOH, TypeLiquid.CH3COOH, Operator.Equally, 10,
                () => { _UIStagesControl.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.CH3COOH, TypeLiquid.KI, Operator.More, 0,
                StateMiniTestTubeS3E3.CH3COOH_KI, () => { });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.CH3COOH_KI, TypeLiquid.KI, Operator.Equally, 15,
                () =>
                {
                    _state = CheckPbInOtherTestTube()
                        ? StateMiniTestTubeS3E3.CH3COOH_KI_PbZn
                        : StateMiniTestTubeS3E3.CH3COOH_KI_Pb;
                    _UIStagesControl.NextStep();
                });
            // byte step_K3_Fe_CN_6_ = 3;
            // _actionAddLiquid.AddAction(StateMiniTestTubeS3E3.CH3COOH_KI_K3_Fe_CN_6_, TypeLiquid.K3_Fe_CN_6_, Operator.More, 0,
            //     () =>
            //     {
            //         ChangeColorLiquid(new Color32(229, 253, 42, 10), step_K3_Fe_CN_6_--);
            //         if (step_K3_Fe_CN_6_ == 0)
            //         {
            //             _UIStagesControl.NextStep();
            //             _state = StateMiniTestTubeS3E3.CH3COOH_KI_PbZn;
            //             step_K3_Fe_CN_6_ = 3;
            //         }
            //     });

            _actionAddWire = new ActionAddWire<StateMiniTestTubeS3E3>();

            _actionAddWire.AddAction(StateMiniTestTubeS3E3.CH3COOH_KI_Pb, TypeWire.Pb,
                StateMiniTestTubeS3E3.CH3COOH_KI_Pb_corrosion, (wire) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    wire.FixedWireIn(transform);
                    wire.PlayExistEffects();
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(4f, (delta) => { }, () =>
                    {
                        CursorSkin.Instance.isUseClock = false;
                        _UIStagesControl.NextStep();
                        SetStateOtherMiniTestTube(StateMiniTestTubeS3E3.CH3COOH,
                            StateMiniTestTubeS3E3.Pb_NO3_2_KI_smooth);
                        StartSmoothlyChangeColor(new Color32(80, 72, 13, 100), 30f, () => {});
                    });
                });

            _actionAddWire.AddAction(StateMiniTestTubeS3E3.CH3COOH_KI_PbZn, TypeWire.Pb,
                StateMiniTestTubeS3E3.CH3COOH_KI_PbZn_corrosion, (wire) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    wire.FixedWireIn(transform);
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyAction(4f, delta => {}, () =>
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

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS3E3.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }

        private void SetStateOtherMiniTestTube(StateMiniTestTubeS3E3 state, params StateMiniTestTubeS3E3[] excepts)
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample3>();

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

        private bool CheckPbInOtherTestTube()
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample3>();

            foreach (var tube in tubes)
            {
                if (tube.GetState == StateMiniTestTubeS3E3.CH3COOH_KI_Pb_corrosion) return true;
            }

            return false;
        }
    }
}