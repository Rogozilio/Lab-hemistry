using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cup : MonoBehaviour, IRestart
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
    private Renderer _rendererMagnesiumChild;
    private StateCup _stateCup;

    private Color _originColorWater;
    private Color _originColorMagnesium;

    private StepStageSystem _stepStageSystem;

    public StateCup GetStateCup => _stateCup;
    public int CountWaterDrop => _countWaterDrop;
    public bool IsHaveShavingsPiece => Magnesium.activeSelf;
    public bool IsHaveWater => Water.activeSelf;

    private void Awake()
    {
        _rendererWater = Water.GetComponent<Renderer>();
        _rendererMagnesiumChild = Magnesium.transform.GetChild(0).GetComponent<Renderer>();
        _stepStageSystem = FindObjectOfType<StepStageSystem>();
        _originColorWater = _rendererWater.material.color;
        _originColorMagnesium = _rendererMagnesiumChild.material.GetColor("_LiquidColor");
        _waterOriginScale = Water.transform.localScale;
    }

    public void AddWaterDrop()
    {
        _countWaterDrop++;

        if (!Water.activeSelf)
            Water.SetActive(true);

        Water.transform.localScale = _waterOriginScale * (_countWaterDrop / 10f);

        if (_countWaterDrop == 10)
        {
            _stateCup = StateCup.WithMagnesiumAndWater;
            _stepStageSystem.NextStep();
        }
            

        if (_countWaterDrop > 10)
        {
            var color = _rendererWater.material.GetColor("_BaseColor");
            _rendererWater.material
                .SetColor("_BaseColor", color + new Color(0.06f, -0.24f, -0.12f, 0f));
        }

        if (_countWaterDrop == 13)
        {
            _stateCup = StateCup.Empty;
            _stepStageSystem.NextStep();
        }
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

            var color =_rendererMagnesiumChild.material.GetColor("_LiquidColor");
            color -= new Color(0, 0, 0, 0.01f);
            if (color.a > 0)
                _rendererMagnesiumChild.material.SetColor("_LiquidColor", color);
            else
            {
                PieceMagnesium.SetActive(false);
                _stateCup = StateCup.Mix;
                _stepStageSystem.NextStep();
            }
        }
    }

    public void Restart()
    {
        Water.transform.localScale = _waterOriginScale;
        Water.SetActive(false);
        Magnesium.SetActive(false);
        PieceMagnesium.SetActive(false);
        _countWaterDrop = 0;
        _stateCup = StateCup.Empty;
        _rendererWater.material.SetColor("_BaseColor", _originColorWater);
        _rendererMagnesiumChild.material.SetColor("_LiquidColor", _originColorMagnesium);
    }
}