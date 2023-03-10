using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample5 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS2E5
        {
            Empty,
            CH3COOH,

            [InspectorName("CH3COOH+methylOrange")]
            CH3COOH_methylOrange,

            [InspectorName("CH3COOH+methylOrange+CH3COONa")]
            CH3COOH_methylOrange_CH3COONa,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS2E5> _actionAddLiquid;
        private ActionAddPowder<StateMiniTestTubeS2E5> _actionAddPowder;

        private StateMiniTestTubeS2E5 _state;
        private Renderer _rendererSediment;
        private readonly Color _colorResult = new Color32(109, 12, 0, 80);
        private Color _originColorSediment;

        public StateMiniTestTubeS2E5 GetState => _state;
        public int getCountLiquid => _countLiquid;
        public int getCountPowder => _countPowder;

        public void Awake()
        {
            base.Awake();

            //var colorCH3COOH = new Color32(63, 63, 63, 72);
            var colorMethylOrange = new Color32(90, 3, 0, 70);

            _rendererSediment = sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E5>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E5.Empty, TypeLiquid.CH3COOH, Operator.More, 0,
                StateMiniTestTubeS2E5.CH3COOH, (bottleColor) => { ChangeColorLiquid(bottleColor); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E5.CH3COOH, TypeLiquid.CH3COOH, Operator.Equally, 6,
                () => { _stepStageSystem.NextStep(); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E5.CH3COOH, TypeLiquid.methylOrange, Operator.More, 0,
                StateMiniTestTubeS2E5.CH3COOH_methylOrange, () =>
                {
                    _stepStageSystem.NextStep();
                    ChangeColorLiquid(colorMethylOrange);
                });

            _actionAddPowder = new ActionAddPowder<StateMiniTestTubeS2E5>();

            _actionAddPowder.AddAction(StateMiniTestTubeS2E5.CH3COOH_methylOrange, TypePowder.CH3COONa, Operator.More,
                0, StateMiniTestTubeS2E5.CH3COOH_methylOrange_CH3COONa, () =>
                {
                    _levelLiquid.level += _step;
                    sediment.level += _step;
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 4f);
                    ChangeColorLiquid(_rendererSediment, new Color32(176, 56, 31, 150));
                });
            _actionAddPowder.AddAction(StateMiniTestTubeS2E5.CH3COOH_methylOrange_CH3COONa, TypePowder.CH3COONa,
                Operator.Equally, 3, () => { _stepStageSystem.NextStep(); });
            _actionAddPowder.AddAction(StateMiniTestTubeS2E5.CH3COOH_methylOrange_CH3COONa, TypePowder.CH3COONa,
                Operator.More,
                0, () =>
                {
                    _levelLiquid.level += _step;
                    sediment.level += _step / 2;
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        public override void AddPowder(PowderDrop powder)
        {
            base.AddPowder(powder);

            _actionAddPowder.Launch(ref _state, powder.typePowder, _countPowder);
        }

        public override void Stir(Transform stick)
        {
            base.Stir(stick);

            if (!_isStir) return;
            var stepSediment = (byte)(_rendererSediment.material.GetColor("_LiquidColor").a * 255 * 4);
            ChangeColorLiquid(_colorResult, stepSediment);

            ChangeColorLiquid(_rendererSediment, _colorResult, stepSediment);
            if (stepSediment == 64)
            {
                _countPowder = 4;
                _stepStageSystem.NextStep();
                _state = StateMiniTestTubeS2E5.NotActive;
                stick.GetComponent<StateItem>().ChangeState(StateItems.BackToMouse);
                stick.GetComponent<MoveMap>().StartToMove(3);
            }
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS2E5.Empty;
            sediment.level = 0;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}