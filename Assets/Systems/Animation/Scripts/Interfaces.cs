using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Animation 
{

public interface IAnimation 
{
	float value { get; set; } 
	float target { get; } 
	float animationTime { get; set; } 

	bool active { get; } 
	void Update (); 
	void Stop (); 
	void Resume (); 
}

public interface IAnimation_01 : IAnimation 
{
	void GoTo0 (); 
	void GoTo1 (); 
}

public interface IAnimation_AB : IAnimation 
{
	float A { get; set; } 
	float B { get; set; } 

	void GoToA (); 
	void GoToB (); 
}

}