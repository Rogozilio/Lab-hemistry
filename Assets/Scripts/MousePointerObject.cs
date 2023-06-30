using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RaycastUtilities
{
    public static bool PointerIsOverUI(Vector2 screenPos)
    {
        var hitObject = UIRaycast(ScreenPosToPointerData(screenPos));
        return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI");
    }

    static GameObject UIRaycast(PointerEventData pointerData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count < 1 ? null : results[0].gameObject;
    }

    static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
        => new(EventSystem.current) { position = screenPos };
}

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
        var hits = RaycastAll();
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
        var priorityOutline = 0;

        foreach (var hit in hits)
        {
            if (RaycastUtilities.PointerIsOverUI(Input.mousePosition))
            {
                break;
            }
            if (hit.collider.TryGetComponent(out MouseItem mouseItem) && !_item ||
                (mouseItem && !_item.IsActive))
            {
                if (mouseItem.priorityOutline >= priorityOutline)
                {
                    if (!Equals(_item, mouseItem))
                    {
                        _item?.HideOutline(false);
                    }
                    
                    priorityOutline = mouseItem.priorityOutline;
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