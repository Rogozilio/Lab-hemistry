using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 



namespace VirtualLab 
{

[ExecuteAlways] 
public class Tooltip : MonoBehaviour
{
    [SerializeField] string message = "Tooltip message"; 
    [SerializeField] float appearDelay = 0.5f; 
    [SerializeField] float fadeTime = 0.25f; 
    [SerializeField] Vector2 offset = new Vector2(0, 20); 

    RectTransform rectTransform; 
    Text textComponent; 
    CanvasGroup canvasGroup; 

    new Camera camera; 



    void Awake () 
    {
        rectTransform = GetComponent<RectTransform>(); 
        textComponent = GetComponentInChildren<Text>(); 
        canvasGroup = GetComponent<CanvasGroup>(); 

        camera = Camera.main; 

        if (Application.isPlaying) 
            gameObject.SetActive(false); 
    }

    void Start () 
    {
        textComponent.text = message; 
    }

#if UNITY_EDITOR 
    void Update () 
    {
        if (!Application.isPlaying) 
            textComponent.text = message; 
    }
#endif 



    //  Position  --------------------------------------------------- 
    public void UpdatePosition (Vector3 worldPos) 
    {
        Vector2 screenPos = camera.WorldToScreenPoint(worldPos); 
        screenPos += offset; 

        rectTransform.anchoredPosition = screenPos; 
    }



    //  Visibility  ------------------------------------------------- 
    public bool visible => gameObject.activeSelf; 

    public void Show (Vector3 position) 
    {
        gameObject.SetActive(true); 
        
        UpdatePosition(position); 

        StopAllCoroutines(); 
        StartCoroutine(ShowAnimation()); 
    }

    public void Hide () 
    {
        if (!visible) return; 

        StopAllCoroutines(); 
        StartCoroutine(HideAnimation()); 
    }



    //  Animation  -------------------------------------------------- 
    IEnumerator ShowAnimation () 
    {
        // wait for some time 
        yield return new WaitForSeconds(appearDelay); 

		// if started disappearing before fully appearing 
        // this will work as if object started disappearing some time ago 
        float tStart = canvasGroup.alpha; 
        float startTime = Time.time - tStart * fadeTime; 

		// main part 
        while (true) 
        {
            float t = (Time.time - startTime) / fadeTime; 
            if (t >= 1) break; 

            canvasGroup.alpha = t; 

            yield return null; 
        }

        // end animation 
        canvasGroup.alpha = 1; 
    }

    IEnumerator HideAnimation () 
    {
        // if started disappearing before fully appearing 
        // this will work as if object started disappearing some time ago 
        float tStart = 1 - canvasGroup.alpha; 
        float startTime = Time.time - tStart * fadeTime; 

        // main part 
        while (true) 
        {
            float t = (Time.time - startTime) / fadeTime; 
            if (t >= 1) break; 

            canvasGroup.alpha = 1 - t; 

            yield return null; 
        }

        // end animation 
        canvasGroup.alpha = 0; 
        gameObject.SetActive(false); 
    }

}

}
