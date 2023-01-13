using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace VirtualLab
{
    public class StageSystem : MonoBehaviour
    {
        // parameters 
        [SerializeField] List<AbstractStage> stages;
        [SerializeField] List<GameObject> stageTitles;

        [SerializeField] RestartDialog restartDialog;
        [SerializeField] StepStageSystem stepStageSystem;

        // connections 
        List<StageButton> buttons = new List<StageButton>();

        // data 
        int stageIndex = -1;
        int subStageIndex = -1;
        int titleIndex = -1;
        int buttonIndex = -1;


        void Awake()
        {
            GetComponentsInChildren<StageButton>(buttons);
        }

        void Start()
        {
            if (stages.Count != stageTitles.Count /*|| stages.Count != buttons.Count*/)
                throw new UnityException("Different number of stage components");

            InitStageTitles();
            InitButtons();
            InitStages();
            _SetStage(0);

            SwitchSubStageButton(false);
        }

        void OnDestroy()
        {
            CleatStages();
        }


        //  Events  ----------------------------------------------------- 
        public void OnMouseEnterButton(int index)
        {
            SetStageTitle(index);
        }

        public void OnMouseExit()
        {
            var index = buttons[stageIndex].GetSubStages.Count > 0 ? subStageIndex : stageIndex;
            SetStageTitle(index);
        }

        public void OnStageCompleted(AbstractStage stage)
        {
            if (stage != stages[stageIndex])
                throw new UnityException("Non current stage was completed");

            MoveToNextStage();
        }

        public void OnRestartConfirmed()
        {
            stages[buttonIndex].Restart();
        }


        //  Stages  ----------------------------------------------------- 
        AbstractStage CurrentStage => stages[stageIndex];

        void InitStages()
        {
            foreach (AbstractStage stage in stages)
            {
                stage.onCompleted += OnStageCompleted;
            }
        }

        void MoveToNextStage()
        {
            if (stageIndex < stages.Count - 1)
            {
                SetStage(stageIndex + 1);
            }
        }

        void Restart()
        {
            CurrentStage.StopStage();
            stageIndex = -1;

            ResetAllResettables();

            foreach (AbstractStage stage in stages)
            {
                stage.Restart();
            }

            _SetStage(0);
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

            // if (index == 0) 
            // {
            //     restartDialog.Show(OnRestartConfirmed); 
            //     return; 
            // }

            _SetStage(index);
        }

        void _SetStage(int index)
        {
            var isSubStage = IsSubStageButton(index);
            var oldIndex = (isSubStage) ? subStageIndex : stageIndex;

            if (oldIndex != -1)
            {
                stages[oldIndex].StopStage();
                buttons[oldIndex].SetState(StageButton.State.Active);

                if (!isSubStage && subStageIndex != -1)
                {
                    stages[subStageIndex].StopStage();
                    buttons[subStageIndex].SetState(StageButton.State.Active);
                }
                    
                SwitchSubStageButton(false, oldIndex);
            }

            if (isSubStage) subStageIndex = index; else stageIndex = index;
            buttonIndex = index;
            
            stages[index].StartStage();
            buttons[index].SetState(StageButton.State.Current);
            stepStageSystem.SwitchStage(stages[index].name.Split(" ")[1]);
            SwitchSubStageButton(true, index);
        }

        void CleatStages()
        {
            foreach (AbstractStage stage in stages)
            {
                stage.onCompleted -= OnStageCompleted;
            }
        }


        //  Stage titles  ----------------------------------------------- 
        GameObject CurrentStageTitle => stageTitles[titleIndex];

        void InitStageTitles()
        {
            stageTitles[0].SetActive(true);
            titleIndex = 0;

            for (int i = 1; i < stageTitles.Count; i++)
            {
                stageTitles[i].SetActive(false);
            }
        }

        void SetStageTitle(int index)
        {
            CurrentStageTitle.SetActive(false);
            titleIndex = index;
            CurrentStageTitle.SetActive(true);
        }


        //  Buttons  ---------------------------------------------------- 
        StageButton CurrentButton => buttons[buttonIndex];

        void InitButtons()
        {
            GiveButtonsIndices();
            GiveButtonsState();
        }

        void GiveButtonsIndices()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetIndex(i);
            }
        }

        void GiveButtonsState()
        {
            buttons[0].SetState(StageButton.State.Current);
            buttonIndex = 0;

            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].SetState(StageButton.State.Active);
            }
        }

        void SetCurrentButton(int index)
        {
            CurrentButton.SetState(StageButton.State.Active);
            buttonIndex = index;
            CurrentButton.SetState(StageButton.State.Current);
        }

        void SwitchSubStageButton(bool isEnable, int index = -1)
        {
            if (index == -1)
            {
                foreach (var button in buttons)
                {
                    foreach (var subStage in button.GetSubStages)
                    {
                        subStage.gameObject.SetActive(isEnable);
                    }
                }
            }
            else
            {
                foreach (var subStage in buttons[index].GetSubStages)
                {
                    subStage.gameObject.SetActive(isEnable);
                }
                
                if (isEnable && buttons[index].GetSubStages.Count > 0)
                {
                    subStageIndex = index + 1;
                    stages[subStageIndex].StartStage();
                    buttons[subStageIndex].SetState(StageButton.State.Current);
                }
            }
        }

        bool IsSubStageButton(int index)
        {
            var isSubStage = false;
            
            for (var i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].GetSubStages.Count > 0)
                    isSubStage = i < index && index <= i + buttons[i].GetSubStages.Count;
                if (isSubStage)
                    return true;
            }

            return false;
        }
    }
}