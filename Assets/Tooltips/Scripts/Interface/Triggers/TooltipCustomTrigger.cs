using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA
{

/// <summary> 
///     Абстракстный контроллер выноски. <br/>
///     Позволяет создать собственную логику для выноски, для этого нужно переопределить метод IsVisible().
/// </summary> 
public abstract class TooltipCustomTrigger : MonoBehaviour
{
    public Tooltip tooltip;

    void Update () 
    {
        if (IsVisible()) tooltip.Show();
        else             tooltip.Hide();
    }

    void OnDisable () 
    {
        tooltip?.Hide();
    }

    /// <summary>
    ///     Этот медот вызывается каждый кадр, возвращаемое значение bool определяет должна ли выноска отображаться сейчас. 
    /// </summary>
    protected abstract bool IsVisible ();
}

}
