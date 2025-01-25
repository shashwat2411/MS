using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ThrowEnemy;

public class TutorialManager : MonoBehaviour
{
    public TutorialMonitorManager video;
    public TutorialTextManager text;

    public enum TUTORIALSTATE
    { 
        MOVEMENT = 0,
        CHARGE,
        EXPERIENCE,
        LEVEL_UP,
        CHARGE_LEVEL,
        AIMER_MOVEMENT,
        DASH,
        MP,
        FINISH,
        
        MAX
    }

    public TUTORIALSTATE state;

    [Header("References")]
    public PlayerManager player;
    public PlayerInput input;
    public InputActionReference closeRangeAttack;

    [Header("Movement")]
    public float movementDelayTime;
    private bool movementOnce = false;

    [Header("Charge")]
    public float chargeDelayTime;
    private bool chargeOnce = false;

    private void Start()
    {
        state = TUTORIALSTATE.MOVEMENT;

        //boolean
        movementOnce = false;
        chargeOnce = false;

        //Subscriptions
        closeRangeAttack.action.canceled += ChargeFinish;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case TUTORIALSTATE.MOVEMENT:
                MOVEMENT();
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

            case TUTORIALSTATE.CHARGE_LEVEL:
                CHARGE_LEVEL();
                break;

            case TUTORIALSTATE.AIMER_MOVEMENT:
                AIMER_MOVEMENT();
                break;

            case TUTORIALSTATE.DASH:
                DASH();
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
        if(movementOnce == false && (player.GetMovementInput().x > 0.5f || player.GetMovementInput().y > 0.5f))
        {
            movementOnce = true;
            StartCoroutine(NextState(movementDelayTime));
        }
    }
    private void CHARGE() { }
    private void EXPERIENCE() { }
    private void LEVEL_UP() { }
    private void CHARGE_LEVEL() { }
    private void AIMER_MOVEMENT() { }
    private void DASH() { }
    private void MP() { }
    private void FINISH() { }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(NextState(0f));
        }
    }

    private IEnumerator NextState(float delay)
    {
        yield return new WaitForSeconds(delay);

        int current = (int)state;
        
        current++;
        if (current >= (int)TUTORIALSTATE.MAX) { current = (int)TUTORIALSTATE.FINISH; }

        video.ChangeScreen();
        text.ChangeScreen();

        state = (TUTORIALSTATE)current;
    }

    void ChargeFinish(InputAction.CallbackContext context)
    {
        if (chargeOnce == false && (player.playerAttack.chargePhase == ChargePhase.High || player.playerAttack.chargePhase == ChargePhase.Max))
        {
            chargeOnce = true;
            StartCoroutine(NextState(chargeDelayTime));
        }
    }
}
