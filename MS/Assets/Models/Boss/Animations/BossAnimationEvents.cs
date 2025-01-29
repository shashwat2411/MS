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
    public float cutSceneScreamShakeIntensity;
    public float deathSceneShakeIntensity;
    public float impactTime;
    public float colorReturnTime;
    private CameraBrain mainCamera;

    public PostProcessController postProcess;
    protected static int _Speed = Animator.StringToHash("_Speed");

    public float saturationFadeOutTime = 1f;

    public ScreenShatter shatterer;
    public string nextScene;

    public ParticleSystem charge;
    public ParticleSystem chargeBall;
    public ParticleSystem laserbeam;

    private bool cutScene = true;
    private void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraBrain>();
        cutScene = true;
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
    public void StartDeathSound()
    {
        StartCoroutine(mainCamera.CameraShake(2f, deathSceneShakeIntensity));
    }
    public void NoiseScreen()
    {

        if (cutScene == false)
        {
            StartCoroutine(mainCamera.CameraShake(1.5f, screamShakeIntensity));
            StartCoroutine(postProcess.SaturationFadeOut(saturationFadeOutTime, -100f));
            StartCoroutine(postProcess.FilmGrainFadeOut(saturationFadeOutTime));
        }
        else
        {
            cutScene = false;
            StartCoroutine(mainCamera.CameraShake(1.5f, cutSceneScreamShakeIntensity));
            StartCoroutine(postProcess.HueShift(1.5f));
        }
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

    public void StartScreenShatter()
    {
        shatterer.loadLevel = nextScene;
        StartCoroutine(shatterer.ShatterScreenInitate());
    }

    public void StartCharge()
    {
        charge.Play();
    }
    public void ShootLaserBeam()
    {
        charge.Stop();
        laserbeam.Play();
    }
}
