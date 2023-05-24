using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : MonoBehaviour, IRestart
{
    private bool _isEmpty = true;
    
    private Vector3 _originLocalPositionPieceMarble;
    private Quaternion _originLocalRotatePieceMarble;

    public GameObject fewPieceMarble;

    private UIStagesControl _uiStagesControl;
    
    public bool IsEmpty
    {
        get => _isEmpty;
        set
        {
            if(!value) _uiStagesControl.NextStep();
            _isEmpty = value;
        }
    }

    private void Awake()
    {
        _uiStagesControl = FindObjectOfType<UIStagesControl>();
        _originLocalPositionPieceMarble = fewPieceMarble.transform.localPosition;
        _originLocalRotatePieceMarble = fewPieceMarble.transform.localRotation;
    }
    
    public void ReturnToOriginFewPieceMarble()
    {
        fewPieceMarble.transform.localPosition = _originLocalPositionPieceMarble;
        fewPieceMarble.transform.localRotation = _originLocalRotatePieceMarble;
        _uiStagesControl.NextStep();
    }

    public void Restart()
    {
        _isEmpty = true;
        fewPieceMarble.SetActive(false);
        fewPieceMarble.transform.localPosition = _originLocalPositionPieceMarble;
        fewPieceMarble.transform.localRotation = _originLocalRotatePieceMarble;
    }
}
