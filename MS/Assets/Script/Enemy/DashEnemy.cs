using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ThrowEnemy;

public class DashEnemy : EnemyBase
{
    public enum DASHENEMY_STATE
    {
        IDLE,
        MOVE,
        CHARGE,
        ATTACK,
        HURT
    }

    [Header("State")]
    public DASHENEMY_STATE state;
    public float idleTime;
    public float chargeTime;
    public float attackCooldownTime;
    public float hurtTime;


    //___仮想関数のOverride_________________________________________________________________________________________________________________________
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

            case DASHENEMY_STATE.CHARGE:
                Charge();
                break;

            case DASHENEMY_STATE.ATTACK:
                Attack();
                break;

            case DASHENEMY_STATE.HURT:
                Hurt();
                break;
        }
    }
    protected override void OnCollision(GameObject collided)
    {
        base.OnCollision(null);

        if(collided.gameObject == player)
        {
            if (state == DASHENEMY_STATE.ATTACK)
            {
                player.GetComponent<PlayerManager>().playerHP.Damage(attackPower);
            }
        }
    }
    public override void Damage(float value, bool killingBlow = false)
    {
        base.Damage(value, killingBlow);
        StartCoroutine(ChangeState(DASHENEMY_STATE.HURT, 0f));
    }

    public override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        stopRotation = false;   //回転再会
        stopMovement = false;

        RotateTowards(player.transform.position);

        StartCoroutine(ChangeState(DASHENEMY_STATE.MOVE, idleTime));
    }
    protected override void Move()
    {
        base.Move();

        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude <= attackDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(ChangeState(DASHENEMY_STATE.CHARGE, 0f));
        }
    }
    void Charge()
    {
        stopMovement = true;
        RotateTowards(player.transform.position);

        direction = player.transform.position - gameObject.transform.position;
        attacked = false;
        StartCoroutine(ChangeState(DASHENEMY_STATE.ATTACK, chargeTime));
    }
    void Attack()
    {
        if (attacked == false)
        {
            rigidbody.AddForce(direction.normalized * attackSpeed, ForceMode.Impulse);
            attacked = true;
        }

        if (rigidbody.velocity.magnitude <= 0.01f)
        {
            StartCoroutine(ChangeState(DASHENEMY_STATE.IDLE, attackCooldownTime));
        }
    }
    void Hurt()
    {
        StartCoroutine(ChangeState(DASHENEMY_STATE.IDLE, hurtTime));
    }
    //____________________________________________________________________________________________________________________________


    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(DASHENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;

        if (value == DASHENEMY_STATE.IDLE || value == DASHENEMY_STATE.CHARGE)
        {
            agent.gameObject.transform.position = transform.position;
        }
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________
}