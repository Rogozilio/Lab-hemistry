using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public abstract class DataView : View
{

    //  Data  -------------------------------------------------------
    public abstract TooltipData.Type type { get; }
    public virtual TooltipData data { get; set; }
    
    public bool Match (TooltipData data) 
    {
        return 
            data != null &&
            this.data == data && 
            this.type == data.type;
    }



    //  Info  -------------------------------------------------------
    protected new bool isReady => base.isReady && data != null;

}

}
