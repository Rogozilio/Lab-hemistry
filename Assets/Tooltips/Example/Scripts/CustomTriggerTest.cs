using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips
{

public class CustomTriggerTest : TooltipCustomTrigger
{
    protected override bool IsVisible () 
    {
        return Input.GetKey(KeyCode.Space);
    }
}

}
