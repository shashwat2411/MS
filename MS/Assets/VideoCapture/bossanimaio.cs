using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossanimaio : MonoBehaviour
{
    public Animator boss;
    public Animator camera;

    public float duration = 0.2f;
    public float intensity = 0.5f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            boss.Play("Scream2"); 
            boss.speed = 0;
            camera.Play("bossanimation");
        }
    }

    public void StartBossAnimation()
    {
        boss.Play("Scream2");
        boss.speed = 1;
    }

    public void Shake()
    {
        StartCoroutine(CameraShake(duration, intensity));
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
