using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 



namespace VirtualLab 
{

public class ParentSizeObserver : MonoBehaviour
{
    RectTransform parentRT; 



    void Awake () 
    {
        parentRT = transform.parent.GetComponent<RectTransform>(); 
    }

    void Start () 
    {
        SaveParentSize(); 
    }

    void Update () 
    {
        if (HasParentSizeChanged()) 
        {
            onParentSizeChagned.Invoke(parentSizeSaved, parentSizeCurrent); 
            SaveParentSize(); 
        }
    }



    //  Events  ----------------------------------------------------- 
    public UnityEvent<Vector2, Vector2> onParentSizeChagned; 



    //  Parent size  ------------------------------------------------ 
    Vector2 parentSizeSaved; 
    Vector2 parentSizeCurrent => parentRT.rect.size; 

    void SaveParentSize () 
    {
        parentSizeSaved = parentSizeCurrent; 
    }

    bool HasParentSizeChanged () 
    {
        return parentSizeSaved != parentSizeCurrent; 
    }

}

}
