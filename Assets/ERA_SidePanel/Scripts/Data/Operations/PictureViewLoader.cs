using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public static class PictureViewLoader
{

    public static IEnumerator Load (MonoBehaviour owner, Result<PictureViewData> result)
    {
        // load pic info 
        var resultPicInfo = new Result<PictureInfo>();
        var loading = owner.StartCoroutine(LoadPictureInfo(owner, resultPicInfo));
        yield return loading;

        // check results 
        if (!resultPicInfo.success)
        {
            result.Error(resultPicInfo.message);
            yield break;
        }

        // load pictures into the list if needed 
        List<PictureData> picDataList;
        if (resultPicInfo.data.hasPictures)
        {
            // get picture entries 
            var entries = GetPictureEntries(resultPicInfo.data);
            
            // load pictures 
            var resultPicData = new Result<List<PictureData>>();
            loading = owner.StartCoroutine(LoadPictures(owner, entries, resultPicData));
            yield return loading;

            // check results 
            if (!resultPicData.success)
            {
                result.Error(resultPicData.message);
                yield break;
            }
            if (resultPicData.data.Count != entries.Count)
            {
                result.Error("не удалось загрузить дополнительные картинки", "Pictures/", "некоторые картинки не найдены");
                yield break;
            }

            // use data from results 
            picDataList = resultPicData.data;
        }
        // or use an empty list 
        else 
        {
            picDataList = new List<PictureData>();
        }

        // create and return picture view data 
        var picViewData = new PictureViewData(picDataList);
        result.Success(picViewData);
    }

    static IEnumerator LoadPictureInfo (MonoBehaviour owner, Result<PictureInfo> result) 
    {
        // load pic info 
        var path          = "Pictures/info.json";
        var resultPicInfo = new Result<PictureInfo>();
        var loading       = owner.StartCoroutine(JSONLoader.Load<PictureInfo>(owner, path, resultPicInfo));
        yield return loading;

        // check pic info 
        if (!resultPicInfo.success) 
        {
            result.Error(resultPicInfo.message);
            yield break;
        }

        // make sure it all works 
        var picInfo = resultPicInfo.data;
        if (picInfo == null) picInfo = new PictureInfo();
        if (picInfo.about == null)        picInfo.about        = new PictureInfo.Entry[0];
        if (picInfo.theory == null)       picInfo.theory       = new PictureInfo.Entry[0];
        if (picInfo.instructions == null) picInfo.instructions = new PictureInfo.Entry[0];

        // return pic info 
        result.Success(picInfo);
    }

    static List<PictureInfo.Entry> GetPictureEntries (PictureInfo picInfo) 
    {
        // get picInfo entries for each tab 
        var entriesTab0 = picInfo.about;
        var entriesTab1 = picInfo.theory;
        var entriesTab2 = picInfo.instructions;

        // assign tabs to entries 
        foreach (var entry in entriesTab0) entry.tab = Tab.About;
        foreach (var entry in entriesTab1) entry.tab = Tab.Theory;
        foreach (var entry in entriesTab2) entry.tab = Tab.Instructions;

        // add them together 
        var entries = new List<PictureInfo.Entry>();
        entries.AddRange(entriesTab0);
        entries.AddRange(entriesTab1);
        entries.AddRange(entriesTab2);

        return entries;
    }

    static IEnumerator LoadPictures (MonoBehaviour owner, List<PictureInfo.Entry> entries, Result<List<PictureData>> result) 
    {
        // create paths to pictures 
        List<string> paths = new List<string>(entries.Count);
        for (int i = 0; i < entries.Count; i++)
        {
            paths.Add("Pictures/" + entries[i].pictureName);
        }

        // load sprites  
        var resultSprites = new Result<List<Sprite>>();
        var loading = owner.StartCoroutine(ListLoader.Load<Sprite>(owner, paths.GetEnumerator(), SpriteLoader.Load, resultSprites));
        yield return loading;

        // check sprites 
        if (!resultSprites.success)
        {
            result.Error(resultSprites.message);
            yield break;
        }

        // create and return pictures 
        var sprites     = resultSprites.data;
        var pictures    = new List<Picture>();
        var picDataList = new List<PictureData>();
        for (int i = 0; i < sprites.Count; i++) 
        {
            Tab    tab         = entries[i].tab;
            string name        = entries[i].pictureName;
            string description = entries[i].pictureDescription;
            int    pageNumber  = entries[i].pageNumber;
            int    x           = entries[i].x;
            int    y           = entries[i].y;
            pictures.Add(new Picture(name, description, sprites[i]));
            picDataList.Add(new PictureData(pictures[i], tab, pageNumber, x, y));
        }
        result.Success(picDataList);
    }

}

}
