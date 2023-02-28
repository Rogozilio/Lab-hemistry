using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.LowLevel;
using VirtualLab.ApplicationData;

public class StepStageSystem : MonoBehaviour
{
    public struct StepStage
    {
        public string StageName;
        public int IndexStep;
        public List<string> Steps;
    }

    public ProgressBar progressBar;
    public RectTransform displaySteps;

    private List<string> _allLines;

    private List<StepStage> _stepStageList;
    private TextMeshProUGUI _textMeshPro;
    private int _currentIndexStepStage;

    private void Awake()
    {
        _allLines = new List<string>();
        _stepStageList = new List<StepStage>();
        _textMeshPro = GetComponent<TextMeshProUGUI>();

        new StringLoader(CreateAllList).Start("Steps.txt");
    }

    private void Update()
    {
        var width = Math.Clamp(_textMeshPro.preferredWidth, 300f, 1000f) + 20f;
        displaySteps.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void CreateAllList(string data)
    {
        _allLines = data.Split('\n').ToList();
        WriteStepStageListFromPath();
    }

    private void WriteStepStageListFromPath()
    {
        var newStepStage = new StepStage();


        for (var i = 0; i < _allLines.Count; i++)
        {
            var isBeginSub = true;
            while (_allLines[i].Contains('_'))
            {
                var index = _allLines[i].IndexOf('_');
                _allLines[i] = _allLines[i].Remove(index, 1)
                    .Insert(index, isBeginSub ? "<size=70%>" : "<size=100%>");
                isBeginSub = !isBeginSub;
            }

            
            if (_allLines[i].Contains('$'))
            {
                newStepStage.StageName = _allLines[i].Split('[')[1].Split(']')[0];
            }
            // else if (_allLines[i].Contains('_'))
            // {
            //     
            //     newStepStage.Steps ??= new List<string>();
            //     newStepStage.Steps.Add(_allLines[i]);
            // }
            else if (_allLines[i].Contains('#'))
            {
                _stepStageList.Add(newStepStage);
                newStepStage = new StepStage();
            }
            else
            {
                newStepStage.Steps ??= new List<string>();
                newStepStage.Steps.Add(_allLines[i]);
            }
        }
    }

    private void DisplayProgressBar()
    {
        var stepStage = _stepStageList[_currentIndexStepStage];
        _textMeshPro.text = stepStage.Steps[stepStage.IndexStep];
        progressBar.SetProgressBar = (float)stepStage.IndexStep /
                                     (stepStage.Steps.Count - 1 == 0 ? 1 : stepStage.Steps.Count - 1);
    }

    public void SwitchStage(string stageName)
    {
        for (var i = 0; i < _stepStageList.Count; i++)
        {
            if (_stepStageList[i].StageName == stageName)
            {
                _currentIndexStepStage = i;
                DisplayProgressBar();
                return;
            }
        }
    }

    public void NextStep()
    {
        var stepStage = _stepStageList[_currentIndexStepStage];
        stepStage.IndexStep++;
        _stepStageList[_currentIndexStepStage] = stepStage;
        DisplayProgressBar();
    }

    public void RestartStage()
    {
        var stepStage = _stepStageList[_currentIndexStepStage];
        stepStage.IndexStep = 0;
        _stepStageList[_currentIndexStepStage] = stepStage;
        DisplayProgressBar();
    }
}