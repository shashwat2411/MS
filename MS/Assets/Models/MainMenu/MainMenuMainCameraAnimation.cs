using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMainCameraAnimation : MonoBehaviour
{
    private PostProcessController postProcess;
    public AudioSource source;
    public ScreenShatter screenShatter;

    public float transitionTime = 0.2f;
    public float pitchValue = 0.9f;

    private void Awake()
    {
        postProcess = FindFirstObjectByType<PostProcessController>();
    }

    public IEnumerator ScaryEffectOn()
    {
        yield return postProcess.ScaryEffectOn(transitionTime);
        yield return SoundEffect(transitionTime, 1f, pitchValue);

        yield return InitiateFade();
    }

    private IEnumerator InitiateFade()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if (screenShatter.currentlyFadingIn == false)
        {
            yield return screenShatter.ShatterScreenInitate();
        }
        else
        {
            yield return InitiateFade();
        }
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
