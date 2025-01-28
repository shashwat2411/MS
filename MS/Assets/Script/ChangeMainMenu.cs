using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeMainMenu : MonoBehaviour
{
    public ScreenShatter screenShatter;
    public AudioSource source;
    public MainMenuMainCameraAnimation animation;
    public void MainGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        source.Play();
        animation.
        StartCoroutine(animation.ScaryEffectOn());
    }
    public void Tutorial(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        source.Play();
        screenShatter.loadLevel = "Tutorial";
        StartCoroutine(animation.ScaryEffectOn());
    }
}
