using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    private DepthOfField dof;
    private Vignette vignette;

    private void Start()
    {
        volume.profile.TryGet(out chr);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out vignette);

        dof.active = true;
        vignette.active = true;

        chr.intensity.value = 0.3f;
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
