using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleSystemDestroyer : MonoBehaviour
{
    public GameObject toDestroy;
    private ParticleSystem paritcleSystem;
    private VisualEffect visualEffect;
    private void Start()
    {
        paritcleSystem = GetComponent<ParticleSystem>();

        if (paritcleSystem == null) { visualEffect = GetComponent<VisualEffect>(); visualEffect.Play(); }
    }
    private void FixedUpdate()
    {
        if (paritcleSystem != null)
        {
            if (paritcleSystem.isPlaying == false)
            {
                //Debug.Log("Destroy Particle");

                if (toDestroy != null) { Destroy(toDestroy); }
                else { paritcleSystem.Stop(); }
            }
        }
        else if(visualEffect != null)
        {
            if(visualEffect.HasAnySystemAwake() == false)
            {
                //Debug.Log("Destroy VFX");

                if (toDestroy != null) { Destroy(toDestroy); }
                else { visualEffect.Stop(); }
            }
        }
    }
}
