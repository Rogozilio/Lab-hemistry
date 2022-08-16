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

    // Update is called once per frame
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
            if (hit.collider.TryGetComponent(out MoveMouseItem item) && !_item ||
                item && !_item.IsActive)
            {
                _item?.HideOutline();
                _item = item;
                _item.ShowOutline();
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