using System.Collections;
using System.Collections.Generic;
using Cursor;
using UnityEngine;

public class IndicatorPaper : MonoBehaviour, IRestart
{
    public enum StatePaperIndicator
    {
        Empty,
        Water,
        Result
    }
    
    public GameObject PaperWater;
    public GameObject PaperResult;

    private MoveToPoint _moveToPoint;
    private StepStageSystem _stepStageSystem;
    
    private Color _originColorWater;
    private Color _originColorResult;

    private StatePaperIndicator _state;

    public StatePaperIndicator GetState => _state;
    void Awake()
    {
        _originColorWater = PaperWater.GetComponent<Renderer>().material.color;
        _originColorResult =  PaperResult.GetComponent<Renderer>().material.color;

        _stepStageSystem = FindObjectOfType<StepStageSystem>();
        _moveToPoint = new MoveToPoint(transform.parent, default, default, new Vector3(1, 1, 0.3f));
        _moveToPoint.SetSpeedTRS = new Vector3(0f, 0f, 1f);
    }

    public void AddWater()
    {
        _state = StatePaperIndicator.Water;
        PaperWater.SetActive(true);
        _stepStageSystem.NextStep();
    }

    public void StartVisibleResult()
    {
        StartCoroutine(VisibleResult());
    }

    private IEnumerator VisibleResult()
    {
        CursorSkin.Instance.isUseClock = true;
        _stepStageSystem.NextStep();
        PaperResult.SetActive(true);
        var renderFrontWater = PaperWater.GetComponent<Renderer>();
        var renderBackWater = PaperWater.transform.GetChild(0).GetComponent<Renderer>();
        var renderFrontResult = PaperResult.GetComponent<Renderer>();
        var renderBackResult = PaperResult.transform.GetChild(0).GetComponent<Renderer>();
        var newColor = new Color(0, 0, 0, Time.fixedDeltaTime / 5f);
        
        while (renderFrontResult.material.color.a < 0.8f)
        {
            renderFrontWater.material.color -= newColor;
            renderBackWater.material.color -= newColor;
            
            renderFrontResult.material.color += newColor;
            renderBackResult.material.color += newColor;
            yield return new WaitForFixedUpdate();
        }

        _state = StatePaperIndicator.Result;
        GetComponent<MoveMouseItem>()?.BackToRespawnOrBackToMouse();
        _stepStageSystem.NextStep();
        CursorSkin.Instance.isUseClock = false;
    }

    public void Restart()
    {
        _state = StatePaperIndicator.Empty;
        PaperWater.SetActive(false);
        PaperResult.SetActive(false);
        PaperWater.GetComponent<Renderer>().material.color = _originColorWater;
        PaperWater.transform.GetChild(0).GetComponent<Renderer>().material.color = _originColorWater;
        PaperResult.GetComponent<Renderer>().material.color = _originColorResult;
        PaperResult.transform.GetChild(0).GetComponent<Renderer>().material.color = _originColorResult;
    }
}
