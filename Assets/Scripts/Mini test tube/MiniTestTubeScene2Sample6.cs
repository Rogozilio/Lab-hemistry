using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample6 : MiniTestTube
    {
        public enum StateMiniTestTubeS2E6
        {
            Empty,
            NH4OH,
            [InspectorName("NH4OH+phenolphthalein")] NH4OH_phenolphthalein,
            [InspectorName("NH4OH+phenolphthalein+NH4Cl")] NH4OH_phenolphthalein_NH4Cl
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS2E6> _actionAddLiquid;
        private ActionAddPowder<StateMiniTestTubeS2E6> _actionAddPowder;

        private StateMiniTestTubeS2E6 _state;
        private Renderer _rendererSediment;
        private readonly Color _colorResult = new Color32(104, 13, 47, 60);

        public StateMiniTestTubeS2E6 GetState => _state;
        public int getCountLiquid => _countLiquid;
        public int getCountPowder => _countPowder;
        public bool isOnlyForNH4Cl => IsOnlyForNH4Cl();

        public void Awake()
        {
            base.Awake();

            var colorNH4OH = new Color32(63, 63, 63, 72);
            var colorPhenolphthalein = new Color32(87, 0, 33, 60);

            _rendererSediment = sediment.GetComponent<Renderer>();

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E6>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E6.Empty, TypeLiquid.NH4OH, Operator.More, 0,
                StateMiniTestTubeS2E6.NH4OH, () =>
                {
                    ChangeColorLiquid(colorNH4OH);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E6.NH4OH, TypeLiquid.Phenolphthalein, Operator.More, 0,
                StateMiniTestTubeS2E6.NH4OH_phenolphthalein, () =>
                {
                    ChangeColorLiquid(colorPhenolphthalein);
                });

            _actionAddPowder = new ActionAddPowder<StateMiniTestTubeS2E6>();
            
            _actionAddPowder.AddAction(StateMiniTestTubeS2E6.NH4OH_phenolphthalein, TypePowder.NH4CI, Operator.More,
                0, StateMiniTestTubeS2E6.NH4OH_phenolphthalein_NH4Cl, () =>
                {
                    _levelLiquid.level += _step;
                    sediment.level += _step;
                    _rendererSediment.material.SetFloat("_SedimentMultiply", 4f);
                    ChangeColorLiquid(_rendererSediment, new Color32(236, 93, 188, 150));
                });
            _actionAddPowder.AddAction(StateMiniTestTubeS2E6.NH4OH_phenolphthalein_NH4Cl, TypePowder.NH4CI, Operator.More,
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

        private bool IsOnlyForNH4Cl()
        {
            var testTubes = FindObjectsOfType<MiniTestTubeScene2Sample6>();

            foreach (var testTube in testTubes)
            {
                if (testTube.GetState == StateMiniTestTubeS2E6.NH4OH_phenolphthalein_NH4Cl)
                    return testTube.GetState == _state;
            }

            return true;
        }
    }
}