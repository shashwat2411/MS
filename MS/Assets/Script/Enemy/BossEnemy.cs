using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThrowEnemy;
using static UnityEngine.Rendering.DebugUI;

public class BossEnemy : MonoBehaviour
{
    public enum BOSSENEMY_STATE
    {
        IDLE = 0,
        SCREAM,
        SLAP,
        SMASH,
        SUMMONLIGHTNING,

        MAX
    }

    [Header("Health")]
    protected float hp = 100f;
    protected float maxHp = 100f;

    [Header("References")]
    protected GameObject player;
    protected HealthBar healthBar;
    private GameObject canvas;

    [Header("Attack")]
    public float slapAttackPower;
    public float smashAttackPower;
    public float lightningAttackPower;

    [Header("State Time")]
    public float screamTime;
    public float slapTime;
    public float smashTime;
    public float lightningTime;
    public BOSSENEMY_STATE currentState;
    public BOSSENEMY_STATE previousState;
    public BOSSENEMY_STATE nextState;

    private float nextPhaseTime;
    private float countDown = 0f;

    private bool currentPhaseOver;
    private bool countDownStart = false;

    //Animation
    public Animator animator;

    //Hash Map
    protected static int _Scream = Animator.StringToHash("_Scream");
    protected static int _Slap = Animator.StringToHash("_Slap");
    protected static int _Smash = Animator.StringToHash("_Smash");
    protected static int _SummonLightning = Animator.StringToHash("_SummonLightning");

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        healthBar = canvas.GetComponentInChildren<HealthBar>();

        countDownStart = false;
        currentPhaseOver = false;

        countDown = 0f;

        ReturnToIdle();
        StartCoroutine(ChangeState(BOSSENEMY_STATE.SCREAM));
    }

    private void FixedUpdate()
    {
        if(countDownStart == true && currentPhaseOver == false)
        {
            if (countDown < nextPhaseTime) { countDown += Time.deltaTime; }
            else if (IsCurrentAnimationOver())
            {
                countDown = 0f;
                countDownStart = false;
                currentPhaseOver = true;
            }
        }

        if(currentPhaseOver == true)
        {
            StartCoroutine(ChangeState(nextState));
        }

        switch (currentState)
        {
            case BOSSENEMY_STATE.IDLE:
                Idle();
                break;

            case BOSSENEMY_STATE.SCREAM:
                Scream();
                break;

            case BOSSENEMY_STATE.SLAP:
                Slap();
                break;

            case BOSSENEMY_STATE.SMASH:
                Smash();
                break;

            case BOSSENEMY_STATE.SUMMONLIGHTNING:
                SummonLightning();
                break;
        }
    }
    virtual protected void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    //HP Related-----------------------------------------
    public void Damage(float value, bool killingBlow = false)
    {
        healthBar.Damage(value, killingBlow);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    //----------------------------------------------------

    public void ReturnToIdle()
    {
        previousState = currentState;
        currentState = BOSSENEMY_STATE.IDLE;

        currentPhaseOver = false;
        CalculateNextAttack();
        countDownStart = true;

        animator.SetBool(_Scream, false);
        animator.SetBool(_Slap, false);
        animator.SetBool(_Smash, false);
        animator.SetBool(_SummonLightning, false);
    }
    private bool IsCurrentAnimationOver()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) { return true; }

        return false;
    }

    private void CalculateNextAttack()
    {
        int current = (int)currentState;
        int previous = (int)previousState;
        int rand = current;
        while (rand == current || rand == previous)
        {
            rand = Random.Range(1, (int)BOSSENEMY_STATE.MAX);
        }

        nextState = (BOSSENEMY_STATE)rand;

        if (nextState == BOSSENEMY_STATE.SCREAM) { nextPhaseTime = screamTime; }
        else if (nextState == BOSSENEMY_STATE.SLAP) { nextPhaseTime = slapTime; }
        else if (nextState == BOSSENEMY_STATE.SMASH) { nextPhaseTime = smashTime; }
        else if (nextState == BOSSENEMY_STATE.SUMMONLIGHTNING) { nextPhaseTime = lightningTime; }
        else { nextPhaseTime = 0f; }
    }

    //____ステート________________________________________________________________________________________________________________________
    void Idle() { }
    void Scream() { animator.SetBool(_Scream, true); }
    void Slap() { animator.SetBool(_Slap, true); }
    void Smash() { animator.SetBool(_Smash, true); }
    void SummonLightning() { animator.SetBool(_SummonLightning, true); }
    //____________________________________________________________________________________________________________________________

    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(BOSSENEMY_STATE value)
    {
        yield return new WaitForSeconds(0f);
        previousState = currentState;
        currentState = value;

        currentPhaseOver = false;
        CalculateNextAttack();
        countDownStart = true;
    }
    //____________________________________________________________________________________________________________________________

    private void OnCollisionEnter(Collision collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
        if(player)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slap"))
            {
                player.playerHP.Damage(slapAttackPower);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Smash"))
            {
                player.playerHP.Damage(smashAttackPower);
            }
        }
    }
}
