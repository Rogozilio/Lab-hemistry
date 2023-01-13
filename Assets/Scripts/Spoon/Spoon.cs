using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : MonoBehaviour, IRestart
{
    private bool _isEmpty = true;
    
    private Vector3 _originLocalPositionPieceMarble;
    private Quaternion _originLocalRotatePieceMarble;

    public GameObject fewPieceMarble;

    private StepStageSystem _stepStageSystem;
    
    public bool IsEmpty
    {
        get => _isEmpty;
        set
        {
            if(!value) _stepStageSystem.NextStep();
            _isEmpty = value;
        }
    }

    private void Awake()
    {
        _stepStageSystem = FindObjectOfType<StepStageSystem>();
        _originLocalPositionPieceMarble = fewPieceMarble.transform.localPosition;
        _originLocalRotatePieceMarble = fewPieceMarble.transform.localRotation;
    }
    
    public void ReturnToOriginFewPieceMarble()
    {
        fewPieceMarble.transform.localPosition = _originLocalPositionPieceMarble;
        fewPieceMarble.transform.localRotation = _originLocalRotatePieceMarble;
        _stepStageSystem.NextStep();
    }

    public void Restart()
    {
        _stepStageSystem.RestartStage();
        _isEmpty = true;
        fewPieceMarble.SetActive(false);
        fewPieceMarble.transform.localPosition = _originLocalPositionPieceMarble;
        fewPieceMarble.transform.localRotation = _originLocalRotatePieceMarble;
    }
}
