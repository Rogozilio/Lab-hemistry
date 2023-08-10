using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



namespace ERA.SidePanelAsset
{

public static class ListLoader 
{

    //  Callbacks  --------------------------------------------------
    public delegate IEnumerator LoadItemAction<T> (MonoBehaviour owner, string path, Result<T> result);



    //  Main  -------------------------------------------------------
    public static IEnumerator Load<T> (MonoBehaviour owner, IEnumerator<string> paths, LoadItemAction<T> loadItem, Result<List<T>> result) 
    {
        // find paths that exist 
        var resultsPaths = new Result<List<string>>();
        var findingPaths = FindPathsThatExist(owner, paths, resultsPaths);
        yield return owner.StartCoroutine(findingPaths);

        // check paths that exist 
        if (!resultsPaths.success) 
        {
            result.Error(resultsPaths.message);
            yield break;
        }

        // start loading items 
        var pathsThatExist = resultsPaths.data;
        var resultsItems   = new List<Result<T>>(pathsThatExist.Count);
        var coroutines     = new List<Coroutine>(pathsThatExist.Count);
        for (int i = 0; i < pathsThatExist.Count; i++)
        {
            resultsItems.Add(new Result<T>());
            coroutines.Add(owner.StartCoroutine(loadItem(owner, pathsThatExist[i], resultsItems[i])));
        }

        // wait for all items to load 
        foreach (var loading in coroutines)
        {
            yield return loading;
        }

        // check items 
        foreach (var resultItem in resultsItems) 
        {
            if (!resultItem.success)
            {
                result.Error(resultItem.message);
                yield break;
            }
        }

        // create and return item list 
        var items = new List<T>(resultsItems.Count);
        for (int i = 0; i < resultsItems.Count; i++)
        {
            items.Add(resultsItems[i].data);
        }
        result.Success(items);
    }

    static IEnumerator FindPathsThatExist (MonoBehaviour owner, IEnumerator<string> paths, Result<List<string>> result) 
    {
        List<string> pathsThatExist = new List<string>();

        while (paths.MoveNext()) 
        {
            string path = paths.Current;
            var resultExistence = new Result<bool>();
            var checking = Network.CheckExistence(path, resultExistence);
            yield return owner.StartCoroutine(checking);

            bool exists = resultExistence.success && resultExistence.data;
            if (!exists) break;

            pathsThatExist.Add(path);
        }

        result.Success(pathsThatExist);
    }

}

}
