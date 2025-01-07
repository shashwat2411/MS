using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseAnimation : MonoBehaviour
{
    public bool IsPause;

    public bool AnimeStart, AnimeEnd;

    Animator AnimeCon;

    PlayerManager player;

    [Header("Audio")]
    public string pauseInSE;
    public string pauseOutSE;


    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();

        IsPause = false;

        AnimeStart = false;
        AnimeEnd = false;

        AnimeCon = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimeCon.SetBool("PauseStart", AnimeStart);
        AnimeCon.SetBool("PauseEnd", AnimeEnd);
    }

    public void AnimeReset()
    {
        AnimeStart = false;
        AnimeEnd = false;
    }

    public void PauseEnd()
    {
        AnimeStart = false;
        AnimeEnd = false;

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void PauseAnimationStart()
    {
        SoundManager.Instance.PlaySE(pauseInSE);
        AnimeStart = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PauseAnimationEnd()
    {
        SoundManager.Instance.PlaySE(pauseOutSE);
        AnimeEnd = true;
    }

    public void OnPause(InputAction.CallbackContext context)
    {

        if (!context.started) return;

       

        IsPause = !IsPause;

        if (IsPause)
        {
            
            Time.timeScale = 0;
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            PauseAnimationStart();
        }
        else
        {

            PauseAnimationEnd();
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            Time.timeScale = 1;
        }

        Debug.Log("Ispause" + IsPause);
    }
}
