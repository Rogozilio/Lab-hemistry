using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerObject : MonoBehaviour
{
    private Camera _camera;

    private MoveMouseItem _item;
    private Vector3 _hitWall;

    void Awake()
    {
        _camera = Camera.main;
    }
    
    void Update()
    {
        RaycastHit[] hits = RaycastAll();
        MoveObjectToPlane(hits);
        
    }

    RaycastHit[] RaycastAll()
    {
        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastAll(mouseRay);
    }

    private void MoveObjectToPlane(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out MoveMouseItem moveItem) && !_item||
                (moveItem && !_item.IsActive && moveItem.IsReadyToAction))
            {
                _item = moveItem;
                _item.ShowOutline();
            }
            else if (hit.collider.TryGetComponent(out ClickMouseItem clickItem) && !_item ||
                     (clickItem && !_item.IsActive && clickItem.IsReadyToAction))
            {
                clickItem.ShowOutline();
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                _hitWall = hit.point;
            }
        }

        if (_item && _hitWall != Vector3.zero)
        {
            _item.SetHitWall = _hitWall;
        }
    }
}