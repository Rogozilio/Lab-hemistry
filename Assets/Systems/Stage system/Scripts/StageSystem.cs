using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace VirtualLab
{
    public class StageSystem : MonoBehaviour
    {
        // parameters 
        [SerializeField] List<AbstractStage> stages;

        [SerializeField] private UnityEvent switchStage;

        // data 
        int stageIndex = -1;
        int subStageIndex = -1;
        int titleIndex = -1;
        int buttonIndex = -1;

        void Start()
        {
            InitStages();
            SetStage(0);
        }

        void OnDestroy()
        {
            CleatStages();
        }

        public void OnStageCompleted(AbstractStage stage)
        {
            if (stage != stages[stageIndex])
                throw new UnityException("Non current stage was completed");
        }

        public void OnRestartConfirmed()
        {
            stages[stageIndex].Restart();
        }


        //  Stages  ----------------------------------------------------- 

        void InitStages()
        {
            foreach (AbstractStage stage in stages)
            {
                stage.onCompleted += OnStageCompleted;
            }
        }

        void ResetAllResettables()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] roots = scene.GetRootGameObjects();

            foreach (GameObject root in roots)
            {
                var objects = root.GetComponentsInChildren<IRestart>(true);

                foreach (var obj in objects)
                {
                    obj.Restart();
                }
            }
        }

        public void SetStage(int index)
        {
            if (stageIndex == index)
            {
                Debug.LogWarning("Setting the same stage");
                return;
            }

            if(stageIndex > -1)
                stages[stageIndex].StopStage();
            stageIndex = index;
            stages[index].StartStage();
            switchStage?.Invoke();
        }

        void CleatStages()
        {
            foreach (AbstractStage stage in stages)
            {
                stage.onCompleted -= OnStageCompleted;
            }
        }
    }
}