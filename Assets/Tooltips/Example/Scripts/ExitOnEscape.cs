using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips
{

public class ExitOnEscape : MonoBehaviour
{

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

}

}
