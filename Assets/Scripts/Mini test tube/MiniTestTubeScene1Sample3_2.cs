using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample3_2 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS1E3_2
        {
            Empty,
            [InspectorName("AI2(SO4)3")]AI2SO43,
            [InspectorName("AI2(SO4)3+NaOH")] AI2SO43_NaOH,
            [InspectorName("AI2(SO4)3+NaOH/2")] AI2SO43_NaOH_half,
            [InspectorName("AI2(SO4)3+NaOH/2+HCI")] AI2SO43_NaOH_half_HCl,
            [InspectorName("AI2(SO4)3+NaOH/2+NaOH")] AI2SO43_NaOH_half_NaOH,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS1E3_2> _actionAddLiquid;
        private StateMiniTestTubeS1E3_2 _state;
        
        private Renderer _rendererSediment;
        private Color _originColorSediment;
        
        private int _countNaOH;
        private int _countHCI;

        public StateMiniTestTubeS1E3_2 GetState => _state;
        public int GetCountLiquid => _countLiquid;
        public int GetCountNaOH => _countNaOH;
        public int GetCountHCI => _countHCI;

        public LevelLiquid Sediment;

        public void Awake()
        {
            base.Awake();

            _rendererSediment = Sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            
            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E3_2>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.Empty, TypeLiquid.Al2_SO4_3, Operator.More, 0,
                StateMiniTestTubeS1E3_2.AI2SO43,
                (colorWater) =>
                {
                    ChangeColorLiquid(colorWater);
                    ChangeOtherTestTube(StateMiniTestTubeS1E3_2.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43, TypeLiquid.Al2_SO4_3, Operator.Equally, 10,
                () => { _stepStageSystem.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43, TypeLiquid.NaOH, Operator.More, 10,
                StateMiniTestTubeS1E3_2.AI2SO43_NaOH,
                () =>
                {
                    Sediment.level = levelLiquid.level / 1.5f;
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 2);
                    _rendererSediment.material.SetFloat("_IsWorldPosition", 1);
                    ChangeColorLiquid(_rendererSediment, new Color32(191, 172, 138, 230));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH, TypeLiquid.NaOH, Operator.Equally, 12,
                () => { Sediment.level = levelLiquid.level / 1.1f; _stepStageSystem.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH, TypeLiquid.NaOH, Operator.More, 10,
                () =>
                {
                    Sediment.level = levelLiquid.level / 1.1f;
                });

            byte stepNaOH = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half, TypeLiquid.NaOH, Operator.More, 0,
                StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_NaOH,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0), stepNaOH--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_NaOH, TypeLiquid.NaOH, Operator.More, 0,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0), stepNaOH--);
                    if (stepNaOH == 0)
                    {
                        _stepStageSystem.NextStep();
                        stepNaOH = 2;
                    }
                });
            byte stepHCI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_HCl,
                () =>
                {
                    _countHCI++;
                    ChangeOtherTestTube(StateMiniTestTubeS1E3_2.NotActive);
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0), stepHCI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_HCl, TypeLiquid.HCI, Operator.More, 0,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0), stepHCI--);
                    if (stepHCI == 0)
                    {
                        _stepStageSystem.NextStep();
                        ChangeOtherTestTube(StateMiniTestTubeS1E3_2.AI2SO43_NaOH_half_NaOH);
                        stepHCI = 2;
                    }
                });
        }

        private void FixedUpdate()
        {
            PourOutLiquid(0.005f, _levelLiquid.level / 2f, Sediment);
        }
        
        private void ChangeOtherTestTube(StateMiniTestTubeS1E3_2 newState)
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample3_2>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube == this) continue;
                miniTestTube.SetStateMiniTestTube((int)newState); 
                return;
            }
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E3_2)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }
        
        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS1E3_2.Empty;
            _countNaOH = 0;
            _countHCI = 0;
            Sediment.level = 0;
            Sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}