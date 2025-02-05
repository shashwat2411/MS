using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public TutorialMonitorManager video;
    public TutorialTextManager text;
    public TutorialControllerManager controller;

    public enum TUTORIALSTATE
    {
        MOVEMENT = 0,
        DASH,
        CHARGE,
        EXPERIENCE,
        LEVEL_UP,
        MP,
        KILL,
        FINISH,

        MAX
    }

    public TUTORIALSTATE state;

    [Header("References")]
    public PlayerManager player;
    public PlayerInput input;
    public ScreenShatter shatterer;
    public Goal goal;

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
    public float enemyDissolveDuration;
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
    public float mpEnemyAppearDelayTime;
    public MegaphoneEnemy[] enemies;
    private bool mpOnce = false;
    private bool mpTwice = false;
    private bool mpThrice = false;

    [Header("Kill")]
    public float killDelayTime;

    [Header("Finish")]
    public float finishDelayTime;
    private bool finishOnce = false;

    private void Start()
    {
        state = TUTORIALSTATE.MOVEMENT;

        //boolean
        movementOnce = false;
        chargeOnce = false;
        chargeTwice = false;
        experienceOnce = false;
        levelUpOnce = false;
        mpOnce = false;
        mpTwice = false;
        mpThrice = false;
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
        killDelayTime += text.dissolveDuration;
        finishDelayTime += text.dissolveDuration;

        //Subscriptions
        dash.action.started += DashInput;
        closeRangeAttack.action.canceled += ChargeFinish;
        mpAttack.action.started += MpAttack;

        //enemy1.gameObject.SetActive(false);

        for (int i = 0; i < enemies.Length; i++)
        {
            //enemies[i].gameObject.SetActive(false);

            //enemies[i].megaphone.SetDissolveToMin();
            //enemies[i].body.SetDissolveToMin();
        }
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

            case TUTORIALSTATE.KILL:
                KILL();
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
            StartCoroutine(NextState(experienceDelayTime, true, true, true));
        }
    }
    private void LEVEL_UP()
    {
        if (levelUpOnce == false)
        {
            levelUpOnce = true;
            StartCoroutine(NextState(levelUpDelayTime, true, true, false));
            StartCoroutine(NextState(levelUpDelayTime + levelUpTwoDelayTime, true, true, true));
        }
    }
    private void MP()
    {

    }
    private void KILL()
    {
        if (mpTwice == false)
        {
            mpTwice = true;

            StartCoroutine(NextState(killDelayTime, true, true, false));
            for (int i = 0; i < enemies.Length; i++)
            {
                StartCoroutine(DissolveIn(enemies[i], mpEnemyAppearDelayTime + (1f * i), enemyDissolveDuration));
            }
        }

        int count = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if ((enemies[i] ? enemies[i].dead : true))
            {
                count++;
            }
        }
        if (count >= 5 && mpThrice == false)
        {
            mpThrice = true;
            StartCoroutine(NextState(finishDelayTime, true, true, true));
        }
    }
    private void FINISH() 
    {
        if(finishOnce == false)
        {
            finishOnce = true;
            goal.EnableGoal();
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
                    StartCoroutine(NextState(chargeDelayTime, true, true, false));
                    return;
                }
            }

            if (chargeOnce == true && chargeTwice == false)
            {
                chargeTwice = true;
                StartCoroutine(NextState(chargeDelayTime, true, true, true));

                StartCoroutine(DissolveIn(enemy1, enemyAppearDelayTime, enemyDissolveDuration));
            }
        }
    }
    void MpAttack(InputAction.CallbackContext context)
    {
        if (state == TUTORIALSTATE.MP)
        {
            if (mpOnce == false)
            {
                mpOnce = true;
                StartCoroutine(NextState(mpDelayTime, true, true, true));
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

        video.ChangeScreen();
        text.ChangeScreen();
        controller.ChangeScreen();

        state = (TUTORIALSTATE)current;
    }

    public IEnumerator DissolveIn(MegaphoneEnemy enemy, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        enemy.gameObject.SetActive(true);

        yield return null;

        enemy.megaphone.renderer.enabled = true;
        enemy.body.renderer.enabled = true;
        StartCoroutine(enemy.megaphone.DissolveIn(duration));
        StartCoroutine(enemy.body.DissolveIn(duration));

        yield return new WaitForSeconds(duration);

        enemy.GetComponent<BoxCollider>().enabled = true;
        enemy.enabled = true;
    }
}
