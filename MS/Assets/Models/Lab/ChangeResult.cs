using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.TimeZoneInfo;

public class ChangeResult : MonoBehaviour
{
    public ScreenShatter screenShatter;
    public AudioSource source;
    public PostProcessController postProcess;

    private bool once = false;

    private void Awake()
    {
        once = false;
    }
    public void MainGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if(once == false)
        {
            once = true;
            source.Play();
            StartCoroutine(ScaryEffectOn());
        }
    }
    public IEnumerator ScaryEffectOn()
    {
        yield return postProcess.ScaryEffectOn(0.2f);
        yield return SoundEffect(0.2f, 1f, 0.8f);

        yield return screenShatter.ShatterScreenInitate();
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
