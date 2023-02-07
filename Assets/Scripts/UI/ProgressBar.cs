using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    
    private Coroutine _coroutineProgressBar;

    public float SetProgressBar
    {
        set
        {
            if(_coroutineProgressBar != null) StopCoroutine(_coroutineProgressBar);
           
            _coroutineProgressBar = StartCoroutine(AnimationProgressBar(value));
        }
    }

    private Image _progressBar;

    private void Awake()
    {
        _progressBar = GetComponent<Image>();
    }

    private IEnumerator AnimationProgressBar(float value)
    {
        var range = (value - _progressBar.fillAmount) * 100f;
        var delta = 1f;
        delta = (range > 0) ? delta : -delta;
        var steps = Math.Abs(range / delta);

        while (steps > 0)
        {
            _progressBar.fillAmount += delta / 100f;
            var progress = Mathf.Floor(_progressBar.fillAmount * 100);
            textMeshPro.text = progress + "%";

            if (progress == 100)
                textMeshPro.text = "Перезапустить";
            
            yield return new WaitForFixedUpdate();
            steps--;
        }
    }

    public void ShowRestartButton()
    {
        textMeshPro.text = "Перезапустить";
    }

    public void HideRestartButton()
    {
        textMeshPro.text = Mathf.Floor(_progressBar.fillAmount * 100) + "%";
    }
}
