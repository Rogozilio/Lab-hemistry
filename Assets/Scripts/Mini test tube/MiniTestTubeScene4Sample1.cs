using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;

namespace Mini_test_tube
{
    public class MiniTestTubeScene4Sample1 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS4E1
        {
            Empty,
            Hg_NO3_2,
            KI,
            KI_smooth,
            KIx2,
            KIx2_smooth,
            NotActive
        }
        
        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS4E1> _actionAddLiquid;

        private PlayerMotion playerMotion;

        private StateMiniTestTubeS4E1 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public StateMiniTestTubeS4E1 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS4E1 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            playerMotion = FindObjectOfType<PlayerMotion>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _rendererSediment.material.SetFloat("_SedimentMultiply", 3);
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS4E1>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS4E1.Empty, TypeLiquid.Hg_NO3_2, Operator.More, 0,
                StateMiniTestTubeS4E1.Hg_NO3_2, (waterColor) => { ChangeColorLiquid(waterColor); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E1.Hg_NO3_2, TypeLiquid.Hg_NO3_2, Operator.Equally, 5,
                () => { _UIStagesControl.NextStep(); });
            var colorKI = new Color32(231, 61, 5, 177);
            byte step_KI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E1.Hg_NO3_2, TypeLiquid.KI, Operator.More, 0,
                StateMiniTestTubeS4E1.KI, () => { ChangeColorLiquid(colorKI, step_KI--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E1.KI, TypeLiquid.KI, Operator.More, 0, () =>
            {
                ChangeColorLiquid(colorKI, step_KI--);
                if (step_KI == 0)
                {
                    UpTestTube();
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    _state = StateMiniTestTubeS4E1.KI_smooth;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(237, 16, 0, 128),
                        3f, () =>
                        {
                            CursorSkin.Instance.isUseClock = false;
                            _UIStagesControl.NextStep();
                            _state = StateMiniTestTubeS4E1.KIx2;
                        });
                    step_KI = 2;
                }
            });
            byte step_KIx2 = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E1.KIx2, TypeLiquid.KI, Operator.More, 0,
                (waterColor) =>
                {
                    ChangeColorLiquid(new Color32(231, 61, 5, 137), step_KIx2);
                    ChangeColorLiquid(_rendererSediment, new Color32(237, 16, 0, 88), step_KIx2--);
                    if (step_KIx2 == 0)
                    {
                        UpTestTube();
                        CursorSkin.Instance.isUseClock = true;
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS4E1.KIx2_smooth;
                        playerMotion.MoveToPoint(transform, 10);
                        StartSmoothlyChangeColor(waterColor, 1f);
                        StartSmoothlyChangeColor(_rendererSediment, waterColor, 
                            1f, () =>
                        {
                            _UIStagesControl.NextStep();
                            CursorSkin.Instance.isUseClock = false;
                            _state = StateMiniTestTubeS4E1.NotActive;
                        });
                        step_KIx2 = 6;
                    }
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS4E1.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}