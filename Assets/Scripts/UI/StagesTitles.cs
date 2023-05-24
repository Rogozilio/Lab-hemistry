using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VirtualLab.ApplicationData;

public class StagesTitles : MonoBehaviour
{
    private struct StageTitle
    {
        public int index;
        public string upName;
        public string downName;
    }

    public TextMeshProUGUI TextMeshProUGUI;

    private int index ;
    private List<string> _allLines;
    private List<StageTitle> _stagesTitles;
    void Awake()
    {
        index = 0;
        _allLines = new List<string>();
        _stagesTitles = new List<StageTitle>();
        
        new StringLoader(CreateAllList).Start("StagesTitles.txt");
    }

    private void CreateAllList(string data)
    {
        _allLines = data.Split('\n').ToList();
        WriteStepStageListFromPath();
    }
    
    private void WriteStepStageListFromPath()
    {
        for (var i = 0; i < _allLines.Count; i++)
        {
            if (_allLines[i].Contains('$'))
            {
                _stagesTitles.Add(new StageTitle()
                {
                    index = index++,
                    upName = _allLines[i+1],
                    downName = _allLines[i+2]
                });
            }
        }
    }

    // public void ShowStageTitle(int index)
    // {
    //     var tagCSpace = "<cspace=-0.02em>";
    //     var tagLineHeight = "<line-height=65%>";
    //
    //     foreach (var stage in _stagesTitles)
    //     {
    //         if (stage.index != index) continue;
    //         TextMeshProUGUI.text = tagCSpace + tagLineHeight + "<b>" + stage.upName + "</b>\n" + stage.downName;
    //         return;
    //     }
    // }
}
