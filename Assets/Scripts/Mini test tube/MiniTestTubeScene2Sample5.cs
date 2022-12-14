using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample5 : MiniTestTube
    {
        public enum StateMiniTestTubeS2E5
        {
            Empty,
            CH3COOH,
            [InspectorName("CH3COOH+methylOrange")] CH3COOH_methylOrange,
            [InspectorName("CH3COOH+methylOrange+CH3COONa")] CH3COOH_methylOrange_CH3COONa
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS2E5> _actionAddLiquid;
        private ActionAddPowder<StateMiniTestTubeS2E5> _actionAddPowder;

        private StateMiniTestTubeS2E5 _state;
        private Renderer _rendererSediment;
        private readonly Color _colorResult = new Color32(58, 6, 0, 80);

        public StateMiniTestTubeS2E5 GetState => _state;
        public int getCountLiquid => _countLiquid;
        public int getCountPowder => _countPowder;
        public bool isOnlyForCH3COONa => IsOnlyForCH3COONa();

        public void Awake()
        {
            base.Awake();

            var colorCH3COOH = new Color32(63, 63, 63, 72);
            var colorMethylOrange = new Color32(90, 3, 0, 70);

            _rendererSediment = sediment.GetComponent<Renderer>();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E5>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E5.Empty, TypeLiquid.CH3COOH, Operator.More, 0,
                StateMiniTestTubeS2E5.CH3COOH, () =>
                {
                    ChangeColorLiquid(colorCH3COOH);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E5.CH3COOH, TypeLiquid.methylOrange, Operator.More, 0,
                StateMiniTestTubeS2E5.CH3COOH_methylOrange, () =>
                {
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
            _actionAddPowder.AddAction(StateMiniTestTubeS2E5.CH3COOH_methylOrange_CH3COONa, TypePowder.CH3COONa, Operator.More,
                0, () =>
                {
                    _levelLiquid.level += _step;
                    sediment.level += _step / 2;
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }
        
        public override void AddPowder(PowderDrop powder)
        {
            base.AddPowder(powder);

            _actionAddPowder.Launch(ref _state, powder.typePowder, _countPowder);
        }
        
        public override void Stir(Transform stick)
        {
            base.Stir(stick);

            if(!_isStir) return;
            var stepSediment = (byte)(_rendererSediment.material.GetColor("_LiquidColor").a * 255 * 4);
            ChangeColorLiquid(_colorResult, stepSediment);
            
            ChangeColorLiquid(_rendererSediment, _colorResult, stepSediment);
        }

        private bool IsOnlyForCH3COONa()
        {
            var testTubes = FindObjectsOfType<MiniTestTubeScene2Sample5>();

            foreach (var testTube in testTubes)
            {
                if (testTube.GetState == StateMiniTestTubeS2E5.CH3COOH_methylOrange_CH3COONa)
                    return testTube.GetState == _state;
            }

            return true;
        }
    }
}