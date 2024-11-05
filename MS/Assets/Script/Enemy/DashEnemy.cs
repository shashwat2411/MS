using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : EnemyBase
{
    [Header("State Time")]
    public float idleTime;
    public float chargeTime;
    public float attackCooldownTime;
    public float hurtTime;

    [Header("Movement")]
    public float speed;
    public float attackDistance;
    private Vector3 direction;

    [Header("Attack")]
    public float attackPower;
    public float attakSpeed;
    private bool attacked;
    public enum DASHENEMY_STATE
    {
        IDLE,
        MOVE,
        CHARGE,
        ATTACK,
        HURT
    }

    public DASHENEMY_STATE state;

    protected override void Start()
    {
        base.Start();

        state = DASHENEMY_STATE.IDLE;
        attacked = false;
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
            player.GetComponent<MeshRenderer>().material.color = Color.red;
            //プレーヤーへのダメージ
        }
    }

    void Idle()
    {
        StartCoroutine(ChangeState(DASHENEMY_STATE.MOVE, idleTime));
    }
    void Move()
    {
        direction = player.transform.position - gameObject.transform.position;
        rigidbody.AddForce(direction.normalized * speed * Time.deltaTime, ForceMode.Acceleration);

        if(direction.magnitude <= attackDistance)
        {
            StartCoroutine(ChangeState(DASHENEMY_STATE.CHARGE, 0f));
        }
        //プレーヤーに向けて移動
    }
    void Charge()
    {
        direction = player.transform.position - gameObject.transform.position;
        attacked = false;
        //rigidbody.velocity = rigidbody.velocity * 0.8f;
        StartCoroutine(ChangeState(DASHENEMY_STATE.ATTACK, chargeTime));
    }
    void Attack()
    {
        if (attacked == false)
        {
            //rigidbody.velocity = rigidbody.velocity * 0.9f;
            rigidbody.AddForce(direction.normalized * attakSpeed * Time.deltaTime, ForceMode.Impulse);
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

    IEnumerator ChangeState(DASHENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;
    }
}