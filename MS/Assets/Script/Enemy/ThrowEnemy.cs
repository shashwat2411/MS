using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GunEnemy;
using static KamikazeEnemy;

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
    public float bombLifetime;
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
    protected override void OnCollision(GameObject collided)
    {
        base.OnCollision(null);

        if (collided.gameObject == player)
        {
            player.GetComponent<MeshRenderer>().material.color = Color.red;
            healthBar.Damage(player.GetComponent<PlayerManager>().playerData.attack);
            //プレーヤーへのダメージ
        }
    }
    public override void Damage(float value)
    {
        base.Damage(value);
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
        Debug.Log("Distance : " + agent.remainingDistance);

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
            GameObject item = Instantiate(enemyItem, spawnPoint.position, spawnPoint.rotation);
            item.GetComponent<ThrowableEnemyObject>().SetTarget(player.transform.position);
            item.GetComponent<ThrowableEnemyObject>().SetOwner(gameObject);

            StartCoroutine(DestroyBomb(item));
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
    IEnumerator DestroyBomb(GameObject bomb)
    {
        yield return new WaitForSeconds(bombLifetime);
        Destroy(bomb);
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________


    //___関数_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________
}