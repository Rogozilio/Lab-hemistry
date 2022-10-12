using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPaper : MonoBehaviour
{
    private Renderer _renderer;
    private Renderer _rendererChild;

    private MoveToPoint _moveToPoint;
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rendererChild = transform.GetChild(0).GetComponent<Renderer>();

        _moveToPoint = new MoveToPoint(transform.parent, default, default, new Vector3(1, 1, 0.3f));
        _moveToPoint.SetSpeedTRS = new Vector3(0f, 0f, 0.5f);
    }

    public void SetColor(Renderer renderer)
    {
        _renderer.material.color = renderer.material.color;
        _rendererChild.material.color = renderer.material.color;
        
        StartCoroutine(_moveToPoint.StartAsync());
    }
}
