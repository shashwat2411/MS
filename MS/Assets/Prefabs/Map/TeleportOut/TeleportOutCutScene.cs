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

    private bool playCutScene = false;

    private void Start()
    {
        playCutScene = false;
    }
    private void Update()
    {
        if(playCutScene == false)
        {
            player.StopPlayback();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCutScene();
        }
    }

    public void TriggerPlayerDissolveOut()
    {
        player.SetBool("dissolveOut", true);
        player.SetBool("dissolveIn", false);

        //PlayerModelEventManager model = player.gameObject.GetComponent<PlayerModelEventManager>();

        //StartCoroutine(model.limbs.DissolveOut(0.75f));
        //StartCoroutine(model.head.DissolveOut(0.75f));
        //StartCoroutine(model.watch.DissolveOut(0.75f));
        //StartCoroutine(model.body.DissolveOut(0.75f));
        //StartCoroutine(model.helmet.DissolveOut(0.75f));
    }
    public void TriggerPlayerDissolveIn()
    {
        player.SetBool("dissolveOut", false);
        player.SetBool("dissolveIn", true);
    }

    private void StartCutScene()
    {
        playCutScene = true;

        FindFirstObjectByType<PlayerManager>().gameObject.GetComponent<Animator>().speed = 0f;

        canvas.SetBool("in", true);
        canvas.SetBool("out", false);

        camera.SetBool("zoomIn", true);
        camera.SetBool("zoomOut", false);
        camera.gameObject.GetComponent<CameraBrain>().ZoomInTrigger();

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
