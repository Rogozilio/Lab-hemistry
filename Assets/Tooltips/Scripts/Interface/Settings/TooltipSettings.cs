using System.Collections;
using System.Collections.Generic;
using ERA;
using UnityEngine;



namespace VirtualLab.Tooltips
{

[System.Serializable]
public class TooltipSettings 
{
    [Tooltip("Система выносок. Используется для доступа к камере и расчета положения выноски на экране")]
    public TooltipSystem tooltipSystem;
    [Tooltip("Имя выноски. Используется для соотношения данных в json файле с объектами выносок")]
    public string name = "";

    public bool isReady => tooltipSystem;
}

}
