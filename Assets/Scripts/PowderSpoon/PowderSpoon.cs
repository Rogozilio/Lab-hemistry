using System.Collections;
using System.Collections.Generic;
using Liquid;
using UnityEngine;

public class PowderSpoon : MonoBehaviour
{
    public TypePowder typePowder;
    public GameObject Powder;
    
    private Vector3 _originPosition;
    private GameObject _idlePowder;
    private StateItem _stateItem;

    private MoveMap _moveMapPowder;
    
    public Transform SetTargetMoveMapPowder
    {
        set => _moveMapPowder.datas[0].move.target = value;
    }

    private void Awake()
    {
        Powder.GetComponent<PowderDrop>().typePowder = typePowder;
        
        _idlePowder = transform.GetChild(0).gameObject;
        
        _stateItem = GetComponent<StateItem>();
        _moveMapPowder = Powder.GetComponent<MoveMap>();

        _originPosition = _idlePowder.transform.position;
    }
    
    private void Update()
    {
        if(_stateItem.State != StateItems.LinearRotate || !_idlePowder.activeSelf) return;

        if (Vector3.Angle(_idlePowder.transform.forward, Vector3.up) > 90f)
        {
            Powder.SetActive(true);
            Powder.transform.position = _idlePowder.transform.position;
            _idlePowder.SetActive(false);
            _moveMapPowder.StartToMove(0);
        }
    }
}
