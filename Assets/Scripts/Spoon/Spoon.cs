using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : MonoBehaviour
{
    private bool _isEmpty = true;
    
    private Vector3 _originLocalPositionPieceMarble;
    private Quaternion _originLocalRotatePieceMarble;

    public GameObject fewPieceMarble;
    
    public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }

    private void Awake()
    {
        _originLocalPositionPieceMarble = fewPieceMarble.transform.localPosition;
        _originLocalRotatePieceMarble = fewPieceMarble.transform.localRotation;
    }
    
    public void ReturnToOriginFewPieceMarble()
    {
        fewPieceMarble.transform.localPosition = _originLocalPositionPieceMarble;
        fewPieceMarble.transform.localRotation = _originLocalRotatePieceMarble;
    }
}
