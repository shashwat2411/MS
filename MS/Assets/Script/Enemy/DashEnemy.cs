using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            player.GetComponent<MeshRenderer>().material.color = Color.red;

            if(state != DASHENEMY_STATE.ATTACK)
            {
                healthBar.Damage(player.GetComponent<PlayerManager>().playerData.attack);
            }
            //プレーヤーへのダメージ
        }
    }
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        stopRotation = false;   //回転再会

        StartCoroutine(ChangeState(DASHENEMY_STATE.MOVE, idleTime));
    }
    void Move()
    {
        direction = player.transform.position - gameObject.transform.position;
        rigidbody.velocity = direction.normalized * speed * Time.deltaTime;

        if (direction.magnitude <= attackDistance)
        {
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(ChangeState(DASHENEMY_STATE.CHARGE, 0f));
        }
    }
    void Charge()
    {
        direction = player.transform.position - gameObject.transform.position;
        attacked = false;
        StartCoroutine(ChangeState(DASHENEMY_STATE.ATTACK, chargeTime));
    }
    void Attack()
    {
        stopRotation = true;    //回転を一時停止

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
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow.WithAlpha(0.3f);
        Gizmos.DrawSphere(transform.position, attackDistance);
    }
    //____________________________________________________________________________________________________________________________
}