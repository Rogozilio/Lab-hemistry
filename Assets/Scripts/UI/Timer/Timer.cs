using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Transform ClockHand;
    public Image ProgressBarYellow;
    public Image ProgressBarGreen;

    private float _step = 1f / 60f;
    private float _stepRotate = 360f / 60f;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void StartLaunchTimer(int time)
    {
        
        StartCoroutine(LaunchTimer(time));
    }

    private IEnumerator LaunchTimer(int time)
    {
        Show();
        ProgressBarYellow.fillAmount = time * _step;

        while (time-- > 0)
        {
            yield return new WaitForSeconds(1f);

            ProgressBarGreen.fillAmount += _step;
            ClockHand.Rotate(Vector3.forward, -_stepRotate);
        }
        Hide();
    }

    private void Show()
    {
        ClockHand.rotation = Quaternion.identity;
        ProgressBarGreen.fillAmount = 0f;
        StartCoroutine(ShowOrHideTimer(true));
    }

    private void Hide()
    {
        StartCoroutine(ShowOrHideTimer(false));
    }

    private IEnumerator ShowOrHideTimer(bool isShowTimer, float time = 0.5f)
    {
        var a = isShowTimer ? 140 : 0;
        var b = isShowTimer ? 0 : 140;;
        var t = 0f;
        
        while (t < time)
        {
            yield return null;
            t += Time.deltaTime;
            var x = Mathf.Lerp(a, b, t / time);
            _rectTransform.anchoredPosition = new Vector2(x, 47);
        }
    }
}