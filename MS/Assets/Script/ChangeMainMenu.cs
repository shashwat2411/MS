using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeMainMenu : MonoBehaviour
{
    public ScreenShatter screenShatter;
    public AudioSource source;
    public MainMenuMainCameraAnimation animation;

    private bool once = false;

    private void Awake()
    {
        once = false;
    }
    public void MainGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (once == false)
        {
            once = true;
            source.Play();
            animation.
            StartCoroutine(animation.ScaryEffectOn());
        }
    }
    public void Tutorial(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (once == false)
        {
            once = true;
            source.Play();
            screenShatter.loadLevel = "Tutorial";
            StartCoroutine(animation.ScaryEffectOn());
        }
    }
}
