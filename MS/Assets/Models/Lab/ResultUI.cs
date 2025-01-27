using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{

    public Volume volume;
    private ChromaticAberration chr;
    private ChannelMixer mixer;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    public ScreenShatter screen;

    private void Start()
    {
        volume.profile.TryGet(out chr);
        volume.profile.TryGet(out mixer);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);

        vignette.active = true;

        vignette.intensity.value = 0.462f;
        vignette.smoothness.value = 0.444f;

        chr.intensity.value = 0.386f;

        //Mixer
        mixer.redOutRedIn.value = 78f;
        mixer.redOutGreenIn.value = 40f;
        mixer.redOutBlueIn.value = 0f;

        mixer.greenOutRedIn.value = 2f;
        mixer.greenOutGreenIn.value = 100f;
        mixer.greenOutBlueIn.value = 0f;

        mixer.blueOutRedIn.value = 23f;
        mixer.blueOutGreenIn.value = 0f;
        mixer.blueOutBlueIn.value = 100f;

        lensDistortion.active = true;
        lensDistortion.intensity.value = 0.28f;
        lensDistortion.yMultiplier.value = 1f;
    }



}
