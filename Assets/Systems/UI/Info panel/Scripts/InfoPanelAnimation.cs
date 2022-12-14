using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



namespace VirtualLab 
{

public class InfoPanelAnimation : MonoBehaviour
{
    const float PROXIMITY_MARGIN = 0.0001f; 

    [SerializeField] GameObject infoPanel; 
    [SerializeField] float animationTime = 1; 



    void Start () 
    {
        InitUI(); 
        SetState(State.Visible); 
    }

    void Update () 
    {
        DoInput(); 
    }

	void LateUpdate () 
	{
		stateLastFrame = state; 
	}



    //  Events  ----------------------------------------------------- 
    public UnityEvent onVisible; 
    public UnityEvent onAppearing; 
    public UnityEvent onDisappearing; 
    public UnityEvent onInvisible; 



	//  Input  ------------------------------------------------------ 
	void DoInput () 
	{
		OpenOnTab(); 
		OpenOnShiftTab(); 
		OpenOnF1(); 
		CloseOnEsc(); 
	}

	void OpenOnTab () 
	{
		if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            switch (state) 
            {
                case State.Invisible: 
                case State.Disappearing: 
                    SetState(State.Appearing); 
                    break; 
            }
        }
	}

	void OpenOnShiftTab () 
	{
		if (
			Input.GetKeyDown(KeyCode.Tab) 
			&& 
			!( Input.GetKey(KeyCode.LeftShift) || 
			   Input.GetKey(KeyCode.RightShift) ) 
		) {
            switch (state) 
            {
                case State.Invisible: 
                case State.Disappearing: 
                    SetState(State.Appearing); 
                    break; 
            }
        }
	}

	void OpenOnF1 () 
	{
		if (Input.GetKeyDown(KeyCode.F1)) 
        {
            switch (state) 
            {
                case State.Invisible: 
                case State.Disappearing: 
                    SetState(State.Appearing); 
                    break; 
            }
        }
	}

	void CloseOnEsc () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            switch (state) 
            {
                case State.Visible: 
                case State.Appearing: 
                    SetState(State.Disappearing); 
                    break; 
            }
        }
	}



    //  State  ------------------------------------------------------ 
    public enum State { Invisible, Appearing, Visible, Disappearing } 
	public State stateLastFrame { get; private set; } 
    public State state { get; private set; } = State.Visible; 

    void SetState (State state) 
    {
        this.state = state; 

        switch (state) 
        {
            case State.Invisible:    ToInvisible();    break; 
            case State.Appearing:    ToAppearing();    break; 
            case State.Visible:      ToVisible();      break; 
            case State.Disappearing: ToDisappearing(); break; 
        }
    }

    void ToInvisible () 
    {
        SetVisibility(0); 
        onInvisible.Invoke(); 
    }

    void ToAppearing () 
    {
        StopAllCoroutines(); 
        StartCoroutine(Animation(1, () => SetState(State.Visible))); 
        onAppearing.Invoke(); 
    }

    void ToVisible () 
    {
        SetVisibility(1); 
        onVisible.Invoke(); 
    }

    void ToDisappearing () 
    {
        StopAllCoroutines(); 
        StartCoroutine(Animation(0, () => SetState(State.Invisible))); 
        onDisappearing.Invoke(); 
    }



    //  Actions  ---------------------------------------------------- 
    public void Show () 
    {
        if (state == State.Visible || state == State.Appearing) return; 
        SetState(State.Appearing); 
    }

    public void Hide () 
    {
        if (state == State.Invisible || state == State.Disappearing) return; 
        SetState(State.Disappearing); 
    }



    //  Animation  -------------------------------------------------- 
    delegate void Callback (); 

    float t = 1; 

    IEnumerator Animation (float targetValue, Callback onCompleted) 
    {
        while (Mathf.Abs(targetValue - t) > PROXIMITY_MARGIN) 
        {
            float speed = 1 / animationTime; 
            float maxChange = speed * Time.deltaTime; 

            t = Mathf.MoveTowards(t, targetValue, maxChange); 

            float visibility = Mathf.SmoothStep(0, 1, t); 
            SetVisibility(t); 

            yield return null; 
        }

        t = targetValue; 
        SetVisibility(t); 

        onCompleted(); 
        yield return null; 
    }



    //  UI  --------------------------------------------------------- 
    LayoutElement layoutElement; 
    float originalWidth; 

    void InitUI () 
    {
        layoutElement = GetComponent<LayoutElement>(); 
        originalWidth = layoutElement.preferredWidth; 
    }

    void SetVisibility (float visibility) 
    {
        infoPanel.SetActive(visibility > 0); 
        layoutElement.preferredWidth = visibility * originalWidth; 
    }

}

}
