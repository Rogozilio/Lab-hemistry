using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace VirtualLab.IntegrityCheck
{

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] int fadeDistance = 5;
    [SerializeField] float speed = 3;
    [SerializeField] Color neutralColor = Color.white;
    [SerializeField] Color activeColor  = Color.green;
    [SerializeField] List<Image> particles;



    void Update () 
    {
        UpdateAngle();
        UpdateParticles();
    }



    //  Angle  ------------------------------------------------------
    public int position;
    public float positionContinious;

    void UpdateAngle () 
    {
        positionContinious += speed * Time.deltaTime;
        positionContinious %= 8;
        
        position = Mathf.FloorToInt(positionContinious);
    }



    //  Particles  --------------------------------------------------
    void UpdateParticles () 
    {
        for (int i = 0; i < particles.Count; i++) 
        {
            UpdateParticle(i);
        }
    }

    void UpdateParticle (int index)
    {
        float acitvity = GetParticleActivity(index);
        SetParticleColor(particles[index], acitvity);
    }

    float GetParticleActivity (int index) 
    {
        int positionRelative = position - index;
        if (positionRelative < 0) positionRelative += 8;

        if (positionRelative < fadeDistance) 
        {
            return 1 - (float) positionRelative / (float) fadeDistance;
        }
        else 
        {
            return 0;
        }
    }

    void SetParticleColor (Image particle, float activity) 
    {
        particle.color = Color.Lerp(neutralColor, activeColor, activity);
    }

}
  
}
