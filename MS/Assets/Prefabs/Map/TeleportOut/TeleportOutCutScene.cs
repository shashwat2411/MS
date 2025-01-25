using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportOutCutScene : MonoBehaviour
{
    public PlayerInput playerInput;

    public Animator canvas;
    public Animator mainCamera;
    public Animator mainCharacter;

    public MiniMapUI minimapUI;

    private bool playCutScene = false;

    private void Start()
    {
        playCutScene = false;
        minimapUI = FindFirstObjectByType<MiniMapUI>();
    }
    private void Update()
    {
        if(playCutScene == false)
        {
            mainCharacter.StopPlayback();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCutScene();
            minimapUI.MovePlayer(1, 0);
            PlayerSave.Instance.minimapPos.Item1 += 1;
        }
    }

    public void TriggerPlayerDissolveOut()
    {
        mainCharacter.SetBool("dissolveOut", true);
        mainCharacter.SetBool("dissolveIn", false);
        mainCharacter.SetBool("deathDissolveOut", false);

        //PlayerModelEventManager model = mainCharacter.gameObject.GetComponent<PlayerModelEventManager>();

        //StartCoroutine(model.limbs.DissolveOut(0.75f));
        //StartCoroutine(model.head.DissolveOut(0.75f));
        //StartCoroutine(model.watch.DissolveOut(0.75f));
        //StartCoroutine(model.body.DissolveOut(0.75f));
        //StartCoroutine(model.helmet.DissolveOut(0.75f));
    }
    public void TriggerPlayerDissolveIn()
    {
        mainCharacter.SetBool("dissolveOut", false);
        mainCharacter.SetBool("deathDissolveOut", false);
        mainCharacter.SetBool("dissolveIn", true);
    }

    private void StartCutScene()
    {
        playCutScene = true;

        FindFirstObjectByType<PlayerManager>().gameObject.GetComponent<Animator>().speed = 0f;

        canvas.SetBool("in", true);
        canvas.SetBool("out", false);

        mainCamera.enabled = true;

        mainCamera.SetBool("zoomIn", true);
        mainCamera.SetBool("zoomOut", false);
        mainCamera.gameObject.GetComponent<CameraBrain>().ZoomInTrigger();

        playerInput.enabled = false;
    }

    private void EndCutScene()
    {
        playerInput.enabled = true;

        canvas.SetBool("in", false);
        canvas.SetBool("out", true);

        mainCamera.SetBool("zoomIn", false);
        mainCamera.SetBool("zoomOut", true);
    }
}
