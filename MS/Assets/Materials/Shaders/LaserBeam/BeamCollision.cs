using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCollision : MonoBehaviour
{
    public float beamDamage = 20f;
    GameObject player;
    ParticleSystem particleSystem;
    List<ParticleSystem.Particle> particlesList = new List<ParticleSystem.Particle>();
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>().gameObject;
        particleSystem = GetComponent<ParticleSystem>();

        Component collider = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>();
        particleSystem.trigger.SetCollider(0, collider);
    }

    private void OnParticleTrigger()
    {
        int triggeredParticles = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList);


        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = particlesList[i];
            p.remainingLifetime = 0f;


            Debug.Log("Beam Hit");
            particlesList[i] = p;

            player.GetComponent<PlayerManager>().playerHP.Damage(beamDamage);
        }

        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList);
    }
}
