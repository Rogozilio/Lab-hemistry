using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public static class SpriteLoader 
{

    //  Main  -------------------------------------------------------
    public static IEnumerator Load (MonoBehaviour owner, string path, Result<Sprite> result) 
    {
        // load bytes 
        var resultBytes = new Result<byte[]>();
        var loading = Network.Load(path, resultBytes);
        yield return owner.StartCoroutine(loading);

        // check bytes 
        if (!resultBytes.success) 
        {
            result.Error(resultBytes.message);
            yield break;
        }

        // create and return sprite 
        try {
            Texture2D texture = CreateTexture(resultBytes.data, path); 
            Sprite    sprite  = CreateSprite(texture); 
            result.Success(sprite);
        }
        catch (Exception) 
        {
            string title = "не удалось создать изображение из байт";
            result.Error(title, path);
        }
    }



    //  Creating data  ----------------------------------------------
    static Texture2D CreateTexture (byte [] data, string name) 
	{
		Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false); 
        bool success = texture.LoadImage(data); 
        if (!success) throw new UnityException();

		texture.name = name; 
        texture.filterMode = FilterMode.Bilinear;

		return texture; 
	}

    static Sprite CreateSprite (Texture2D texture) 
	{
		Sprite sprite = Sprite.Create(
            texture, 
            new Rect(0, 0, texture.width, texture.height), 
            new Vector2(0.5f, 0.5f) 
        ); 

		sprite.name = texture.name; 

		return sprite; 
	}

}

}
