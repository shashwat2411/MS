using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MenkoExplosion : MonoBehaviour
{
    [HideInInspector] public float damage;

    public float cameraShakeDuration = 0.1f;
    public float cameraShakeIntensity = 5f;

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

        float duration = Mathf.Lerp(cameraShakeDuration * 0.8f, cameraShakeDuration * 1.2f, damage);
        float intensity = Mathf.Lerp(cameraShakeIntensity * 0.2f, cameraShakeIntensity, damage);
        StartCoroutine(Camera.main.GetComponent<CameraBrain>().CameraShake(duration, intensity));
    }
}
