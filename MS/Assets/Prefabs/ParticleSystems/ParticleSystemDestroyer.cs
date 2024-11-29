using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDestroyer : MonoBehaviour
{
    private ParticleSystem paritcleSystem;
    private void Start()
    {
        paritcleSystem = GetComponent<ParticleSystem>();
    }
    private void FixedUpdate()
    {
        if (paritcleSystem != null)
        {
            if (paritcleSystem.isPlaying == false)
            {
                Debug.Log("Destroy Particle");
                Destroy(gameObject);
            }
        }
    }
}
