using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class TabDataLoader : Operation<TabData> 
{
    Tab tab; 



    public TabDataLoader (OnCompleted onCompleted, OnError onError = null) 
        : base(onCompleted, onError) {} 



    public void Start (Tab tab) 
    {
        this.tab = tab; 

        int tabIndex = (int) tab; 
        string folderName = AppData.TAB_NAMES[tabIndex]; 

        PathGenerator pathGen = new PathGenerator(
            folderName, 
            tabIndex
        ); 

        new SpriteListLoader(OnDataLoaded, GoBackWithError).Start(pathGen); 
    }

    void OnDataLoaded (List<Sprite> pages) 
    {
		TabData tabData = new TabData(tab, pages); 
        GoBack(tabData); 
    }


    

    class PathGenerator : IPathGenerator 
    {
        string folderName; 
        int tabIndex; 
        int pageNumber = 1; 

        public PathGenerator (string folderName, int tabIndex) 
        {
            this.folderName = folderName; 
            this.tabIndex = tabIndex; 
        }

        public string NextPath () 
        {
            string spriteName = (tabIndex + 1) + "-" + pageNumber++ + ".png"; 
            return folderName + "/" + spriteName; 
        }
    }

}

}
