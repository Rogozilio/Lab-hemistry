using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPaper : MonoBehaviour
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

    private StatePaperIndicator _state;

    public StatePaperIndicator GetState => _state;
    void Awake()
    {
        _moveToPoint = new MoveToPoint(transform.parent, default, default, new Vector3(1, 1, 0.3f));
        _moveToPoint.SetSpeedTRS = new Vector3(0f, 0f, 1f);
    }

    public void AddWater()
    {
        _state = StatePaperIndicator.Water;
        PaperWater.SetActive(true);
    }

    public void StartVisibleResult()
    {
        StartCoroutine(VisibleResult());
    }

    private IEnumerator VisibleResult()
    {
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
    }
}
