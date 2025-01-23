    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossAnimationEvents : MonoBehaviour
{
    public BossEnemy owner;
    public Animator aimer;

    public float cameraShakeIntensity;
    public float screamShakeIntensity;
    public float impactTime;
    public float colorReturnTime;
    private CameraBrain mainCamera;

    public PostProcessController postProcess;
    protected static int _Speed = Animator.StringToHash("_Speed");

    public float saturationFadeOutTime = 1f;

    private void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraBrain>();
    }

    public void ReturnToIdle()
    {
        owner.ReturnToIdle();
    }
    public void StartAimerAnimation()
    {
        aimer.Play("LightningAimer");
    }

    public void StartScreamSound()
    {
        SoundManager.Instance.PlaySE("BossScream");
    }
    public void StartBeforeSlapSound()
    {
        SoundManager.Instance.PlaySE("BossBeforeSlap", owner.GetAnimator().GetFloat(_Speed));
    }
    public void StartSlapSound()
    {
        SoundManager.Instance.PlaySE("BossSlap", owner.GetAnimator().GetFloat(_Speed));
    }
    public void StartBeforeSmashSound()
    {
        SoundManager.Instance.PlaySE("BossBeforeSmash", owner.GetAnimator().GetFloat(_Speed));
    }
    public void StartSmashSound()
    {
        SoundManager.Instance.PlaySE("BossSmash", owner.GetAnimator().GetFloat(_Speed));
    }
    public void StartLightningSound()
    {
        SoundManager.Instance.PlaySE("BossLightning", owner.GetAnimator().GetFloat(_Speed));
    }
    public void NoiseScreen()
    {
        StartCoroutine(mainCamera.CameraShake(1.5f, screamShakeIntensity));
        StartCoroutine(postProcess.SaturationFadeOut(saturationFadeOutTime, -100f));
        StartCoroutine(postProcess.FilmGrainFadeOut(saturationFadeOutTime));
    }

    public void ImpactCameraShake()
    {
        StartCoroutine(mainCamera.CameraShake(0.2f, cameraShakeIntensity));
        if (owner.bossHealthBar.health / owner.bossHealthBar.maxHealth < owner.phaseChangeThreshold)
        {
            int type = Random.Range(0, 2);

            if (type == 0) { StartCoroutine(postProcess.ImpactEnhancer(impactTime, colorReturnTime)); }
            else { StartCoroutine(postProcess.ImpactEnhancer2(impactTime, colorReturnTime)); }
        }
    }
    public void CameraShake()
    {
        StartCoroutine(mainCamera.CameraShake(0.2f, cameraShakeIntensity));
    }

    public void DisableColliders()
    {
        owner.disableColliders = true;
    }
}
