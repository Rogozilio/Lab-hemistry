using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestartEvent : MonoBehaviour, IRestart
{
    public UnityEvent OnRestartStage;

    public void Restart()
    {
        OnRestartStage?.Invoke();
    }
}
