using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample4 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS2E4
        {
            Empty,
            MgCl2,
            [InspectorName("MgCl2+NH4OH")] MgCl2_NH4OH,
            [InspectorName("MgCl2+NH4OH+NH4Cl")] MgCl2_NH4OH_NH4Cl,
            NotActive
        }

        public LevelLiquid sediment;

        private ActionAddLiquid<StateMiniTestTubeS2E4> _actionAddLiquid;

        private StateMiniTestTubeS2E4 _state;
        private Renderer _rendererSediment;
        private Color _originColorSediment;

        public StateMiniTestTubeS2E4 GetState => _state;
        public int getCountLiquid => _countLiquid;
        public bool isOnlyForNH4Cl => IsOnlyForNH4Cl();

        public void Awake()
        {
            base.Awake();

            _rendererSediment = sediment.GetComponent<Renderer>();
            _originColorSediment = _rendererSediment.material.GetColor("_LiquidColor");
            
            //var waterColor = new Color32(63, 63, 63, 72);
            var sedimentColor = new Color32(130, 130, 130, 180);

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E4>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.Empty, TypeLiquid.MgCI2, Operator.More, 0,
                StateMiniTestTubeS2E4.MgCl2, (waterColor) => { ChangeColorLiquid(waterColor); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2, TypeLiquid.MgCI2, Operator.Equally, 10,
                () => { _UIStagesControl.NextStep(); });

            byte stepNH4OH = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2, TypeLiquid.NH4OH, Operator.More, 0,
                StateMiniTestTubeS2E4.MgCl2_NH4OH,
                () => { ChangeColorLiquid(_rendererSediment, sedimentColor, stepNH4OH--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2_NH4OH, TypeLiquid.NH4OH, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(_rendererSediment, sedimentColor, stepNH4OH--);
                    if (stepNH4OH == 0)
                    {
                        _UIStagesControl.NextStep();
                        stepNH4OH = 4;
                    }
                });

            byte stepNH4Cl = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2_NH4OH, TypeLiquid.NH4CI, Operator.More, 0,
                StateMiniTestTubeS2E4.MgCl2_NH4OH_NH4Cl,
                (waterColor) => { ChangeColorLiquid(_rendererSediment, waterColor, stepNH4Cl--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2_NH4OH_NH4Cl, TypeLiquid.NH4CI, Operator.More, 0,
                (waterColor) =>
                {
                    ChangeColorLiquid(_rendererSediment, waterColor, stepNH4Cl--);
                    if (stepNH4Cl == 0)
                    {
                        _UIStagesControl.NextStep();
                        stepNH4Cl = 4;
                    }
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid, liquid.GetColor);
        }

        private bool IsOnlyForNH4Cl()
        {
            var testTubes = FindObjectsOfType<MiniTestTubeScene2Sample4>();

            foreach (var testTube in testTubes)
            {
                if (testTube.GetState == StateMiniTestTubeS2E4.MgCl2_NH4OH_NH4Cl)
                    return testTube.GetState == _state;
            }

            return true;
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS2E4.Empty;
            sediment.gameObject.SetActive(true);
            _rendererSediment.material.SetColor("_LiquidColor", _originColorSediment);
        }
    }
}