using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace VirtualLab {

public class Stage1 : AbstractStage 
{
    [SerializeField] Samples samples; 



    //  Stage events  ----------------------------------------------- 
    public override void StartStage () 
    {
        samples.gameObject.SetActive(true); 
    }

    public override void StopStage () 
    {
        samples.gameObject.SetActive(false); 
    }

    public override void Restart () 
    {
        
    }

}

}
