using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    private Button _buttonRestart;

    public float SetProgressBar
    {
        set
        {
            if (value != 1f)
                StartCoroutine(AnimationProgressBar(value));
            else
            {
                _buttonRestart.enabled = true;
                StartCoroutine(AnimationProgressBar(value));
            }

        }
    }

    private Image _progressBar;

    private void Awake()
    {
        _progressBar = GetComponent<Image>();
        _buttonRestart = transform.parent.GetComponent<Button>();
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
            var progress = Math.Floor(_progressBar.fillAmount * 100);
            textMeshPro.text = progress + "%";

            if (progress == 100)
                textMeshPro.text = "Перезапустить";
            
            yield return new WaitForFixedUpdate();
            steps--;
        }
    }
    
}
