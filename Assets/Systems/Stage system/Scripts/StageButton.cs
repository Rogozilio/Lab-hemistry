using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





namespace VirtualLab {

public class StageButton : MonoBehaviour
{
    // parameters 
    [SerializeField] GameObject currentImage; 
    [SerializeField] GameObject activeImage; 
    [SerializeField] GameObject inactiveImage; 
    // connections 
    Button button; 
    StageSystem stageSystem; 
    // data 
    int index; 
    public enum State { Current, Active, Inactive } 
    State state = State.Inactive; 



    void Awake () 
    {
        button = GetComponent<Button>(); 
        stageSystem = GetComponentInParent<StageSystem>(); 
    }

    public void SetIndex (int index) 
    {
        this.index = index; 
    }

    void Start () 
    {
        button.onClick.AddListener(OnClick); 
    }





    //  Events  ----------------------------------------------------- 
    void OnClick () 
    {
        if (state == State.Active) 
        {
            stageSystem.SetStage(index); 
        }
    }





    //  State  ------------------------------------------------------ 
    public void SetState (State newState) 
    {
        if (state == newState) return; 

        state = newState; 

        currentImage.SetActive(state == State.Current); 
        activeImage.SetActive(state == State.Active); 
        inactiveImage.SetActive(state == State.Inactive); 
    }

}

}
