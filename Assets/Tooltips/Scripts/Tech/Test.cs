using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

public class Test : MonoBehaviour
{

}


#if(UNITY_EDITOR)
[CustomEditor(typeof(Test))]
public class TestEditor : Editor 
{

    void OnSceneGUI () 
    {
        Debug.Log("Test");
    }

}
#endif
}