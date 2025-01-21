using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBase;
using static UnityEngine.Rendering.DebugUI;

public class BossEnemy : EnemyBase
{
    public enum BOSSENEMY_STATE
    {
        IDLE = 0,
        SCREAM,
        SLAP,
        SMASH,
        SMASHFAR,
        SUMMONLIGHTNING,

        MAX
    }

    [Header("References")]
    public HealthBar bossHealthBar;

    [Header("Attack")]
    public float slapAttackPower;
    public float smashAttackPower;
    public float lightningAttackPower;
    public bool disableColliders;
    public float speed = 1f;
    public float maxSpeed = 1.5f;

    [Header("State Time")]
    public float screamTime;
    public float slapTime;
    public float smashTime;
    public float lightningTime;
    public float phaseChangeThreshold = 0.25f;
    public BOSSENEMY_STATE currentState;
    public BOSSENEMY_STATE previousState;
    public BOSSENEMY_STATE nextState;

    private float nextPhaseTime;
    private float countDown = 0f;

    private bool currentPhaseOver;
    private bool countDownStart = false;


    [Header("Material")]
    public EnemyMaterial eyeball;
    public EnemyMaterial gums;
    public EnemyMaterial teeth;
    public EnemyMaterial hands;
    public EnemyMaterial tongue;
    public Animator visualAimer;

    //Hash Map
    protected static int _Scream = Animator.StringToHash("_Scream");
    protected static int _Slap = Animator.StringToHash("_Slap");
    protected static int _Smash = Animator.StringToHash("_Smash");
    protected static int _SmashFar = Animator.StringToHash("_SmashFar");
    protected static int _SummonLightning = Animator.StringToHash("_SummonLightning");
    protected static int _Speed = Animator.StringToHash("_Speed");

    protected override void Start()
    {
        healthBar = bossHealthBar;

        disableColliders = false;
        countDownStart = false;
        currentPhaseOver = false;

        countDown = 0f;

        ReturnToIdle();
        StartCoroutine(ChangeState(BOSSENEMY_STATE.SCREAM));

        healthBar.Stimulate();
        healthBar.disappear = false;

        eyeball.InstantiateMaterial();
        gums.InstantiateMaterial();
        teeth.InstantiateMaterial();
        hands.InstantiateMaterial();
        tongue.InstantiateMaterial();
    }

    protected override void FixedUpdate()
    { 
        float percentage = 1f - (bossHealthBar.health / bossHealthBar.maxHealth);
        speed = Mathf.Lerp(1f, maxSpeed, percentage);

        animator.SetFloat(_Speed, speed);
        visualAimer.SetFloat(_Speed, speed);

        if (dead == false)
        {
            if (countDownStart == true && currentPhaseOver == false)
            {
                if (countDown < nextPhaseTime) { countDown += Time.deltaTime * speed; }
                else if (IsCurrentAnimationOver())
                {
                    countDown = 0f;
                    countDownStart = false;
                    currentPhaseOver = true;
                }
            }

            if (currentPhaseOver == true)
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

                case BOSSENEMY_STATE.SMASHFAR:
                    SmashFar();
                    break;

                case BOSSENEMY_STATE.SUMMONLIGHTNING:
                    SummonLightning();
                    break;
            }
        }
    }

    public override void Death()
    {
        dead = true;

        
        animator.StopPlayback();

        speed = 1f;

        animator.SetBool(_Scream, true);
        animator.SetBool(_Slap, false);
        animator.SetBool(_Smash, false);
        animator.SetBool(_SmashFar, false);
        animator.SetBool(_SummonLightning, false);
        animator.Play("Scream");

        StartCoroutine(eyeball.DissolveOut(2f));
        StartCoroutine(gums.DissolveOut(2f));
        StartCoroutine(teeth.DissolveOut(2f));
        StartCoroutine(hands.DissolveOut(2f));
        StartCoroutine(tongue.DissolveOut(2f));

        //Destroy(gameObject, 2.2f);
    }

    public IEnumerator DeathCall(float delay)
    {
        yield return new WaitForSeconds(delay);

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
        disableColliders = false;

        animator.SetBool(_Scream, false);
        animator.SetBool(_Slap, false);
        animator.SetBool(_Smash, false);
        animator.SetBool(_SmashFar, false);
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
        else if (nextState == BOSSENEMY_STATE.SMASHFAR) { nextPhaseTime = smashTime; }
        else if (nextState == BOSSENEMY_STATE.SUMMONLIGHTNING) { nextPhaseTime = lightningTime; }
        else { nextPhaseTime = 0f; }
    }

    //____ステート________________________________________________________________________________________________________________________
    void Idle() { }
    void Scream() { animator.SetBool(_Scream, true); }
    void Slap() { animator.SetBool(_Slap, true); }
    void Smash() { animator.SetBool(_Smash, true); }
    void SmashFar() { animator.SetBool(_SmashFar, true); }
    void SummonLightning() { animator.SetBool(_SummonLightning, true); }
    //____________________________________________________________________________________________________________________________

    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(BOSSENEMY_STATE value)
    {
        yield return new WaitForSeconds(0f);
        previousState = currentState;
        currentState = value;

        currentPhaseOver = false;
        //CalculateNextAttack();
        countDownStart = true;
    }
    //____________________________________________________________________________________________________________________________

    protected override void OnCollisionEnter(Collision collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
        if(player)
        {
            if (disableColliders == false)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slap"))
                {
                    player.playerHP.Damage(slapAttackPower);
                    disableColliders = true;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Smash") || animator.GetCurrentAnimatorStateInfo(0).IsName("SmashFar"))
                {
                    player.playerHP.Damage(smashAttackPower);
                    disableColliders = true;
                }
            }
        }
    }
}
