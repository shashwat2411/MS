using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AimerEvent : MonoBehaviour
{
    public BossAnimationEvents owner;

    public Transform[] aimers = new Transform[14];
    public GameObject lightning;

    private int index = 0;

    private CameraBrain mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraBrain>();
    }
    public void SpawnLightning()
    {
        Vector3 position = aimers[index].position;
        position.y = 0f;

        Instantiate(lightning, position, Quaternion.identity).GetComponent<BossLightning>().damage = owner.owner.lightningAttackPower;
        StartCoroutine(ShakeDelay());

        if (index < 14) { index++; }

        if (index >= 14) { index = 0; }
    }

    private IEnumerator ShakeDelay()
    {
        yield return new WaitForSeconds(0.15f);

        // Start the second coroutine
        SoundManager.Instance.PlaySE("Lighting");
        yield return StartCoroutine(mainCamera.CameraShake(0.4f, 0.1f));
    }

    public void ReturnToIdle()
    {
        owner.ReturnToIdle();
    }
}
