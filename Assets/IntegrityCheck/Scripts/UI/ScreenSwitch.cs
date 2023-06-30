using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.IntegrityCheck
{

public class ScreenSwitch : MonoBehaviour
{
    [SerializeField] List<Screen> screens;



    //  Actions  ----------------------------------------------------
    public void SetScreen (string name) 
    {
        var desiredScreen = FindScreen(name);
        if (desiredScreen == null) throw new UnityException("Screen \"" + name + "\" was not found");

        SetActiveScreen(desiredScreen);
    }

    Screen FindScreen (string name) 
    {
        foreach (var screen in screens) 
        {
            if (screen.name == name) return screen;
        }

        return null;
    }

    void SetActiveScreen (Screen activeScreen) 
    {
        foreach (var screen in screens) 
        {
            screen.gameObject.SetActive(screen == activeScreen);
        }
    }



    //  Screen  -----------------------------------------------------
    [System.Serializable]
    public class Screen 
    {
        public string name;
        public GameObject gameObject;
    }

}

}
