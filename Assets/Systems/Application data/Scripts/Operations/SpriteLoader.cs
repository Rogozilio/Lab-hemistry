using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SpriteLoader : Operation<Sprite> 
{

    public SpriteLoader (OnCompleted onCompleted, OnError onError = null) 
        : base(onCompleted, onError) {}



    public void Start (string path) 
    {
        new AssetLoader(
			(data) => OnDataLoaded(path, data), 
			(_) => OnLoadError(path) 
		).Start(path); 
    }

    void OnDataLoaded (string path, byte [] data)
    {
        Texture2D texture = CreateTexture(data, path); 
		Sprite sprite = CreateSprite(texture); 

        GoBack(sprite); 
    }

	Texture2D CreateTexture (byte [] data, string name) 
	{
		Texture2D texture = new Texture2D(1, 1); 
        texture.LoadImage(data); 
		texture.name = name; 
		return texture; 
	}

	Sprite CreateSprite (Texture2D texture) 
	{
		Sprite sprite = Sprite.Create(
            texture, 
            new Rect(0, 0, texture.width, texture.height), 
            new Vector2(0.5f, 0.5f) 
        ); 

		sprite.name = texture.name; 

		return sprite; 
	}

	void OnLoadError (string path) 
	{
		GoBackWithError("Не получилось загрузить спрайт: " + path); 
	}

}

}