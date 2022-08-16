using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 



namespace VirtualLab 
{

public class Hint : HandSystem.AbstractHandOverItem 
{
    [SerializeField] float tranparencyNormal = 0.4f; 
    [SerializeField] float tranparencyHighlighted = 0.9f; 



    CanvasGroup canvasGroup; 



    void Awake () 
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
        gameObject.SetActive(false); 
    }

    void Start () 
    {
        HighlightOff(); 
    }



    //  Events  ----------------------------------------------------- 
    public UnityEvent<Hint> onClick; 



    //  Hand over  -------------------------------------------------- 
    public override void OnHandOverStart () 
    {
        HighlightOn(); 
    }

    public override void OnHandOverEnd ()
    {
        HighlightOff(); 
    }



    //  Mouse clicks  ----------------------------------------------- 
    public bool recieveClicks { get; set; } = true; 

    public void OnClick () 
    {
        if (recieveClicks) onClick.Invoke(this); 
    }


    
    //  Visibility  ------------------------------------------------- 
    public void Show () 
    {
        HighlightOff(); 
        gameObject.SetActive(true); 
    }

    public void Hide () 
    {
        gameObject.SetActive(false); 
    }



    //  Highlight  -------------------------------------------------- 
    void HighlightOn () 
    {
        canvasGroup.alpha = tranparencyHighlighted; 
    }

    void HighlightOff () 
    {
        canvasGroup.alpha = tranparencyNormal; 
    }

}

}
