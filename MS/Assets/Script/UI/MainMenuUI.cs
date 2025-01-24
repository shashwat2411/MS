using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public Image title;
    public float size1;
    public float size2;

    public float speed = 0.4f;

    float counter = 0;
    bool switcher = false;

    public Volume volume;
    private ChromaticAberration chr;
    private ChannelMixer mixer;
    private DepthOfField dof;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    public ScreenShatter screen;

    private void Start()
    {
        volume.profile.TryGet(out chr);
        volume.profile.TryGet(out mixer);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);

        dof.active = true;
        vignette.active = true;

        vignette.intensity.value = 0.444f;
        vignette.smoothness.value = 0.444f;

        chr.intensity.value = 0.3f;

        //Mixer
        mixer.redOutRedIn.value = 117f;
        mixer.redOutGreenIn.value = 17f;
        mixer.redOutBlueIn.value = 0f;

        mixer.greenOutRedIn.value = 25f;
        mixer.greenOutGreenIn.value = 100f;
        mixer.greenOutBlueIn.value = 0f;

        mixer.blueOutRedIn.value = 0f;
        mixer.blueOutGreenIn.value = 0f;
        mixer.blueOutBlueIn.value = 100f;

        lensDistortion.active = true;
        lensDistortion.intensity.value = 0.35f;
        lensDistortion.yMultiplier.value = 1f;
    }

    private void FixedUpdate()
    {
        if (switcher == false)
        {
            counter += Time.deltaTime;

            if (counter < speed)
            {
                title.GetComponent<RectTransform>().localScale = Vector3.Lerp(title.GetComponent<RectTransform>().localScale, new Vector3(size1, size1, size1), 0.5f);
            }
            else if (counter >= speed) { counter = speed; switcher = true; }
        }
        else
        {
            counter -= Time.deltaTime;

            if (counter > 0f)
            {
                title.GetComponent<RectTransform>().localScale = Vector3.Lerp(title.GetComponent<RectTransform>().localScale, new Vector3(size2, size2, size2), 0.5f);
            }
            else if (counter <= 0f) { counter = 0f; switcher = false; }
        }
    }

}
