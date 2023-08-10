using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 



namespace ERA.SidePanelAsset {

public class TabSystem : MonoBehaviour
{
    [SerializeField] List<TabButton> tabButtons; 
    [SerializeField] int firstSelectedTab = 0; 



    void Awake () 
    {
        InitButtons(); 
    }

    void Start () 
    {
        DoInitialSelect(); 
    }



	//  Init  ------------------------------------------------------- 
	void InitButtons () 
    {      
        foreach (TabButton button in tabButtons) 
            button.DeselectQuietly(); 

        foreach (TabButton button in tabButtons) 
        {
            button.onSelected += OnButtonSelected; 
        }
    }

    void DoInitialSelect () 
    {
        tabButtons[firstSelectedTab].Select(); 
    }



	//  Events  -----------------------------------------------------
    public delegate void TabChangedHandler (GameObject newTab); 
    public event TabChangedHandler onTabChanged = delegate {}; 

    public void OnDataLoaded (SidePanelData data) 
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            string name = data.GetTabData((Tab) i).name;
            tabButtons[i].SetName(name);
        }
    }



	//  Buttons  ---------------------------------------------------- 
	int GetSelectedButtonIndex () 
	{
		for (int i = 0; i < tabButtons.Count; i++) 
		{
			if (tabButtons[i].isSelected) return i; 
		}

		return -1; 
	}

	void SelectButton (int buttonIndex) 
	{
		tabButtons[buttonIndex].Select(); 
	}

	public void OnButtonSelected (TabButton button) 
    {
        DeselectAll(exceptThisOne: button); 
        onTabChanged.Invoke(button.TabArea); 
    }

    void DeselectAll (TabButton exceptThisOne) 
    {
        foreach (TabButton button in tabButtons)  
        {
            if (button == exceptThisOne) continue; 
            button.Deselect(); 
        }
    }



	//  Tabs  ------------------------------------------------------- 
	int firstTabIndex => 0; 
	int lastTabIndex => tabButtons.Count - 1; 

	public int currentTab 
	{
		get => GetSelectedButtonIndex(); 
	}

	public void SwitchToTab (int index) 
	{
		SelectButton(index); 
	}

	public void SwitchToNextTab () 
	{
		int current = currentTab; 

		if (current == lastTabIndex) 
		{
			SwitchToTab(firstTabIndex); 
		}
		else 
		{
			SwitchToTab(current + 1); 
		}
	}

	public void SwitchToPreviousTab () 
	{
		int current = currentTab; 

		if (current == firstTabIndex) 
		{
			SwitchToTab(lastTabIndex); 
		}
		else 
		{
			SwitchToTab(current - 1); 
		}
	}

}

}
