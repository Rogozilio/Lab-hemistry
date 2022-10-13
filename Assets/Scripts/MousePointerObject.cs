using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePointerObject : MonoBehaviour
{
    private Camera _camera;

    private MouseItem _item;
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
        var isHitMouseItem = false;
        var minDistance = 100f;

        foreach (var hit in hits)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                break;
            }

            if (hit.collider.TryGetComponent(out MouseItem mouseItem) && !_item ||
                (mouseItem && !_item.IsActive))
            {
                if (!mouseItem.isActiveAndEnabled) continue;

                if (Vector3.Distance(_camera.transform.position, mouseItem.transform.position) < minDistance)
                {
                    if (!Equals(_item, mouseItem))
                        _item?.HideOutline();
                    
                    minDistance = Vector3.Distance(_camera.transform.position, mouseItem.transform.position);
                    _item = mouseItem;
                }

                isHitMouseItem = true;
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                _hitWall = hit.point;
            }
        }

        if (isHitMouseItem)
            _item?.ShowOutline();
        else
            _item?.HideOutline();

        if (_item?.GetType() == typeof(MoveMouseItem) && _hitWall != Vector3.zero)
        {
            var itemMove = _item as MoveMouseItem;
            itemMove.SetHitWall = _hitWall;
        }
    }
}