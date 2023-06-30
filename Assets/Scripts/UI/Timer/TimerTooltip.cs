using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerTooltip : MonoBehaviour
{
    public ERA.Tooltip tooltip;
    public float time;

    private UnityEvent _actionsStart;
    private UnityEvent _actionsEnd;

    public UnityAction AddActionStart
    {
        set
        {
            _actionsStart ??= new UnityEvent();
            _actionsStart.AddListener(value);
        }
    }

    public UnityAction AddActionEnd
    {
        set
        {
            _actionsEnd ??= new UnityEvent();
            _actionsEnd.AddListener(value);
        }
    }

    public void StartTimerTooltip(Transform target, string title, string progressBar)
    {
        _actionsStart?.Invoke();
        tooltip.setTarget = target;
        tooltip.SetAlwaysVisible = true;
        tooltip.dataList[0].stringData = title;
        tooltip.dataList[1].stringData = progressBar;
     
        StartCoroutine(LaunchTimerTooltip());
        
    }

    private IEnumerator LaunchTimerTooltip()
    {
        tooltip.gameObject.SetActive(true);
        var timer = 0f;
        while (timer < time)
        {
            tooltip.dataList[1].numberData = timer / time;
            timer++;
            yield return new WaitForSeconds(1);
        }

        tooltip.dataList[1].numberData = 1f;
        tooltip.SetAlwaysVisible = false;
        tooltip.Hide();
        _actionsEnd?.Invoke();
        tooltip.gameObject.SetActive(false);
    }
}
