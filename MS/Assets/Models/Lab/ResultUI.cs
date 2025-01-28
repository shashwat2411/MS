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
    private ShadowsMidtonesHighlights smh;

    public ScreenShatter screen;

    private void Start()
    {
        volume.profile.TryGet(out chr);
        volume.profile.TryGet(out mixer);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out smh);

        vignette.active = true;
        smh.active = true;

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


        if (PlayerSave.Instance.victory == false)
        {
            Vector4 shadows = smh.shadows.value;
            Vector4 midtones = smh.midtones.value;
            Vector4 highlights = smh.highlights.value;

            shadows.x = 1f;
            shadows.y = 0.19f;
            shadows.z = 0.26f;

            midtones.x = 1f;
            midtones.y = 0.54f;
            midtones.z = 0.66f;

            highlights.x = 0.8f;
            highlights.y = 1f;
            highlights.z = 0.23f;

            smh.shadows.value = shadows;
            smh.midtones.value = midtones;
            smh.highlights.value = highlights;
        }
    }



}
