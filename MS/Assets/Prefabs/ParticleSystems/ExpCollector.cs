using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpCollector : MonoBehaviour
{
    public float gainedExp = 5f;

    GameObject player;
    ParticleSystem particleSystem;

    List<ParticleSystem.Particle> particlesList = new List<ParticleSystem.Particle>();
    List<bool> collision;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerManager>().gameObject;

        particleSystem = GetComponent<ParticleSystem>();

        Component collider = GameObject.FindWithTag("ExpCollector").GetComponent<CapsuleCollider>();
        particleSystem.trigger.SetCollider(0, collider);
    }

    private void OnParticleTrigger()
    {
        int triggeredParticles = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList);


        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = particlesList[i];
            p.remainingLifetime = 0f; 
            

            Debug.Log("Collected 1 particle !");
            particlesList[i] = p;

            player.GetComponent<PlayerManager>().playerExp.ExpFill(gainedExp);
        }

        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList);
    }
}
