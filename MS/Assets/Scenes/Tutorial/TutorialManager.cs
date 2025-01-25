using System.Collections;
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
        AIMER_MOVEMENT,
        MP,
        FINISH,

        MAX
    }

    public TUTORIALSTATE state;

    [Header("References")]
    public PlayerManager player;
    public PlayerInput input;

    [Header("Inputs")]
    public InputActionReference closeRangeAttack;
    public InputActionReference dash;

    [Header("Movement")]
    public float movementDelayTime;
    private bool movementOnce = false;

    [Header("Dash")]
    public float dashDelayTime;

    [Header("Charge")]
    public float chargeDelayTime;
    private bool chargeOnce = false;
    private bool chargeTwice = false;

    [Header("Experience")]
    public MegaphoneEnemy enemy1;

    private void Start()
    {
        state = TUTORIALSTATE.MOVEMENT;

        enemy1.gameObject.SetActive(false);

        //boolean
        movementOnce = false;
        chargeOnce = false;
        chargeTwice = false;

        //Subscriptions
        closeRangeAttack.action.canceled += ChargeFinish;
        dash.action.started += DashInput;
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

            case TUTORIALSTATE.AIMER_MOVEMENT:
                AIMER_MOVEMENT();
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
    private void EXPERIENCE() { }
    private void LEVEL_UP() { }
    private void AIMER_MOVEMENT() { }
    private void MP() { }
    private void FINISH() { }

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
                }
            }

            if(chargeOnce == true && chargeTwice == false)
            {
                chargeTwice = true;
                StartCoroutine(NextState(chargeDelayTime, true, true));

                enemy1.gameObject.SetActive(true);
                enemy1.DissolveIn();
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

            if (videoShift == true) { video.ChangeScreen(); }
            if (textShift == true) { text.ChangeScreen(); }
        }

        state = (TUTORIALSTATE)current;
    }

}
