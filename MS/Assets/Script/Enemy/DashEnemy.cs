using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : EnemyBase
{
    public float idleTime;
    public float attackTime;
    public float hurtTime;
    public enum DASHENEMY_STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        HURT
    }

    public DASHENEMY_STATE state;
    protected override void Start()
    {
        base.Start();

        state = DASHENEMY_STATE.IDLE;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case DASHENEMY_STATE.IDLE:
                Idle();
                break;

            case DASHENEMY_STATE.MOVE:
                Move();
                break;

            case DASHENEMY_STATE.ATTACK:
                Attack();
                break;

            case DASHENEMY_STATE.HURT:
                Hurt();
                break;
        }
    }

    void Idle()
    {
        StartCoroutine(ChangeState(DASHENEMY_STATE.MOVE, idleTime));
    }
    void Move()
    {
        //ÉvÉåÅ[ÉÑÅ[Ç…å¸ÇØÇƒà⁄ìÆ
    }
    void Attack()
    {
        StartCoroutine(ChangeState(DASHENEMY_STATE.IDLE, attackTime));
    }
    void Hurt()
    {
        StartCoroutine(ChangeState(DASHENEMY_STATE.IDLE, hurtTime));
    }

    IEnumerator ChangeState(DASHENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;
    }
}