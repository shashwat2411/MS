using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeMainMenu : MonoBehaviour
{
    public ScreenShatter screenShatter;
    public void MainGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        StartCoroutine(screenShatter.ShatterScreenInitate());
    }
}
