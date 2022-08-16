using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class TestStage : AbstractStage 
{



    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            OnCompleted(); 
        }
    }





    public override void StartStage () 
    {
        gameObject.SetActive(true); 
        Debug.Log(name + " started"); 
    }

    public override void StopStage () 
    {
        gameObject.SetActive(false); 
        Debug.Log(name + " stoped"); 
    }

}
