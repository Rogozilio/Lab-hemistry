using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cup : MonoBehaviour
{
    public GameObject Water;
    public GameObject Magnesium;

    private int _countWaterDrop;
    private Vector3 _prevPosition;
    private Renderer _rendererWater;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    private Color32[] _resultColors = new Color32[]
    {
        new Color32(106, 0, 47, 100),
        new Color32(116, 0, 28, 100),
        new Color32(115, 6, 18, 100),
        new Color32(146, 14, 20, 100),
        new Color32(156, 34, 12, 100),
        new Color32(171, 136, 22, 100),
        new Color32(162, 139, 15, 100),
        new Color32(147, 124, 6, 100),
        new Color32(10, 101, 0, 100),
        new Color32(0, 65, 12, 100),
        new Color32(1, 27, 7, 100),
        new Color32(4, 6, 8, 100),
        new Color32(0, 0, 40, 100)
    };

    private Color _originColor;
    private Color _resultColor;

    public int CountWaterDrop => _countWaterDrop;
    public bool IsHaveShavingsPiece => Magnesium.activeSelf;
    public bool IsHaveWater => Water.activeSelf;

    private void Awake()
    {
        _rendererWater = Water.GetComponent<Renderer>();
        _resultColor = _resultColors[Random.Range(0, _resultColors.Length)];
        _originColor = _rendererWater.material.color;
    }

    public void AddWaterDrop()
    {
        _countWaterDrop++;
        if (_countWaterDrop >= 10)
        {
            Water.SetActive(true);
        }
    }

    public void AddPieceMagnesium(Transform target)
    {
        Magnesium.SetActive(true);
        Magnesium.transform.position = target.position;
        Magnesium.transform.rotation = target.rotation;
        Magnesium.transform.localScale = target.localScale;
    }

    public void StirAll(Transform glassStick)
    {
        var steps = 60;

        if (_prevPosition == Vector3.zero)
            _prevPosition = glassStick.position;

        if (Vector3.Distance(_prevPosition, glassStick.position) > 0.01f)
        {
            _prevPosition = glassStick.position;

            //Scale
            var decrease = (1 - 0.5f) / steps;
            if (Magnesium.transform.localScale.x >= 0.5f)
                Magnesium.transform.localScale -= new Vector3(decrease, decrease, decrease);

            var a = (_resultColor - _originColor) / steps;
            //Color
            if (_rendererWater.material.color != _resultColor)
                _rendererWater.material.color += a;
        }
    }
}