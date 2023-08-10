using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



namespace ERA.SidePanelAsset 
{

public class InfoPanelAnimation : MonoBehaviour
{
    [SerializeField] GameObject tabs;
    [SerializeField] GameObject expandButton;
    [SerializeField] float animationTime = 0.75f; 
    public Events events;

    float progress;
    float target;



    void Start () 
    {
        InitUI();
    }

    void Update () 
    {
        UpdateInteractions();
        progress = UpdateProgress(progress, target, animationTime);
        UpdateUI(progress);
    }



    //  Info  -------------------------------------------------------
    public bool isVisible => progress > 0;

    public bool isAppearingOrVisible      => target == 1;
    public bool isDisappearingOrInvisible => target == 0;




    //  Events  ----------------------------------------------------- 
    [System.Serializable]
    public class Events 
    {
        public UnityEvent onVisible; 
        public UnityEvent onAppearing; 
        public UnityEvent onDisappearing; 
        public UnityEvent onInvisible; 
    }



    //  Interactions  -----------------------------------------------
    void UpdateInteractions () 
    {
        OpenOrHideOnTab();
    }

	void OpenOrHideOnTab () 
	{
		if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            if (isDisappearingOrInvisible) Show();
            else                           Hide();
        }
	}



    //  Actions  ---------------------------------------------------- 
    public void Show () 
    {
        target = 1;
    }

    public void Hide () 
    {
        target = 0;
    }



    //  Animation  -------------------------------------------------- 
    float UpdateProgress (float progress, float target, float animationTime) 
    {
        if (progress != target) 
        {
            float speed     = GetAnimationSpeed(animationTime);
            float maxChange = speed * Time.deltaTime;
            progress = Mathf.MoveTowards(progress, target, maxChange); 
        }

        return progress;
    }

    float GetAnimationSpeed (float animationTime) 
    {
        return animationTime > 0 ? 1 / animationTime : float.MaxValue;
    }



    //  UI  --------------------------------------------------------- 
    LayoutElement layoutElement; 
    float originalWidth; 

    void InitUI () 
    {
        layoutElement = GetComponent<LayoutElement>(); 
        originalWidth = layoutElement.preferredWidth; 
    }

    void UpdateUI (float progress) 
    {
        float visibility = Mathf.SmoothStep(0, 1, progress);

        bool isVisible = visibility > 0;
        tabs        .SetActive(isVisible); 
        expandButton.SetActive(!isVisible); 

        layoutElement.preferredWidth = visibility * originalWidth; 
    }

}

}
