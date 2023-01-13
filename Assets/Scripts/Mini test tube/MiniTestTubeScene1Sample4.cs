using System.Collections;
using Liquid;
using Unity.VisualScripting;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTubeScene1Sample4 : MiniTestTube, IRestart
    {
        public enum StateMiniTestTubeS1E4
        {
            Empty,
            NH4CI,
            [InspectorName("NH4CI+NaOH")] NH4CI_NaOH,
            [InspectorName("NH4CI+NaOH+fire")] NH4CI_NaOH_fire,
            Na2CO3,
            [InspectorName("Na2CO3+HCI")] Na2CO3_HCI,
            NotActive
        }

        private ActionAddLiquid<StateMiniTestTubeS1E4> _actionAddLiquid;
        private StateMiniTestTubeS1E4 _state;
        private Coroutine _effervescenceLiquid;
        private Vector3 _originPositionBubble;
        private float _heightEffervescence;
        private float _multiply = 1;

        public StateMiniTestTubeS1E4 GetState => _state;
        public int GetCountLiquid => _countLiquid;

        public ParticleSystem Bubbles;
        public GameObject SweatyGlass;

        public void Awake()
        {
            base.Awake();

            _originPositionBubble = Bubbles.gameObject.transform.localPosition;
            
            var newColor = new Color32(172, 198, 219, 30);
            _actionAddLiquid = new ActionAddLiquid<StateMiniTestTubeS1E4>();

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.Empty, TypeLiquid.NH4CI, Operator.More, 0,
                StateMiniTestTubeS1E4.NH4CI,
                () =>
                {
                    ChangeColorLiquid(newColor);
                    ChangeOtherTestTube(StateMiniTestTubeS1E4.NotActive);
                });
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.NH4CI, TypeLiquid.NH4CI, Operator.Equally, 8,
                () => _stepStageSystem.NextStep());
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.NH4CI, TypeLiquid.NH4CI, Operator.More, 0,
                () => ChangeColorLiquid(newColor));
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.NH4CI, TypeLiquid.NaOH, Operator.More, 0,
                StateMiniTestTubeS1E4.NH4CI_NaOH);
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.NH4CI_NaOH, TypeLiquid.NaOH, Operator.Equally, 16,
                () => _stepStageSystem.NextStep());

            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.Empty, TypeLiquid.Na2CO3, Operator.More, 0,
                StateMiniTestTubeS1E4.Na2CO3,
                () => ChangeColorLiquid(newColor));
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.Na2CO3, TypeLiquid.Na2CO3, Operator.Equally, 10,
                () => _stepStageSystem.NextStep());
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.Na2CO3, TypeLiquid.Na2CO3, Operator.More, 0,
                () => ChangeColorLiquid(newColor));
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.Na2CO3, TypeLiquid.HCI, Operator.More, 0,
                StateMiniTestTubeS1E4.Na2CO3_HCI);
            _actionAddLiquid.AddAction(StateMiniTestTubeS1E4.Na2CO3_HCI, TypeLiquid.HCI, Operator.Equally, 14,
                () => _stepStageSystem.NextStep());
        }

        public override void SetStateMiniTestTube(int index)
        {
            _state = (StateMiniTestTubeS1E4)index;
        }

        public override void AddLiquid(LiquidDrop liquid)
        {
            base.AddLiquid(liquid);

            _actionAddLiquid.Launch(ref _state, liquid.typeLiquid, _countLiquid);
        }

        public void StartBurnLiquid()
        {
            StartCoroutine(BurnLiquid());
        }

        public void StartEffervescenceLiquid(float plusHeight)
        {
            _heightEffervescence += plusHeight * _multiply;
            _effervescenceLiquid ??= StartCoroutine(EffervescenceLiquid());
            _multiply++;
        }

        private IEnumerator BurnLiquid()
        {
            yield return new WaitForSeconds(4f);
            Bubbles.Play();

            var time = 0f;
            var bubblesVelocityOverLifetime = Bubbles.velocityOverLifetime;
          
            while (time < 0.3f)
            {
                var localPositionBubble = Bubbles.gameObject.transform.localPosition;
                Bubbles.gameObject.transform.localPosition = new Vector3(localPositionBubble.x,
                    localPositionBubble.y, levelLiquid.Plane.localPosition.z - 0.005f);
                Bubbles.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.up);

                var ratio = 0.02f;
                bubblesVelocityOverLifetime.x = transform.forward.x * ratio;
                bubblesVelocityOverLifetime.y = transform.forward.y * ratio;
                bubblesVelocityOverLifetime.z = transform.forward.z * ratio;

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            
            SweatyGlass.SetActive(true);
            _state = StateMiniTestTubeS1E4.NH4CI_NaOH_fire;

            var bubblesMain = Bubbles.main;
            var originStartLifetimeMultiplier = bubblesMain.startLifetimeMultiplier;
            while (bubblesMain.startLifetimeMultiplier > 0.01f)
            {
                var localPositionBubble = Bubbles.gameObject.transform.localPosition;
                Bubbles.gameObject.transform.localPosition = new Vector3(localPositionBubble.x,
                    localPositionBubble.y, levelLiquid.Plane.localPosition.z - 0.005f);
                Bubbles.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.up);

                var ratio = 0.02f;
                bubblesVelocityOverLifetime.x = transform.forward.x * ratio;
                bubblesVelocityOverLifetime.y = transform.forward.y * ratio;
                bubblesVelocityOverLifetime.z = transform.forward.z * ratio;
                
                bubblesMain.startLifetimeMultiplier -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            bubblesMain.startLifetimeMultiplier = originStartLifetimeMultiplier;
            Bubbles.Stop();
            _stepStageSystem.NextStep();
        }
        
        private void ChangeOtherTestTube(StateMiniTestTubeS1E4 newState)
        {
            var miniTestTubes = FindObjectsOfType<MiniTestTubeScene1Sample4>();

            foreach (var miniTestTube in miniTestTubes)
            {
                if (miniTestTube == this) continue;
                miniTestTube.SetStateMiniTestTube((int)newState); 
                return;
            }
        }

        private IEnumerator EffervescenceLiquid()
        {
            Bubbles.Play();

            var bubblesVelocityOverLifetime = Bubbles.velocityOverLifetime;
            while (_heightEffervescence > 0)
            {
                _heightEffervescence -= Time.fixedDeltaTime / 6f;

                var localPositionBubble = Bubbles.gameObject.transform.localPosition;
                Bubbles.gameObject.transform.localPosition = new Vector3(localPositionBubble.x,
                    localPositionBubble.y, levelLiquid.Plane.localPosition.z - 0.003f);
                Bubbles.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.up);

                bubblesVelocityOverLifetime.x = transform.forward.x * _heightEffervescence;
                bubblesVelocityOverLifetime.y = transform.forward.y * _heightEffervescence;
                bubblesVelocityOverLifetime.z = transform.forward.z * _heightEffervescence;

                yield return new WaitForFixedUpdate();
            }

            _effervescenceLiquid = null;
            Bubbles.Stop();
        }

        public void Restart()
        {
            RestartBase();
            _state = StateMiniTestTubeS1E4.Empty;
            _multiply = 1;
            _countLiquid = 0;
            _heightEffervescence = 0;
            Bubbles.Stop();
            SweatyGlass.SetActive(false);
            Bubbles.gameObject.transform.localPosition = _originPositionBubble;
        }
    }
}