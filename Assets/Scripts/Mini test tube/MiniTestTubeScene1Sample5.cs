using System.Collections;
using Liquid;
using Unity.VisualScripting;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample5 : MiniTestTube
    {
        public enum StateMiniTestTubeS1E5
        {
            Empty,
            [InspectorName("Pb(NO3)2")] Pb_NO3_2,
            [InspectorName("Pb(NO3)2+KI")] Pb_NO3_2_KI,
            [InspectorName("Pb(NO3)2+K2CrO4")] Pb_NO3_2_K2CrO4,
            K2CrO4,
            [InspectorName("K2CrO4+AgNO3")] K2CrO4_AgNO3,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS1E5> _actionAddLiquid;
        private StateMiniTestTubeS1E5 _state;

        public StateMiniTestTubeS1E5 GetState => _state;
        public int GetCountLiquid => _countLiquid;

        public void Awake()
        {
            base.Awake();

            var clearColor = new Color32(172, 198, 219, 30);

            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E5>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Empty, TypeLiquid.Pb_NO3_2, Operator.More, 0,
                StateMiniTestTubeS1E5.Pb_NO3_2, () =>
                {
                    CheckTwoMiniTestTubeWithPbNO3_2();
                    ChangeColorLiquid(clearColor);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Pb_NO3_2, TypeLiquid.Pb_NO3_2, Operator.More, 0,
                () => { ChangeColorLiquid(clearColor); });
            byte stepKI = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Pb_NO3_2, TypeLiquid.KI, Operator.More, 0,
                StateMiniTestTubeS1E5.Pb_NO3_2_KI,
                () =>
                {
                    CheckKIAndK2CrO4();
                    ChangeColorLiquid(new Color32(203, 124, 0, 157), stepKI--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Pb_NO3_2_KI, TypeLiquid.KI, Operator.More, 0,
                () => { ChangeColorLiquid(new Color32(203, 124, 0, 157), stepKI--); });
            byte stepK2Cro4 = 2;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Pb_NO3_2, TypeLiquid.K2CrO4, Operator.More, 0,
                StateMiniTestTubeS1E5.Pb_NO3_2_K2CrO4,
                () =>
                {
                    CheckKIAndK2CrO4();
                    ChangeColorLiquid(new Color32(145, 145, 0, 157), stepK2Cro4--);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Pb_NO3_2_K2CrO4, TypeLiquid.K2CrO4, Operator.More, 0,
                () => { ChangeColorLiquid(new Color32(145, 145, 0, 157), stepK2Cro4--); });

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.Empty, TypeLiquid.K2CrO4, Operator.More, 0,
                StateMiniTestTubeS1E5.K2CrO4, () =>
                {
                    CheckFirstK2CrO4();
                    ChangeColorLiquid(new Color32(94, 71, 0, 69));
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.K2CrO4, TypeLiquid.K2CrO4, Operator.More, 0,
                () => { ChangeColorLiquid(new Color32(94, 71, 0, 69)); });
            byte stepAgNO3 = 3;
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.K2CrO4, TypeLiquid.AgNO3, Operator.More, 0,
                StateMiniTestTubeS1E5.K2CrO4_AgNO3,
                () => { ChangeColorLiquid(new Color32(102, 10, 6, 142), stepAgNO3--); });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E5.K2CrO4_AgNO3, TypeLiquid.AgNO3, Operator.More, 0,
                () => { ChangeColorLiquid(new Color32(102, 10, 6, 142), stepAgNO3--); });
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E5)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }

        private void CheckTwoMiniTestTubeWithPbNO3_2()
        {
            var countPbNO3_3 = 0;
            MiniTestTubeScene1Sample5 last = null;
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample5>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube.GetState is StateMiniTestTubeS1E5.Pb_NO3_2 or StateMiniTestTubeS1E5.Pb_NO3_2_KI
                    or StateMiniTestTubeS1E5.Pb_NO3_2_K2CrO4)
                    countPbNO3_3++;
                else
                {
                    last = miniTestTube;
                }
            }

            if (countPbNO3_3 != 2) return;

            last.SetStateMiniTestTube((int)StateMiniTestTubeS1E5.K2CrO4);
        }

        private void CheckFirstK2CrO4()
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample5>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube.GetState == StateMiniTestTubeS1E5.Empty)
                {
                    miniTestTube.SetStateMiniTestTube((int)StateMiniTestTubeS1E5.Pb_NO3_2);
                }
            }
        }

        private void CheckKIAndK2CrO4()
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample5>();
            MiniTestTubeScene1Sample5 Pb_NoO3_2 = null;
            var isKI = false;
            var isk2CrO4 = false;
            
            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube.GetState == StateMiniTestTubeS1E5.Pb_NO3_2)
                {
                    Pb_NoO3_2 = miniTestTube;
                }

                if (miniTestTube.GetState == StateMiniTestTubeS1E5.Pb_NO3_2_KI)
                {
                    isKI = true;
                }
                if (miniTestTube.GetState == StateMiniTestTubeS1E5.Pb_NO3_2_K2CrO4)
                {
                    isk2CrO4 = true;
                }
            }

            if(isKI) Pb_NoO3_2?.SetStateMiniTestTube((int)StateMiniTestTubeS1E5.Pb_NO3_2_K2CrO4);
            if(isk2CrO4) Pb_NoO3_2?.SetStateMiniTestTube((int)StateMiniTestTubeS1E5.Pb_NO3_2_KI);
        }
    }
}