using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cup : MonoBehaviour
{
    public enum StateCup
    {
        Empty,
        WithMagnesium,
        WithMagnesiumAndWater,
        Mix
    }

    public GameObject Water;
    public GameObject Magnesium;
    public GameObject PieceMagnesium;

    private int _countWaterDrop;
    private Vector3 _prevPosition;
    private Vector3 _waterOriginScale;
    private Renderer _rendererWater;
    private StateCup _stateCup;

    private bool _isChangeScaleFinish;
    private bool _isChangeColorFinish;

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

    public StateCup GetStateCup => _stateCup;
    public int CountWaterDrop => _countWaterDrop;
    public bool IsHaveShavingsPiece => Magnesium.activeSelf;
    public bool IsHaveWater => Water.activeSelf;
    public bool IsReadyForPaperIndicator => _isChangeColorFinish && _isChangeScaleFinish;

    private void Awake()
    {
        _rendererWater = Water.GetComponent<Renderer>();
        _resultColor = _resultColors[Random.Range(0, _resultColors.Length)];
        _originColor = _rendererWater.material.color;
        _waterOriginScale = Water.transform.localScale;
    }

    public void AddWaterDrop()
    {
        _countWaterDrop++;

        if (!Water.activeSelf)
            Water.SetActive(true);

        Water.transform.localScale = _waterOriginScale * (_countWaterDrop / 10f);

        if (_countWaterDrop == 10) _stateCup = StateCup.WithMagnesiumAndWater;

        if (_countWaterDrop > 10)
        {
            var color = Water.GetComponent<Renderer>().material.GetColor("_BaseColor");
            Water.GetComponent<Renderer>().material
                .SetColor("_BaseColor", color + new Color(0.06f, -0.24f, -0.12f, 0f));
        }
        
        if(_countWaterDrop == 13) _stateCup = StateCup.Empty;
            
    }

    public void AddPieceMagnesium(Transform target)
    {
        _stateCup = StateCup.WithMagnesium;
        Magnesium.SetActive(true);
        Magnesium.transform.position = target.position;
        Magnesium.transform.rotation = target.rotation;
        Magnesium.transform.localScale = target.localScale;
        target.gameObject.SetActive(false);
    }

    public void StirAll(Transform glassStick)
    {
        if (_prevPosition == Vector3.zero)
            _prevPosition = glassStick.position;

        if (Vector3.Distance(_prevPosition, glassStick.position) > 0.01f)
        {
            _prevPosition = glassStick.position;

            var color = Magnesium.transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_LiquidColor");
            color -= new Color(0, 0, 0, 0.01f);
            if (color.a > 0)
                Magnesium.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_LiquidColor", color);
            else
            {
                PieceMagnesium.SetActive(false);
                _stateCup = StateCup.Mix;
            }
        }
    }
}