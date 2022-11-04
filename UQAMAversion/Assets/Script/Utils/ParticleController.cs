using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    
    //Start particle effect with a function call
    public void StartParticleEffect()
    {
        particleSystem.Play();
    }
    
    //Stop particle effect with a function call
    public void StopParticleEffect()
    {
        particleSystem.Stop();
    }
    
    
}
