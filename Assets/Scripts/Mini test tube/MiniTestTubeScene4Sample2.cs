using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;

namespace Mini_test_tube
{
    public class MiniTestTubeScene4Sample2 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS4E2
        {
            Empty,
            CuSO4,
            NH4OH,
            NH4OH_smooth,
            NH4OHx2,
            NH4OHx2_smooth,
            NotActive
        }
        
        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS4E2> _actionAddLiquid;

        private PlayerMotion playerMotion;

        private StateMiniTestTubeS4E2 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public StateMiniTestTubeS4E2 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS4E2 SetState
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

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS4E2>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS4E2.Empty, TypeLiquid.CuSO4, Operator.More, 0,
                StateMiniTestTubeS4E2.CuSO4, () => { ChangeColorLiquid(new Color32(12, 58, 50, 80)); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E2.CuSO4, TypeLiquid.CuSO4, Operator.Equally, 6,
                () => { _UIStagesControl.NextStep(); });
            var colorNH4OH = new Color32(0, 2, 56, 150);
            byte step_NH4OH = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E2.CuSO4, TypeLiquid.NH4OH, Operator.More, 0,
                StateMiniTestTubeS4E2.NH4OH, () => { ChangeColorLiquid(colorNH4OH, step_NH4OH--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E2.NH4OH, TypeLiquid.NH4OH, Operator.More, 0, () =>
            {
                ChangeColorLiquid(colorNH4OH, step_NH4OH--);
                if (step_NH4OH == 0)
                {
                    UpTestTube();
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    _state = StateMiniTestTubeS4E2.NH4OH_smooth;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(25, 107, 229, 103),
                        3f, () =>
                        {
                            _UIStagesControl.NextStep();
                            CursorSkin.Instance.isUseClock = false;
                            _state = StateMiniTestTubeS4E2.NH4OHx2;
                        });
                    step_NH4OH = 2;
                }
            });
            byte step_NH4OHx2 = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E2.NH4OHx2, TypeLiquid.NH4OH, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(new Color32(0, 2, 56, 170), step_NH4OHx2);
                    ChangeColorLiquid(_rendererSediment, new Color32(25, 107, 229, 63), step_NH4OHx2--);
                    if (step_NH4OHx2 == 0)
                    {
                        UpTestTube();
                        CursorSkin.Instance.isUseClock = true;
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS4E2.NH4OHx2_smooth;
                        playerMotion.MoveToPoint(transform, 10);
                        StartSmoothlyChangeColor(new Color32(0, 2, 56, 190), 1f);
                        StartSmoothlyChangeColor(_rendererSediment, new Color32(25, 107, 229, 0), 
                            1f, () =>
                        {
                            _UIStagesControl.NextStep();
                            CursorSkin.Instance.isUseClock = false;
                            _state = StateMiniTestTubeS4E2.NotActive;
                        });
                        step_NH4OHx2 = 6;
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
            _state = StateMiniTestTubeS4E2.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}