using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene2Sample4 : MiniTestTube
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

        public StateMiniTestTubeS2E4 GetState => _state;
        public int getCountLiquid => _countLiquid;
        public bool isOnlyForNH4Cl => IsOnlyForNH4Cl(); 

        public void Awake()
        {
            base.Awake();

            var waterColor = new Color32(63, 63, 63, 72);
            var sedimentColor = new Color32(130, 130, 130, 130);

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS2E4>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.Empty, TypeLiquid.MgCI2, Operator.More, 0,
                StateMiniTestTubeS2E4.MgCl2, () =>
                {
                    ChangeColorLiquid(waterColor);
                });

            byte stepNH4OH = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2, TypeLiquid.NH4OH, Operator.More, 0,
                StateMiniTestTubeS2E4.MgCl2_NH4OH, () =>
                {
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),sedimentColor, stepNH4OH--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2_NH4OH, TypeLiquid.NH4OH, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),sedimentColor, stepNH4OH--);
                });
            
            byte stepNH4Cl = 4;
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2_NH4OH, TypeLiquid.NH4CI, Operator.More, 0,
                StateMiniTestTubeS2E4.MgCl2_NH4OH_NH4Cl, () =>
                {
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),waterColor, stepNH4Cl--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS2E4.MgCl2_NH4OH_NH4Cl, TypeLiquid.NH4CI, Operator.More, 0,
                () =>
                {
                    ChangeColorLiquid(sediment.GetComponent<Renderer>(),waterColor, stepNH4Cl--);
                });
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
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
    }
}