using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportOutCutScene : MonoBehaviour
{
    public PlayerInput input;

    public Animator canvas;
    public Animator camera;
    public Animator player;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCutScene();
        }
    }

    private void StartCutScene()
    {
        canvas.SetBool("in", true);
        canvas.SetBool("out", false);

        camera.SetBool("zoomIn", true);
        camera.SetBool("zoomOut", false);

        input.enabled = false;
    }

    private void EndCutScene()
    {
        input.enabled = true;

        canvas.SetBool("in", false);
        canvas.SetBool("out", true);

        camera.SetBool("zoomIn", false);
        camera.SetBool("zoomOut", true);
    }
}
