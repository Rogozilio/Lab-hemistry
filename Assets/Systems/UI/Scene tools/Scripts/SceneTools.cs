using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirtualLab.PlayerMotion; 



namespace VirtualLab {

public class SceneTools : MonoBehaviour
{
    // parameters 
    [SerializeField] Button left; 
    [SerializeField] Button right; 
    [SerializeField] Button up; 
    [SerializeField] Button down; 
    [SerializeField] Button forward; 
    [SerializeField] Button back; 
	[SerializeField] PlayerMotion.PlayerMotion playerMotion; 
	[SerializeField] CameraZoom cameraZoom; 



    void Update () 
    {
        UpdateUI(); 
    }



    //  UI  --------------------------------------------------------- 
    void UpdateUI () 
    {
        left.interactable    = playerMotion.CanRotateLeft; 
        right.interactable   = playerMotion.CanRotateRight; 
        up.interactable      = playerMotion.CanRotateUp; 
        down.interactable    = playerMotion.CanRotateDown; 
        forward.interactable = cameraZoom.CanZoomIn; 
        back.interactable    = cameraZoom.CanZoomOut; 
    }



    //  Actions  ---------------------------------------------------- 
	public void MoveToPoint (int pointID) 
	{
		playerMotion.MoveToPoint(pointID); 
	}

    public void RotateLeft ()  => playerMotion.Rotate(new Orientation(-1,  0)); 
    public void RotateRight () => playerMotion.Rotate(new Orientation( 1,  0)); 
    public void RotateUp ()    => playerMotion.Rotate(new Orientation( 0,  1)); 
    public void RotateDown ()  => playerMotion.Rotate(new Orientation( 0, -1)); 

    public void ZoomIn () 	   => cameraZoom.ZoomIn(); 
    public void ZoomOut ()     => cameraZoom.ZoomOut(); 

}

}
