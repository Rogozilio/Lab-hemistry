using System;
using UnityEngine;
using UnityEngine.Events;

public class CupForGranule : MonoBehaviour
{
    public Transform granule;

    private MoveMap _moveMap;
    private ClickMouseItem _clickMouseItem;

    private void Awake()
    {
        if (!granule)
        {
            Debug.LogError("Granule not found");
            return;
        }

        if (!granule.TryGetComponent(out _moveMap))
        {
            Debug.LogError("Component MoveMap in granule not found");
            return;
        }

        if (!TryGetComponent(out _clickMouseItem))
        {
            Debug.LogError("Component ClickMouseItem not found");
            return;
        }

        _clickMouseItem.enabled = false;
    }

    public void ActiveCupGranule(Transform target)
    {
        _moveMap.SetTargetMove(0, target);
        _moveMap.SetTargetMove(1, target);
        _clickMouseItem.enabled = true;
    }

    public void AddGranuleInTestTube()
    {
        _moveMap.StartToMove(0);
        _clickMouseItem.enabled = false;
    }
}