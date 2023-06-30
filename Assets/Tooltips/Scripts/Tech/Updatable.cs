using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

[ExecuteAlways]
public class Updatable : MonoBehaviour
{

    protected void Awake () 
    {
        if (Application.isPlaying) AwakePlayMode();
        else                       AwakeEditMode();
    }

    protected void Start () 
    {
        if (Application.isPlaying) StartPlayMode();
        else                       StartEditMode();
    }

    protected void Update () 
    {
        if (Application.isPlaying) UpdatePlayMode();
        else                       UpdateEditMode();
    }

    protected void LateUpdate () 
    {
        if (Application.isPlaying) LateUpdatePlayMode();
        else                       LateUpdateEditMode();
    }


    protected virtual void AwakeEditMode () {}
    protected virtual void AwakePlayMode () {}

    protected virtual void StartEditMode () {}
    protected virtual void StartPlayMode () {}

    protected virtual void UpdateEditMode () {}
    protected virtual void UpdatePlayMode () {}

    protected virtual void LateUpdateEditMode () {}
    protected virtual void LateUpdatePlayMode () {}

}

}