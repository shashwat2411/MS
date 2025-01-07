using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    public BossEnemy owner;
    public Animator aimer;

    public float cameraShakeIntensity;
    private CameraBrain mainCamera;
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

    public void CameraShake()
    {
        StartCoroutine(mainCamera.CameraShake(0.15f, cameraShakeIntensity));
    }
}
