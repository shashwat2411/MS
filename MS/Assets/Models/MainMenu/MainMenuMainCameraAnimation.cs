using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMainCameraAnimation : MonoBehaviour
{
    private PostProcessController postProcess;
    private AudioSource source;

    public float transitionTime = 0.2f;
    public float pitchValue = 0.9f;

    private void Awake()
    {
        postProcess = FindFirstObjectByType<PostProcessController>();
        source = GetComponent<AudioSource>();
    }

    public void ScaryEffectOn()
    {
        StartCoroutine(postProcess.ScaryEffectOn(transitionTime));
        StartCoroutine(SoundEffect(transitionTime, 1f, pitchValue));
    }
    public void ScaryEffectOff()
    {
        StartCoroutine(postProcess.ScaryEffectOn(transitionTime, true));
        StartCoroutine(SoundEffect(transitionTime, pitchValue, 1f));
    }

    private IEnumerator SoundEffect(float duration, float startValue, float endValue)
    {
        float elapsed = 0f;

        source.pitch = startValue;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            source.pitch = Mathf.Lerp(startValue, endValue, elapsed / duration);

            yield return null;
        }

        source.pitch = endValue;
    }
}
