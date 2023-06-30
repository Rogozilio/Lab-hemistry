using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public static class TooltipsInfoLoader 
{

    public static IEnumerator Load (MonoBehaviour owner, Result<TooltipsInfo> result) 
    {
        // load tooltips info 
        var resultInfo = new Result<TooltipsInfo>();
        var loading = JSONLoader.Load<TooltipsInfo>(owner, "tooltips.json", resultInfo);
        yield return owner.StartCoroutine(loading);

        // check that object exists 
        if (!resultInfo.success)
        {
            result.Error(resultInfo.message);
            yield break;
        }

        // check object and return 
        var info = resultInfo.data;
        info.Validate();
        if (info.IsGood()) result.Success(info);
        else               result.Error("Ошибка в данных для выноски");
    }

}

}
