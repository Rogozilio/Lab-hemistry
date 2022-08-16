using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SpriteListLoader : Operation<List<Sprite>> 
{
    IPathGenerator pathGenerator; 
    SpriteLoader spriteLoader; 

    List<Sprite> sprites = new List<Sprite>(); 



    public SpriteListLoader (OnCompleted onCompleted, OnError onError = null) 
        : base(onCompleted, onError) {} 



    //  Operation  -------------------------------------------------- 
    public void Start (IPathGenerator pathGenerator) 
    {
        this.pathGenerator = pathGenerator; 
        spriteLoader = new SpriteLoader(OnSpriteLoaded, OnLoadError); 

        LoadNextSprite(); 
    }



    //  Loading sprite  --------------------------------------------- 
    void LoadNextSprite () 
    {
        string path = pathGenerator.NextPath(); 
        spriteLoader.Start(path); 
    }

    void OnSpriteLoaded (Sprite sprite) 
    {
        sprites.Add(sprite); 
		LoadNextSprite(); 
    }

	void OnLoadError (string message) 
	{
		GoBack(sprites); 
	}

}

}
