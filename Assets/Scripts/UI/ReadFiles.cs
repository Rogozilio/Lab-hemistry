using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using VirtualLab.ApplicationData;
using ERA;
using VirtualLab.Tooltips;

public class ReadFiles : MonoBehaviour
{
    public bool isReadFileStagesTitles = true;
    public bool isReadFileSteps = true;
    public bool isReadFileTooltips = true;

    private List<string> _allLines;
    private UIStagesControl _uiStagesControl;
    private ERA.Tooltip _tooltip;

    private void Awake()
    {
        _allLines = new List<string>();
        _uiStagesControl = FindObjectOfType<UIStagesControl>();
        _tooltip = FindObjectOfType<ERA.Tooltip>();
        
        if(isReadFileStagesTitles) StringLoaderForStages();
        if(isReadFileSteps) StringLoaderForSteps();
        if(isReadFileTooltips) StringLoaderForTooltips();
    }

    public void StringLoaderForStages()
    {
        new StringLoader(CreateAllListForStages).Start("StagesTitles.txt");
    }
    public void StringLoaderForSteps()
    {
        new StringLoader(CreateAllListForSteps).Start("Steps.txt");
    }
    public void StringLoaderForTooltips()
    {
        new StringLoader(CreateAllListForTooltips).Start("Tooltips.txt");
    }

    private void CreateAllListForStages(string data)
    {
        _allLines = data.Split('\n').ToList();
        WriteStagesListFromPath();
    }
    private void CreateAllListForSteps(string data)
    {
        _allLines = data.Split('\n').ToList();
        WriteStepsListFromPath();
    }

    private void CreateAllListForTooltips(string data)
    {
        _allLines = data.Split('\n').ToList();
        WriteTooltipsFromPath();
    }
    
    private void WriteStagesListFromPath()
    {
        _uiStagesControl.SetNameLab = _allLines[0];
        _uiStagesControl.ClearTextStages();
        for (var i = 0; i < _allLines.Count;)
        {
            if (_allLines[i].Contains('#'))
            {
                _uiStagesControl.AddTextStage(_allLines[i+1], _allLines[i+2]);
                i = math.clamp(i + 3, i, _allLines.Count);
            }
            else
            {
                i++;
            }
        }
    }
    
    private void WriteStepsListFromPath()
    {
        var indexStage = 0;
        _uiStagesControl.ClearSteps();
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
            
            if (_allLines[i].Contains('#'))
            {
                indexStage++;
            }
            else if (_allLines[i].Contains(':'))
            {
                var text = _allLines[i].Split(':');
                _uiStagesControl.AddStep(indexStage, text[0], text[1]);
            }
        }
    }
    
    private void WriteTooltipsFromPath()
    {
        _tooltip ??= FindObjectOfType<ERA.Tooltip>();
        _tooltip.DataFromFile ??= new Dictionary<int, string>();
        _tooltip.DataFromFile.Clear();
        foreach (var line in _allLines)
        {
            var key = int.Parse(line.Split('[')[1].Split(']')[0]);
            var value = line.Split('<')[1].Split('>')[0];
            
            var isBeginSub = true;
            while (value.Contains('_'))
            {
                var index = value.IndexOf('_');
                value = value.Remove(index, 1)
                    .Insert(index, isBeginSub ? "<size=70%>" : "<size=100%>");
                isBeginSub = !isBeginSub;
            }
            _tooltip.DataFromFile.Add(key, value);
        }
    }
}
