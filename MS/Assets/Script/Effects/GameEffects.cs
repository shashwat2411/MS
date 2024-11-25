using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffects : MonoBehaviour
{
    public float slowDownFactor = 0.1f;
    public float slowDownLength = 1f;

    private bool hitStop;
    private float timeScaleBackUp;

    private void Start()
    {
        hitStop = false;
        timeScaleBackUp = Time.timeScale;
    }
    private void Update()
    {
        if (hitStop == false)
        {
            Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            if (Input.GetKey(KeyCode.T))
            {
                DoSlowMotion(1f, 0.1f);
            }
        }
    }

    public void DoSlowMotion(float duration, float scale)
    {
        slowDownLength = duration;
        slowDownFactor = scale;

        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public IEnumerator HitStop(float duration)
    {
        timeScaleBackUp = Time.timeScale;
        Time.timeScale = 0f;
        hitStop = true;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = timeScaleBackUp;
        hitStop = false
    }
}
