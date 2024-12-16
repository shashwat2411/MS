using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MenkoExplosion : MonoBehaviour
{
    private VisualEffect sparkParticles;
    private Animator animator;

    private void Start()
    {
        sparkParticles = GetComponentInChildren<VisualEffect>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(gameObject);
        }
    }

    private void StartExplosion()
    {
        sparkParticles.Play();
    }
}
