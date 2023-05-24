using Liquid;
using UnityEngine;
using VirtualLab.PlayerMotion;

namespace Mini_test_tube
{
    public class MiniTestTubeScene4Sample3 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS4E3
        {
            Empty,
            K4_Fe_CN_6_,
            K4_Fe_CN_6_NiCl2,
            K4_Fe_CN_6_NiCl2_smooth,
            K4_Fe_CN_6_NiCl2_NH4OH,
            K4_Fe_CN_6_NiCl2_NH4OH_smooth,
            NotActive
        }
        
        public LevelLiquid sediment;
        public LevelLiquid crystals;

        private ActionAddLiquid<StateMiniTestTubeS4E3> _actionAddLiquid;

        private PlayerMotion playerMotion;

        private StateMiniTestTubeS4E3 _state;

        private Renderer _rendererSediment;
        private Renderer _rendererCrystal;
        private Color _originColorSediment;
        private Color _originColorCrystal;

        public StateMiniTestTubeS4E3 GetState => _state;
        public int getCountLiquid => _countLiquid;

        public StateMiniTestTubeS4E3 SetState
        {
            set => _state = value;
        }

        public void Awake()
        {
            base.Awake();

            playerMotion = FindObjectOfType<PlayerMotion>();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _rendererCrystal = crystals.GetComponent<Renderer>();
            _rendererSediment.material.SetFloat("_SedimentMultiply", 0.3f);
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            _originColorCrystal = _rendererCrystal.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS4E3>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS4E3.Empty, TypeLiquid.K4_Fe_CN_6_, Operator.More, 0,
                StateMiniTestTubeS4E3.K4_Fe_CN_6_, (bottleColor) => { ChangeColorLiquid(bottleColor); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E3.K4_Fe_CN_6_, TypeLiquid.K4_Fe_CN_6_, Operator.Equally, 5,
                () => { _UIStagesControl.NextStep(); });
            byte step_NiCl2 = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E3.K4_Fe_CN_6_, TypeLiquid.NiCl2, Operator.More, 0,
                StateMiniTestTubeS4E3.K4_Fe_CN_6_NiCl2, () =>
                {
                    ChangeColorLiquid(_rendererSediment,new Color32(63, 243, 76, 60));
                    sediment.level = levelLiquid.level / step_NiCl2--;
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E3.K4_Fe_CN_6_NiCl2, TypeLiquid.NiCl2, Operator.More, 0, () =>
            {
                ChangeColorLiquid(new Color32(26, 85, 27, 50), step_NiCl2);
                sediment.level = levelLiquid.level / step_NiCl2--;
                if (step_NiCl2 == 0)
                {
                    UpTestTube();
                    CursorSkin.Instance.isUseClock = true;
                    _UIStagesControl.NextStep();
                    _state = StateMiniTestTubeS4E3.K4_Fe_CN_6_NiCl2_smooth;
                    playerMotion.MoveToPoint(transform, 10);
                    StartSmoothlyChangeColor(new Color32(26, 85, 27, 192), 5f);
                    StartSmoothlyChangeColor(_rendererSediment, new Color32(63, 243, 76, 127), 5f);
                    StartSmoothlyAction(5f, delta =>
                    {
                        var value = Mathf.Lerp(0.3f, 2.5f, delta);
                        _rendererSediment.material.SetFloat("_SedimentMultiply", value);
                    }, () =>
                    {
                        _UIStagesControl.NextStep();
                        CursorSkin.Instance.isUseClock = false;
                        _state = StateMiniTestTubeS4E3.K4_Fe_CN_6_NiCl2_NH4OH;
                    });
                    step_NiCl2 = 6;
                }
            });
            byte step_NH4OH = 6;
            _actionAddLiquid.AddAction(StateMiniTestTubeS4E3.K4_Fe_CN_6_NiCl2_NH4OH, TypeLiquid.NH4OH, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(new Color32(199, 210, 231, 20), step_NH4OH);
                    ChangeColorLiquid(_rendererSediment, new Color32(63, 243, 76, 255), step_NH4OH--);
                    if (step_NH4OH == 0)
                    {
                        UpTestTube();
                        CursorSkin.Instance.isUseClock = true;
                        _UIStagesControl.NextStep();
                        _state = StateMiniTestTubeS4E3.K4_Fe_CN_6_NiCl2_NH4OH_smooth;
                        playerMotion.MoveToPoint(transform, 10);
                        StartSmoothlyChangeColor(_rendererCrystal, new Color32(41, 43, 198, 200), 
                            8f, () =>
                        {
                            CursorSkin.Instance.isUseClock = false;
                            _UIStagesControl.NextStep();
                            _state = StateMiniTestTubeS4E3.NotActive;
                        });
                        step_NH4OH = 6;
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
            _state = StateMiniTestTubeS4E3.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
            _rendererSediment.material.SetFloat("_SedimentMultiply", 0.3f);
            _rendererCrystal.material.SetColor("_LiquidColor", _originColorCrystal);
        }
    }
}