using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThrowEnemy;

public class BossEnemy : MonoBehaviour
{
    public enum BOSSENEMY_STATE
    {
        IDLE = 0,
        ATTACK_01,
        ATTACK_02,
        ATTACK_03,
    }

    [Header("Health")]
    protected float hp = 100f;
    protected float maxHp = 100f;

    [Header("Movement")]
    private Vector3 velocity;

    [Header("References")]
    protected GameObject player;
    protected Rigidbody rigidbody;

    [Header("Attack")]
    public float attackPower;

    [Header("State Time")]
    public float time_AttackPhase_01;
    public float time_AttackPhase_02;
    public float time_AttackPhase_03;
    public BOSSENEMY_STATE state;
    public BOSSENEMY_STATE previousState;
    public BOSSENEMY_STATE nextState;
    public float nextPhaseTime;
    private bool currentPhaseOver;

    private bool countDownStart = false;
    private float countDown = 0f;

    private void Start()
    {
        countDownStart = false;
        currentPhaseOver = false;

        countDown = 0f;   
    }

    private void FixedUpdate()
    {
        if(countDownStart == true && currentPhaseOver == false)
        {
            if (countDown < nextPhaseTime) { countDown += Time.deltaTime; }
            else
            {
                countDown = 0f;
                countDownStart = false;
                currentPhaseOver = true;
            }
        }

        if(currentPhaseOver == true)
        {
            StartCoroutine(ChangeState(nextState, nextPhaseTime));
        }

        switch (state)
        {
            case BOSSENEMY_STATE.IDLE:
                Idle();
                break;

            case BOSSENEMY_STATE.ATTACK_01:
                AttackPattern1();
                break;

            case BOSSENEMY_STATE.ATTACK_02:
                AttackPattern2();
                break;

            case BOSSENEMY_STATE.ATTACK_03:
                AttackPattern3();
                break;
        }
    }

    public void Damage(float value, bool killingBlow = false)
    {

    }

    public void Death()
    {
        Destroy(gameObject);
    }

    private void CalculateNextAttack()
    {
        int current = (int)state;
        int previous = (int)previousState;
        int rand = current;
        while (rand == current || rand == previous)
        {
            rand = Random.Range(1, 4);
        }

        nextState = (BOSSENEMY_STATE)rand;

        if (nextState == BOSSENEMY_STATE.ATTACK_01) { nextPhaseTime = time_AttackPhase_01; }
        else if (nextState == BOSSENEMY_STATE.ATTACK_02) { nextPhaseTime = time_AttackPhase_02; }
        else { nextPhaseTime = time_AttackPhase_03; }
    }

    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        StartCoroutine(ChangeState(BOSSENEMY_STATE.ATTACK_01, time_AttackPhase_01));
    }

    void AttackPattern1()
    {

    }
    void AttackPattern2()
    {

    }
    void AttackPattern3()
    {

    }
    //____________________________________________________________________________________________________________________________

    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(BOSSENEMY_STATE value, float phaseTime)
    {
        yield return new WaitForSeconds(0f);
        previousState = state;
        state = value;

        currentPhaseOver = false;
        CalculateNextAttack();
        countDownStart = true;
    }
    IEnumerator PhaseTime(float time)
    {
        yield return new WaitForSeconds(time);
        currentPhaseOver = true;
    }
    //____________________________________________________________________________________________________________________________
}
