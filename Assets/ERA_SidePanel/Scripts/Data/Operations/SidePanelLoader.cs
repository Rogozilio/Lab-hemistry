using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public static class SidePanelLoader
{

    public static IEnumerator Load (MonoBehaviour owner, Result<SidePanelData> result) 
    {
        // load tabs and picture view 
        var resultTabs        = new Result<List<TabData>>();
        var resultPictureView = new Result<PictureViewData>();
        var loadingTabs        = owner.StartCoroutine(TabsLoader.Load(owner, resultTabs));
        var loadingPictureView = owner.StartCoroutine(PictureViewLoader.Load(owner, resultPictureView));
        yield return loadingTabs;
        yield return loadingPictureView;

        // check results 
        if (!resultTabs.success) 
        {
            result.Error(resultTabs.message);
            yield break;
        }
        if (!resultPictureView.success) 
        {
            result.Error(resultPictureView.message);
            yield break;
        }

        // create and return data
        var sidePanelData = new SidePanelData(resultTabs.data, resultPictureView.data);
        result.Success(sidePanelData);
    }

    
}

}
