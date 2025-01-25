using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeMainMenu : MonoBehaviour
{
    public ScreenShatter screenShatter;
    public AudioSource source;
    public void MainGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        source.Play();
        StartCoroutine(screenShatter.ShatterScreenInitate());
    }
}
