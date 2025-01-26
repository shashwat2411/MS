using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapCheck : MonoBehaviour
{
    PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<SkillWindow>().player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.currentActionMap.name == "Player")
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    private void OnDisable()
    {
        if (playerInput.currentActionMap.name == "UI")
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
