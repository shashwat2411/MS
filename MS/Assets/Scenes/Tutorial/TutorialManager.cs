using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public TutorialMonitorManager video;
    public TutorialTextManager text;

    public enum TUTORIALSTATE
    {
        MOVEMENT = 0,
        DASH,
        CHARGE,
        EXPERIENCE,
        LEVEL_UP,
        MP,
        FINISH,

        MAX
    }

    public TUTORIALSTATE state;

    [Header("References")]
    public PlayerManager player;
    public PlayerInput input;
    public ScreenShatter shatterer;

    [Header("Inputs")]
    public InputActionReference dash;
    public InputActionReference closeRangeAttack;
    public InputActionReference mpAttack;

    [Header("Movement")]
    public float movementDelayTime;
    private bool movementOnce = false;

    [Header("Dash")]
    public float dashDelayTime;

    [Header("Charge")]
    public float chargeDelayTime;
    public float enemyAppearDelayTime;
    private bool chargeOnce = false;
    private bool chargeTwice = false;

    [Header("Experience")]
    public MegaphoneEnemy enemy1;
    public float experienceDelayTime;
    private bool experienceOnce = false;

    [Header("Level Up")]
    public float levelUpDelayTime;
    public float levelUpTwoDelayTime;
    private bool levelUpOnce = false;

    [Header("MP")]
    public float mpDelayTime;
    public MegaphoneEnemy[] enemies;
    private bool mpOnce = false;

    [Header("Finish")]
    public float finishDelayTime;
    private bool finishOnce = false;

    private void Start()
    {
        state = TUTORIALSTATE.MOVEMENT;

        enemy1.gameObject.SetActive(false);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }

        //boolean
        movementOnce = false;
        chargeOnce = false;
        chargeTwice = false;
        experienceOnce = false;
        levelUpOnce = false;
        mpOnce = false;
        finishOnce = false;

        //delays
        movementDelayTime += text.dissolveDuration;
        dashDelayTime += text.dissolveDuration;
        chargeDelayTime += text.dissolveDuration;
        enemyAppearDelayTime += text.dissolveDuration;
        experienceDelayTime += text.dissolveDuration;
        levelUpDelayTime += text.dissolveDuration;
        levelUpTwoDelayTime += text.dissolveDuration;
        mpDelayTime += text.dissolveDuration;
        finishDelayTime += text.dissolveDuration;

        //Subscriptions
        dash.action.started += DashInput;
        closeRangeAttack.action.canceled += ChargeFinish;
        mpAttack.action.started += MpAttack;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case TUTORIALSTATE.MOVEMENT:
                MOVEMENT();
                break;

            case TUTORIALSTATE.DASH:
                DASH();
                break;

            case TUTORIALSTATE.CHARGE:
                CHARGE();
                break;

            case TUTORIALSTATE.EXPERIENCE:
                EXPERIENCE();
                break;

            case TUTORIALSTATE.LEVEL_UP:
                LEVEL_UP();
                break;

            case TUTORIALSTATE.MP:
                MP();
                break;

            case TUTORIALSTATE.FINISH:
                FINISH();
                break;
        }

    }

    private void MOVEMENT()
    {
        if (movementOnce == false && (player.GetMovementInput().x > 0.5f || player.GetMovementInput().y > 0.5f))
        {
            movementOnce = true;
            StartCoroutine(NextState(movementDelayTime));
        }
    }
    private void DASH() { }
    private void CHARGE() { }
    private void EXPERIENCE()
    {
        if ((enemy1 ? enemy1.dead : true) && experienceOnce == false)
        {
            experienceOnce = true;
            StartCoroutine(NextState(experienceDelayTime, false, true, true));
        }
    }
    private void LEVEL_UP()
    {
        if (levelUpOnce == false)
        {
            levelUpOnce = true;
            StartCoroutine(NextState(levelUpDelayTime, false, true, false));
            StartCoroutine(NextState(levelUpDelayTime + levelUpTwoDelayTime, true, true, true));

            for (int i = 0; i < enemies.Length; i++)
            {
                StartCoroutine(enemies[i].DissolveIn(levelUpDelayTime + levelUpTwoDelayTime + enemyAppearDelayTime - text.dissolveDuration * 2f));
            }
        }
    }
    private void MP() { }
    private void FINISH() 
    {
        if(finishOnce == false)
        {
            finishOnce = true;
            StartCoroutine(shatterer.ShatterScreenInitate());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(NextState(0f));
        }
    }

    void DashInput(InputAction.CallbackContext context)
    {
        if (state == TUTORIALSTATE.DASH)
        {
            StartCoroutine(NextState(dashDelayTime));
        }
    }
    void ChargeFinish(InputAction.CallbackContext context)
    {
        if (state == TUTORIALSTATE.CHARGE)
        {
            if ((player.playerAttack.chargePhase == ChargePhase.High || player.playerAttack.chargePhase == ChargePhase.Max))
            {
                if (chargeOnce == false)
                {
                    chargeOnce = true;
                    StartCoroutine(NextState(chargeDelayTime, false, true, false));
                    return;
                }
            }

            if (chargeOnce == true && chargeTwice == false)
            {
                chargeTwice = true;
                StartCoroutine(NextState(chargeDelayTime, true, true));

                StartCoroutine(enemy1.DissolveIn(enemyAppearDelayTime));
            }
        }
    }
    void MpAttack(InputAction.CallbackContext context)
    {
        if (state == TUTORIALSTATE.MP)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if ((enemies[i] ? enemies[i].dead : true) && mpOnce == false)
                {
                    mpOnce = true;
                    StartCoroutine(NextState(mpDelayTime, true, true, true));
                }
            }
        }
    }


    //Coroutines--------------------------------------------------------------------------------------------------------------------------
    private IEnumerator NextState(float delay, bool videoShift = true, bool textShift = true, bool changeState = true)
    {
        yield return new WaitForSeconds(delay);

        int current = (int)state;

        if (changeState == true)
        {
            current++;
            if (current >= (int)TUTORIALSTATE.MAX) { current = (int)TUTORIALSTATE.FINISH; }
        }

        if (videoShift == true)
        {
            video.ChangeScreen();
        }
        if (textShift == true)
        {
            text.ChangeScreen();
        }

        state = (TUTORIALSTATE)current;
    }
}
