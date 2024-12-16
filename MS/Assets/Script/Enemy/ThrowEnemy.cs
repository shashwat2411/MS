using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowEnemy : EnemyBase
{
    public enum THROWENEMY_STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        HURT
    }

    [Header("State Time")]
    public THROWENEMY_STATE state;
    public float idleTime;
    public float attackCooldownTime;
    public float hurtTime;

    [Header("Item")]
    public GameObject enemyItem;
    public Transform spawnPoint;
    public float itemLifetime;
    public int numOfItems = 1;
    public float spawnInterval = 0f;
    private float cooldown = 0f;


    //___仮想関数のOverride_________________________________________________________________________________________________________________________
    protected override void Start()
    {
        base.Start();

        state = THROWENEMY_STATE.IDLE;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case THROWENEMY_STATE.IDLE:
                Idle();
                break;

            case THROWENEMY_STATE.MOVE:
                Move();
                break;

            case THROWENEMY_STATE.ATTACK:
                Attack();
                break;

            case THROWENEMY_STATE.HURT:
                Hurt();
                break;
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {

    }

    public override void Damage(float value, bool killingBlow = false)
    {
        base.Damage(value, killingBlow);
        StartCoroutine(ChangeState(THROWENEMY_STATE.HURT, 0f));
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

        StartCoroutine(ChangeState(THROWENEMY_STATE.MOVE, idleTime));
    }
    protected override void Move()
    {
        base.Move();
        //direction = player.transform.position - gameObject.transform.position;
        //rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
        //プレーヤーに向けて移動
       // Debug.Log("Distance : " + agent.remainingDistance);

        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude < attackDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(ChangeState(THROWENEMY_STATE.ATTACK, idleTime));
        }
    }
    void Attack()
    {
        RotateTowards(player.transform.position);

        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude >= attackDistance)
        {
            StartCoroutine(ChangeState(THROWENEMY_STATE.IDLE, 0f));
        }

        if (attacked == false)
        {
            for (int i = 0; i < numOfItems; i++)
            {
                StartCoroutine(SpawnItem(spawnInterval * (float)i));
            }
            //ThrowableEnemyObject item = Instantiate(enemyItem, spawnPoint.position, spawnPoint.rotation).GetComponent<ThrowableEnemyObject>();

            //item.SetTarget(player.transform.position);
            //item.SetOwner(gameObject);
            //item.SetDamage(attackPower);
            //item.SetMaxLifetime(itemLifetime);

            attacked = true;
        }
        else
        {
            cooldown += Time.deltaTime;
            if (cooldown >= attackCooldownTime)
            {
                cooldown = 0f;
                attacked = false;
            }
        }
    }
    void Hurt()
    {
        StartCoroutine(ChangeState(THROWENEMY_STATE.IDLE, hurtTime));
    }
    //____________________________________________________________________________________________________________________________


    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(THROWENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;

        if (value == THROWENEMY_STATE.IDLE)
        {
            agent.gameObject.transform.position = transform.position;
        }
    }

    IEnumerator SpawnItem(float interval)
    {
        yield return new WaitForSeconds(interval);

        ThrowableEnemyObject item = Instantiate(enemyItem, spawnPoint.position, spawnPoint.rotation).GetComponent<ThrowableEnemyObject>();

        item.SetTarget(player.transform.position);
        item.SetOwner(gameObject);
        item.SetDamage(attackPower);
        item.SetMaxLifetime(itemLifetime);
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________


    //___関数_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________
}