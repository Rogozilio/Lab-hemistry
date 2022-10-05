using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPaper : MonoBehaviour
{
    private Renderer _renderer;
    private Renderer _rendererChild;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _rendererChild = transform.GetChild(0).GetComponent<Renderer>();
    }

    public void SetColor(Renderer renderer)
    {
        _renderer.material.color = renderer.material.color;
        _rendererChild.material.color = renderer.material.color;
    }
}
