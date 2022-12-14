using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.OutlineNS 
{

public static class ExtensionMethods 
{
    public static void SetLayerRecursively(this GameObject obj, int layer) 
	{
        obj.layer = layer;
 
        foreach (Transform child in obj.transform) 
		{
            child.gameObject.SetLayerRecursively(layer); 
        }
    }
}

}
