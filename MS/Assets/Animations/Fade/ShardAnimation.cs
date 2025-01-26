using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardAnimation : MonoBehaviour
{
    private ScreenShatter screen;
    [HideInInspector] public bool animation = false;
    [HideInInspector] public bool reverseAnimation = false;

    public AudioClip fade;
    private AudioSource source;

    private void Awake()
    {
        animation = false;
        reverseAnimation = false;
        screen = transform.parent.parent.parent.GetComponent<ScreenShatter>();

        source = GetComponent<AudioSource>();
    }
    public IEnumerator Shatter(float duration)
    {
        float elapsed = 0f;
        Vector3 position = Vector3.zero;

        source.Play();

        while(elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            position = transform.localPosition;
            position.z = screen.finalPositionZ * screen.shardShatterAnimationCurve.Evaluate(elapsed / duration);
            transform.localPosition = position;

            yield return null;
        }

        position = transform.localPosition;
        position.z = screen.finalPositionZ;
        transform.localPosition = position;
        animation = true;
    }

    public IEnumerator ShatterReverse(float duration)
    {
        float elapsed = 0f;
        Vector3 position = Vector3.zero;

        source.Play();

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            position = transform.localPosition;
            position.z = screen.finalPositionZ * screen.shardShatterReverseAnimationCurve.Evaluate(elapsed / duration);
            transform.localPosition = position;

            yield return null;
        }

        position = transform.localPosition;
        position.z = 0f;
        transform.localPosition = position;
        reverseAnimation = true;
    }
}
