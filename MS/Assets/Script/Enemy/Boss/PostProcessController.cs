using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    public AudioSource bgm;
    private AudioSource noise;
    public float pitch;
    public float sound;
    private float originalVolume;
    private float noiseOriginalVolume;

    private Volume volume;

    private ChromaticAberration chr;
    private ColorAdjustments color;
    private FilmGrain film;
    private ChannelMixer mixer;


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

    private void Awake()
    {
        volume = GetComponent<Volume>();
        noise = GetComponent<AudioSource>();

        volume.profile.TryGet(out chr);
        volume.profile.TryGet(out color);
        volume.profile.TryGet(out film);
        volume.profile.TryGet(out mixer);

        originalVolume = 0.5f;
        noiseOriginalVolume = noise.volume;
    }
    void FixedUpdate()
    {
        chr.intensity.value = chromaticAberration;

        color.hueShift.value = hueShift;
        color.saturation.value = saturation;

        film.intensity.value = filmGrain;

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

        saturation = 0f;
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
}
