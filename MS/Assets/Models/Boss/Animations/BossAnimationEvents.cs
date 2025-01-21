using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    public BossEnemy owner;
    public Animator aimer;

    public float cameraShakeIntensity;
    private CameraBrain mainCamera;

    public BoxCollider handCollider1;
    public BoxCollider handCollider2;

    private void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraBrain>();
    }
    public void ReturnToIdle()
    {
        owner.ReturnToIdle();
        ResetColliders();
    }
    public void StartAimerAnimation()
    {
        aimer.Play("LightningAimer");
    }

    public void CameraShake()
    {
        StartCoroutine(mainCamera.CameraShake(0.15f, cameraShakeIntensity));
    }

    public void DisableColliders()
    {
        owner.disableColliders = true;
    }

    public void ResetColliders()
    {
        owner.disableColliders = false;
    }
}
