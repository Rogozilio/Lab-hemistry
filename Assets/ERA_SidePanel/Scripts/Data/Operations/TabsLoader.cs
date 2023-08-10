using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public static class TabsLoader
{

    //  Main  -------------------------------------------------------
    public static IEnumerator Load (MonoBehaviour owner, Result<List<TabData>> result) 
    {
        // load tab data 
        var resultTab0 = new Result<TabData>();
        var resultTab1 = new Result<TabData>();
        var resultTab2 = new Result<TabData>();
        var loadingTab0 = owner.StartCoroutine(LoadTab(owner, Tab.About,        resultTab0));
        var loadingTab1 = owner.StartCoroutine(LoadTab(owner, Tab.Theory,       resultTab1));
        var loadingTab2 = owner.StartCoroutine(LoadTab(owner, Tab.Instructions, resultTab2));
        yield return loadingTab0;
        yield return loadingTab1;
        yield return loadingTab2;

        // check results 
        if (!resultTab0.success) 
        {
            result.Error(resultTab0.message);
            yield break;
        }
        if (!resultTab1.success) 
        {
            result.Error(resultTab1.message);
            yield break;
        }
        if (!resultTab2.success) 
        {
            result.Error(resultTab2.message);
            yield break;
        }

        // create and return data 
        List<TabData> tabDataList = new List<TabData>
        {
            resultTab0.data,
            resultTab1.data,
            resultTab2.data,
        };
        result.Success(tabDataList);
    }



    //  Tab  --------------------------------------------------------
    static IEnumerator LoadTab (MonoBehaviour owner, Tab tab, Result<TabData> result) 
    { 
        var paths = CreatePaths(tab);

        // load name 
        string namePath = tab.ToString() + "/name.txt"; 
        var resultName = new Result<string>();
        var loadingName = StringLoader.Load(owner, namePath, resultName);
        yield return owner.StartCoroutine(loadingName);
        string name = resultName.success ? resultName.data : "";

        // load sprites 
        var resultSprites = new Result<List<Sprite>>();
        var loadingSprites = LoadSpriteList(owner, tab, paths, resultSprites);
        yield return owner.StartCoroutine(loadingSprites);

        if (!resultSprites.success)
        {
            result.Error(resultSprites.message);
            yield break;
        }
        
        // create and return data 
        var tabData = new TabData(tab, name, resultSprites.data);
        result.Success(tabData);
    }

    static IEnumerator LoadSpriteList (MonoBehaviour owner, Tab tab, IEnumerator<string> paths, Result<List<Sprite>> result) 
    {
        // load sprite list 
        var resultSprites = new Result<List<Sprite>>();
        yield return owner.StartCoroutine(
            ListLoader.Load<Sprite>(
                owner, 
                paths, 
                SpriteLoader.Load, 
                resultSprites
            )
        );

        // check sprite list 
        if (!resultSprites.success) 
        {
            result.Error(resultSprites.message);
            yield break;
        }
        if (resultSprites.data.Count == 0)
        {
            result.Error(
                "не удалось загрузить изображения для вкладки " + tab, 
                tab + "/",
                "изображения не найдены"
            );
            yield break;
        }

        result.Success(resultSprites.data);
    }



    //  Paths  ------------------------------------------------------
    static IEnumerator<string> CreatePaths (Tab tab) 
    {
        int    tabIndex   = (int) tab; 
        string folderName = tab.ToString(); 
        return new PathGenerator(folderName, tabIndex);
    }

    class PathGenerator : IEnumerator<string>
    {
        string folderName; 
        int tabIndex; 
        int pageNumber = 1; 

        public PathGenerator (string folderName, int tabIndex) 
        {
            this.folderName = folderName; 
            this.tabIndex = tabIndex; 
        }

        public void Dispose () {}

        public string Current { get; private set; }
        object IEnumerator.Current => Current;

        public bool MoveNext ()
        {
            string spriteName = (tabIndex + 1) + "-" + pageNumber++ + ".png"; 
            Current = folderName + "/" + spriteName; 
            return true;
        }

        public void Reset ()
        {
            pageNumber = 1;
        }
    }

}

}
