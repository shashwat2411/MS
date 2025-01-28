using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    public bool mainMenu = false;

    public AudioSource bgm;
    public Transform light;
    private AudioSource noise;
    public float pitch;
    public float sound;
    private float originalVolume;
    private float noiseOriginalVolume;
    private float originalVignette;

    private Volume volume;

    private ChromaticAberration chr;
    private ColorAdjustments color;
    private FilmGrain film;
    private ChannelMixer mixer;
    private DepthOfField dof;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private ShadowsMidtonesHighlights smh;
    private WhiteBalance whiteBalance;


    [Header("ChromaticAberration")]
    [Range(0, 1)] public float chromaticAberration;
    public AnimationCurve chromaticAberrationCurve;

    [Header("ColorAdjustments")]
    [Range(-180, 180)] public float hueShift;
    public AnimationCurve hueShiftCurve;
    [Range(-100, 100)] public float saturation;
    public AnimationCurve saturationCurve;

    [Header("FilmGrain")]
    [Range(0, 1)] public float filmGrain;
    public AnimationCurve filmGrainCurve;

    [Header("ChannelMixer")]
    [Header("Red")]
    [Range(-200, 200)] public float redInRed = 100f;
    [Range(-200, 200)] public float redInGreen;
    [Range(-200, 200)] public float redInBlue;
    [Header("Green")]
    [Range(-200, 200)] public float greenInRed;
    [Range(-200, 200)] public float greenInGreen = 100f;
    [Range(-200, 200)] public float greenInBlue;
    [Header("Blue")]
    [Range(-200, 200)] public float blueInRed;
    [Range(-200, 200)] public float blueInGreen;
    [Range(-200, 200)] public float blueInBlue = 100f;

    [Header("Vignette")]
    [Range(0, 1)] public float vignetteIntensity;
    public AnimationCurve vignetteCurve;

    [Header("LensDistortion")]
    [Range(0.01f, 5f)] public float lensDistortionScale = 1f;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        noise = GetComponent<AudioSource>();

        volume.profile.TryGet(out chr);
        volume.profile.TryGet(out color);
        volume.profile.TryGet(out film);
        volume.profile.TryGet(out mixer);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out smh);
        volume.profile.TryGet(out whiteBalance);

        originalVolume = 0.5f;
        noiseOriginalVolume = noise.volume;

        originalVignette = vignette.intensity.value;

        if (mainMenu == false)
        {
            dof.active = false;
            vignette.active = false;
            smh.active = false;
            whiteBalance.active = false;

            lensDistortion.intensity.value = 0.01f;
            lensDistortion.yMultiplier.value = 0f;

            film.response.value = 0f;
        }
    }
    void Update()
    {
        lensDistortion.scale.value = lensDistortionScale;

        if (mainMenu == false)
        {
            chr.intensity.value = chromaticAberration;

            color.hueShift.value = hueShift;
            color.saturation.value = saturation;

            film.intensity.value = filmGrain;

            vignette.intensity.value = vignetteIntensity;

            //Mixer
            mixer.redOutRedIn.value = redInRed;
            mixer.redOutGreenIn.value = redInGreen;
            mixer.redOutBlueIn.value = redInBlue;

            mixer.greenOutRedIn.value = greenInRed;
            mixer.greenOutGreenIn.value = greenInGreen;
            mixer.greenOutBlueIn.value = greenInBlue;

            mixer.blueOutRedIn.value = blueInRed;
            mixer.blueOutGreenIn.value = blueInGreen;
            mixer.blueOutBlueIn.value = blueInBlue;
        }
    }

    public IEnumerator ChromaticAberrationFadeOut(float duration)
    {
        float elapsed = 0f;
        chromaticAberration = 1f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            chromaticAberration = chromaticAberrationCurve.Evaluate(elapsed / duration);

            yield return null;
        }

        chromaticAberration = 0f;
    }

    public IEnumerator SaturationFadeOut(float duration, float value)
    {
        float elapsed = 0f;
        saturation = value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            saturation = value * saturationCurve.Evaluate(elapsed / duration);

            yield return null;
        }

        saturation = 100f;
    }
    public IEnumerator HueShift(float duration)
    {
        float elapsed = 0f;
        hueShift = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            hueShift = hueShiftCurve.Evaluate(elapsed / duration) * 180f;

            yield return null;
        }

        hueShift = 0f;
    }
    public IEnumerator FilmGrainFadeOut(float duration)
    {
        float elapsed = 0f;
        filmGrain = 1f;

        noise.Play();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            filmGrain = filmGrainCurve.Evaluate(elapsed / duration);
            noise.volume = filmGrainCurve.Evaluate(elapsed / duration) * noiseOriginalVolume;

            yield return null;
        }

        filmGrain = 0f;
        noise.volume = 0f;
    }
    public IEnumerator VignetteFadeIn(float duration)
    {
        vignette.active = true;

        float elapsed = 0f;
        vignetteIntensity = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            vignetteIntensity = vignetteCurve.Evaluate(elapsed / duration) * originalVignette;

            yield return null;
        }

        vignetteIntensity = originalVignette;
    }
    public IEnumerator VignetteFadeOut(float duration)
    {
        if (mainMenu == true) { yield return null; }

        vignette.active = true;

        float elapsed = duration;
        vignetteIntensity = originalVignette;

        while (elapsed > duration)
        {
            elapsed -= Time.unscaledDeltaTime;

            vignetteIntensity = vignetteCurve.Evaluate(elapsed / duration) * originalVignette;

            yield return null;
        }

        vignetteIntensity = 0f;
    }

    public IEnumerator ImpactEnhancer(float delay, float duration)
    {
        redInRed = -200f;
        redInGreen = -200f;
        redInBlue = 200f;

        greenInRed = 200f;
        greenInGreen = -173f;
        greenInBlue = -5f;

        blueInRed = -200f;
        blueInGreen = 200f;
        blueInBlue = -200f;

        float elapsed = 0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            bgm.volume = Mathf.Lerp(originalVolume, sound, elapsed / duration);
            bgm.pitch = Mathf.Lerp(1f, pitch, elapsed / duration);
        }

        yield return new WaitForSecondsRealtime(duration);

        bgm.volume = sound;
        bgm.pitch = pitch;

        yield return ResetChannelMixer(duration);
    }
    public IEnumerator ImpactEnhancer2(float delay, float duration)
    {
        redInRed = 200f;
        redInGreen = -200f;
        redInBlue = -200f;

        greenInRed = 17f;
        greenInGreen = 200f;
        greenInBlue = -200f;

        blueInRed = -200f;
        blueInGreen = -200f;
        blueInBlue = 200f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            bgm.volume = Mathf.Lerp(originalVolume, sound, elapsed / duration);
            bgm.pitch = Mathf.Lerp(1f, pitch, elapsed / duration);
        }

        yield return new WaitForSecondsRealtime(duration);

        bgm.volume = sound;
        bgm.pitch = pitch;

        yield return ResetChannelMixer(duration);
    }

    public IEnumerator ResetChannelMixer(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentage = elapsed / duration;

            redInRed = Mathf.Lerp(redInRed, 100f, percentage);
            redInGreen = Mathf.Lerp(redInRed, 0f, percentage);
            redInBlue = Mathf.Lerp(redInRed, 0f, percentage);

            greenInRed = Mathf.Lerp(redInRed, 0f, percentage);
            greenInGreen = Mathf.Lerp(redInRed, 100f, percentage);
            greenInBlue = Mathf.Lerp(redInRed, 0f, percentage);

            blueInRed = Mathf.Lerp(redInRed, 0f, percentage);
            blueInGreen = Mathf.Lerp(redInRed, 0f, percentage);
            blueInBlue = Mathf.Lerp(redInRed, 100f, percentage);

            bgm.volume = Mathf.Lerp(bgm.volume, originalVolume, percentage);
            bgm.pitch = Mathf.Lerp(bgm.pitch, 1f, percentage);

            yield return null;
        }

        redInRed = 100f;
        redInGreen = 0f;
        redInBlue = 0f;

        greenInRed = 0f;
        greenInGreen = 100f;
        greenInBlue = 0f;

        blueInRed = 0f;
        blueInGreen = 0f;
        blueInBlue = 100f;

        bgm.volume = originalVolume;
        bgm.pitch = 1f;
    }

    public IEnumerator ScaryEffectOn(float time, bool negative = false)
    {
        float elapsed = 0f;

        while(elapsed < time)
        {
            elapsed += Time.deltaTime;

            float percentage = elapsed / time;
            if (negative == true) { percentage = 1f - elapsed / time; }

            lensDistortion.intensity.value = Mathf.Lerp(0.35f, 0.75f, percentage);
            lensDistortion.xMultiplier.value = Mathf.Lerp(1f, 0f, percentage);
            lensDistortion.yMultiplier.value = Mathf.Lerp(1f, 0.128f, percentage);

            Vector4 shadows = smh.shadows.value;
            Vector4 midtones = smh.midtones.value;
            Vector4 highlights = smh.highlights.value;

            shadows.x = Mathf.Lerp(1f, 0.13f, percentage);
            shadows.y = Mathf.Lerp(1f, 0.40f, percentage);
            shadows.z = Mathf.Lerp(1f, 1.00f, percentage);

            midtones.x = Mathf.Lerp(1f, 1.00f, percentage);
            midtones.y = Mathf.Lerp(1f, 0.33f, percentage);
            midtones.z = Mathf.Lerp(1f, 0.00f, percentage);

            highlights.x = Mathf.Lerp(1f, 1.00f, percentage);
            highlights.y = Mathf.Lerp(1f, 0.95f, percentage);
            highlights.z = Mathf.Lerp(1f, 0.49f, percentage);

            smh.shadows.value = shadows;
            smh.midtones.value = midtones;
            smh.highlights.value = highlights;

            float x = Mathf.Lerp(297.407f, 347.34f, percentage);
            Quaternion rotation = light.rotation;
            rotation.eulerAngles = new Vector3(x, rotation.eulerAngles.y, rotation.eulerAngles.z);
            light.rotation = rotation;

            whiteBalance.temperature.value = Mathf.Lerp(0f, -70f, percentage);
            whiteBalance.tint.value = Mathf.Lerp(0f, 92f, percentage);

            yield return null;
        }
    }
}
