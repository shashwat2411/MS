using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public class MenkoExplosion : MonoBehaviour
{
    private Color baseColor;
    [ColorUsage(false, true)] private Color emission;

    private Color firstBaseColor;
    [ColorUsage(false, true)] private Color firstEmissionColor;

    public Color differentBaseColor;
    [ColorUsage(false, true)] public Color differentEmissionColor;

    [HideInInspector] public float damage;

    private bool switchColor = false;
    public float cameraShakeDuration = 0.1f;
    public float cameraShakeIntensity = 5f;

    public AnimationCurve baseColorAlpha;
    public AnimationCurve[] emissionColor = new AnimationCurve[4];

    private float timer;
    private VisualEffect sparkParticles;
    private Animator animator;
    private Material material;

    //Hash Map
    private int _BaseColor = Shader.PropertyToID("_BaseColor");
    private int _EmissiveColor = Shader.PropertyToID("_EmissiveColor");

    public Color newBaseColor;
    [ColorUsage(false, true)] public Color newEmissionColor;

    private void Start()
    {
        timer = 0f;

        sparkParticles = GetComponentInChildren<VisualEffect>();
        animator = GetComponent<Animator>();

        material = Instantiate(GetComponentInChildren<MeshRenderer>().material);
        GetComponentInChildren<MeshRenderer>().material = material;

        baseColor = material.GetColor(_BaseColor);
        emission = material.GetColor(_EmissiveColor);

        firstBaseColor = baseColor;
        firstEmissionColor = emission;

        if (switchColor == true)
        {
            baseColor = differentBaseColor;
            emission = differentEmissionColor;
        }
        else
        {
            baseColor = firstBaseColor;
            emission = firstEmissionColor;
        }
    }

    private void FixedUpdate()
    {
        if(timer <= 1f) { timer += Time.deltaTime; }
        else { timer = 1f; }

        float baseAlpha = baseColorAlpha.Evaluate(timer);
        float emissionRed = emissionColor[0].Evaluate(timer);
        float emissionGreen = emissionColor[1].Evaluate(timer);
        float emissionBlue = emissionColor[2].Evaluate(timer);
        float emissionAlpha = emissionColor[3].Evaluate(timer);

        newBaseColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseAlpha);
        newEmissionColor = new Color(emission.r * emissionRed, emission.g * emissionGreen, emission.b * emissionBlue, emission.a * emissionAlpha);

        material.SetColor(_BaseColor, newBaseColor);
        material.SetColor(_EmissiveColor, newEmissionColor);

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
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

    public void SwitchColors(bool red = true)
    {
        switchColor = red;
    }
}
