using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static DashEnemy;
using static ThrowEnemy;
using static UnityEditor.Progress;

public class GunEnemy : EnemyBase
{
    public enum GUNENEMY_STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        HURT
    }

    [Header("State Time")]
    public GUNENEMY_STATE state;
    public float idleTime;
    public float attackCooldownTime;
    public float hurtTime;

    [Header("Bullet")]
    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletLifetime;
    private float cooldown = 0f;


    //___仮想関数のOverride_________________________________________________________________________________________________________________________
    protected override void Start()
    {
        base.Start();

        state = GUNENEMY_STATE.IDLE;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case GUNENEMY_STATE.IDLE:
                Idle();
                break;

            case GUNENEMY_STATE.MOVE:
                Move();
                break;

            case GUNENEMY_STATE.ATTACK:
                Attack();
                break;

            case GUNENEMY_STATE.HURT:
                Hurt();
                break;
        }
    }
    protected override void OnCollision(GameObject collided)
    {
        base.OnCollision(null);

    }
    public override void Damage(float value, bool killingBlow = false)
    {
        base.Damage(value, killingBlow);
        StartCoroutine(ChangeState(GUNENEMY_STATE.HURT, 0f));
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

        StartCoroutine(ChangeState(GUNENEMY_STATE.MOVE, idleTime));
    }
    protected override void Move()
    {
        base.Move();

        //direction = player.transform.position - gameObject.transform.position;
        //rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
        //プレーヤーに向けて移動
        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude < attackDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(ChangeState(GUNENEMY_STATE.ATTACK, idleTime));
        }
    }
    void Attack()
    {
        RotateTowards(player.transform.position);

        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude >= attackDistance)
        {
            StartCoroutine(ChangeState(GUNENEMY_STATE.IDLE, 0f));
        }

        if (attacked == false)
        {
            ThrowableEnemyObject bullet = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation).GetComponent<ThrowableEnemyObject>();

            //bullet.SetTarget(player.transform.position);
            //bullet.SetOwner(gameObject);
            //bullet.SetMaxLifetime(itemLifetime);

            attacked = true;
        }
        else
        {
            cooldown += Time.deltaTime;
            if(cooldown >= attackCooldownTime)
            {
                cooldown = 0f;
                attacked = false;
            }
        }
    }
    void Hurt()
    {
        StartCoroutine(ChangeState(GUNENEMY_STATE.IDLE, hurtTime));
    }
    //____________________________________________________________________________________________________________________________


    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(GUNENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;

        if (value == GUNENEMY_STATE.IDLE)
        {
            agent.gameObject.transform.position = transform.position;
        }
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);
        Destroy(bullet);
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________
}