using Granule;
using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;
using Wire;

namespace Mini_test_tube
{
    public class MiniTestTubeScene3Sample2 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS3E2
        {
            Empty,
            К3_Fe_CN_6_,
            К3_Fe_CN_6_FeCl3,
            К3_Fe_CN_6_FeCl3_smooth,
            H2O,
            H2O_H2SO4,
            H2O_H2SO4_К3_Fe_CN_6_,
            H2O_H2SO4_К3_Fe_CN_6_Zn,
            H2O_H2SO4_К3_Fe_CN_6_Zn_corrosion,
            H2O_H2SO4_К3_Fe_CN_6_Sn,
            H2O_H2SO4_К3_Fe_CN_6_Sn_smooth,
            NotActive
        }

        public LevelLiquid sediment;
        public LevelLiquid crystal;

        private ActionAddLiquid<StateMiniTestTubeS3E2> _actionAddLiquid;
        private ActionAddGranule<StateMiniTestTubeS3E2> _actionAddGranule;
        private ActionAddWire<StateMiniTestTubeS3E2> _actionAddWire;

        private ClickMouseItem _clickMouseItem;

        private PlayerMotion _playerMotion;

        private StateMiniTestTubeS3E2 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;
        private Renderer _rendererCrystal;
        private Color _originColorCrystal;

        public StateMiniTestTubeS3E2 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS3E2 SetState
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
            _rendererCrystal = crystal.GetComponent<Renderer>();
            _originColorCrystal = _rendererCrystal.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS3E2>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.Empty, TypeLiquid.K3_Fe_CN_6_, Operator.More, 0,
                StateMiniTestTubeS3E2.К3_Fe_CN_6_, (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E2.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.К3_Fe_CN_6_, TypeLiquid.K3_Fe_CN_6_, Operator.Equally, 5,
                () => { _UIStagesControl.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.К3_Fe_CN_6_, TypeLiquid.FeCl3, Operator.More, 0,
                StateMiniTestTubeS3E2.К3_Fe_CN_6_FeCl3, () =>
                {
                    UpTestTube();
                    _UIStagesControl.NextStep();
                    sediment.level = levelLiquid.level;
                    _playerMotion.MoveToPoint(transform, 10);
                    ChangeColorLiquid(new Color32(0, 4, 20, 100));
                    ChangeColorLiquid(_rendererSediment, new Color32(1, 1, 7, 50));
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(1, 1, 7, 222), 5f);
                    StartSmoothlyChangeColor(new Color32(0, 4, 20, 235), 5f, () =>
                    {
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS3E2.К3_Fe_CN_6_FeCl3_smooth;
                        SetStateOtherMiniTestTube(StateMiniTestTubeS3E2.H2O);
                    });
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O, TypeLiquid.H2O, Operator.Equally, 1,
                (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor);
                    SetStateOtherMiniTestTube(StateMiniTestTubeS3E2.NotActive,
                        StateMiniTestTubeS3E2.К3_Fe_CN_6_FeCl3_smooth,
                        StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Zn_corrosion);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O, TypeLiquid.H2O, Operator.Equally, 20,
                () => { _UIStagesControl.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O, TypeLiquid.H2SO4, Operator.More, 0,
                StateMiniTestTubeS3E2.H2O_H2SO4, () => { });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O_H2SO4, TypeLiquid.H2SO4, Operator.Equally, 23,
                () => { _UIStagesControl.NextStep(); });
            byte step_H2SO4 = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O_H2SO4, TypeLiquid.K3_Fe_CN_6_, Operator.More, 0,
                StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_,
                () => { ChangeColorLiquid(new Color32(229, 253, 42, 10), step_H2SO4--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_, TypeLiquid.K3_Fe_CN_6_,
                Operator.Equally, 26, () =>
                {
                    _state = CheckZnInOtherTestTube()
                        ? StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Sn
                        : StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Zn;
                    _UIStagesControl.NextStep();
                    step_H2SO4 = 3;
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_, TypeLiquid.K3_Fe_CN_6_,
                Operator.More, 0, () => { ChangeColorLiquid(new Color32(229, 253, 42, 10), step_H2SO4--); });

            _actionAddGranule = new ActionAddGranule<StateMiniTestTubeS3E2>();
            _actionAddWire = new ActionAddWire<StateMiniTestTubeS3E2>();

            _actionAddWire.AddAction(StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Zn, TypeWire.Fe,
                StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Zn_corrosion, (wire) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    wire.FixedWireIn(transform);
                    wire.GetGranule.FixedGranuleIn(transform);
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    wire.GetGranule.PlayBubble();

                    StartSmoothlyAction(5f, (delta) =>
                    {
                        wire.GetGranule.ChangeBubbleRadius(Mathf.Lerp(0.1f, 0.5f, delta));
                        wire.GetGranule.ChangeBubbleCountEmission(Mathf.Lerp(100, 1000, delta));
                        wire.GetGranule.ChangeBubbleMeshCountEmission(Mathf.Lerp(0, 1000, delta));
                    }, () =>
                    {
                        StartSmoothlyChangeColor(new Color32(102, 128, 42, 127), 10f, () =>
                        {
                            CursorSkin.Instance.isUseClock = false;
                            _UIStagesControl.NextStep();
                            SetStateOtherMiniTestTube(StateMiniTestTubeS3E2.H2O,
                                StateMiniTestTubeS3E2.К3_Fe_CN_6_FeCl3_smooth);
                        });
                    });
                });

            _actionAddWire.AddAction(StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Sn, TypeWire.Fe,
                StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Sn_smooth, (wire) =>
                {
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    wire.FixedWireIn(transform);
                    UpTestTube();
                    _playerMotion.MoveToPoint(transform, 10);
                    wire.PlayExistEffects();
                    crystal.level = levelLiquid.level;
                    StartSmoothlyChangeColor(_rendererCrystal, new Color32(5, 26, 197, 100), 10f);
                    StartSmoothlyChangeColor(new Color32(40, 149, 53, 43), 10f, () =>
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
            _state = StateMiniTestTubeS3E2.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
            _rendererCrystal.material.SetColor("_LiquidColor", _originColorCrystal);
        }

        private void SetStateOtherMiniTestTube(StateMiniTestTubeS3E2 state, params StateMiniTestTubeS3E2[] excepts)
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample2>();

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

        private bool CheckZnInOtherTestTube()
        {
            var tubes = FindObjectsOfType<MiniTestTubeScene3Sample2>();

            foreach (var tube in tubes)
            {
                if (tube.GetState == StateMiniTestTubeS3E2.H2O_H2SO4_К3_Fe_CN_6_Zn_corrosion) return true;
            }

            return false;
        }
    }
}