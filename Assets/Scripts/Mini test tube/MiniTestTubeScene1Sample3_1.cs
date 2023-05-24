using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample3_1 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS1E3_1
        {
            Empty,
            CrCl3,
            [InspectorName("CrCl3+NaOH")] CrCl3_NaOH,
            [InspectorName("CrCl3+NaOH/2")] CrCl3_NaOH_half,
            [InspectorName("CrCl3+NaOH/2+HCI")] CrCl3_NaOH_half_HCl,
            [InspectorName("CrCl3+NaOH/2+NaOH")] CrCl3_NaOH_half_NaOH,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS1E3_1> _actionAddLiquid;
        private StateMiniTestTubeS1E3_1 _state;

        private Renderer _rendererSediment;
        private Color _originColorSediment;

        private int _countNaOH;
        private int _countHCI;

        public StateMiniTestTubeS1E3_1 GetState => _state;
        public int GetCountLiquid => _countLiquid;
        public int GetCountNaOH => _countNaOH;
        public int GetCountHCI => _countHCI;

        public LevelLiquid Sediment;

        public void Awake()
        {
            base.Awake();

            _rendererSediment = Sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            
            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E3_1>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.Empty, TypeLiquid.CrCl3, Operator.More, 0,
                StateMiniTestTubeS1E3_1.CrCl3,
                (bottleColor) =>
                {
                    ChangeColorLiquid(bottleColor/*new Color32(0, 2, 31, 83)*/);
                    ChangeOtherTestTube(StateMiniTestTubeS1E3_1.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3, TypeLiquid.CrCl3, Operator.Equally, 10,
                () => { _UIStagesControl.NextStep(); });
            byte stepNaOH = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3, TypeLiquid.NaOH, Operator.More, 10,
                StateMiniTestTubeS1E3_1.CrCl3_NaOH,
                () =>
                {
                    Sediment.level = levelLiquid.level;
                    _rendererSediment.material.SetFloat("_IsWorldPosition", 1);
                    ChangeColorLiquid(new Color32(7, 12, 7, 183));
                    ChangeColorLiquid(_rendererSediment, new Color(0.115f, 0.115f, 0.115f, 1f), stepNaOH--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH, TypeLiquid.NaOH, Operator.More, 10,
                () =>
                {
                    Sediment.level = levelLiquid.level;
                    ChangeColorLiquid(_rendererSediment,  new Color(0.115f, 0.115f, 0.115f, 1f), stepNaOH--);
                    if (stepNaOH == 0)
                    {
                        _UIStagesControl.NextStep();
                        stepNaOH = 2;
                    }
                });
            byte stepNaOHhalf = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half, TypeLiquid.NaOH, Operator.More, 0,
                StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_NaOH,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(new Color32(2,4,2,130));
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0f), stepNaOHhalf--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_NaOH, TypeLiquid.NaOH, Operator.More, 0,
                () =>
                {
                    _countNaOH++;
                    ChangeColorLiquid(new Color32(2,4,2,130));
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0f), stepNaOHhalf--);
                    if (stepNaOHhalf == 0)
                    {
                        _UIStagesControl.NextStep();
                        stepNaOHhalf = 2;
                    }
                });
            byte stepHCI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_HCl,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(new Color32(2, 3, 10, 130));
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0f), stepHCI--);
                    ChangeOtherTestTube(StateMiniTestTubeS1E3_1.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_HCl, TypeLiquid.HCI, Operator.More, 0,
                () =>
                {
                    _countHCI++;
                    ChangeColorLiquid(new Color32(2, 3, 10, 130));
                    ChangeColorLiquid(_rendererSediment, new Color(0, 0, 0, 0), stepHCI--);
                    if (stepHCI == 0)
                    {
                        _UIStagesControl.NextStep();
                        ChangeOtherTestTube(StateMiniTestTubeS1E3_1.CrCl3_NaOH_half_NaOH);
                        stepHCI = 2;
                    }
                });
        }

        private void FixedUpdate()
        {
            PourOutLiquid(0.005f, _levelLiquid.level / 2f, Sediment);
        }
        
        private void ChangeOtherTestTube(StateMiniTestTubeS1E3_1 newState)
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample3_1>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube == this) continue;
                miniTestTube.SetStateMiniTestTube((int)newState); 
                return;
            }
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E3_1)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS1E3_1.Empty;
            _countNaOH = 0;
            _countHCI = 0;
            Sediment.level = 0;
            Sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}