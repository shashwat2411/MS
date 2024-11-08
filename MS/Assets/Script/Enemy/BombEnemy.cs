using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BombEnemy : EnemyBase
{
    public enum BOMBENEMY_STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        HURT
    }

    [Header("State Time")]
    public BOMBENEMY_STATE state;
    public float idleTime;
    public float attackCooldownTime;
    public float hurtTime;

    [Header("Bomb")]
    public GameObject enemyBomb;
    public Transform spawnPoint;
    public float bombLifetime;
    private float cooldown = 0f;


    //___仮想関数のOverride_________________________________________________________________________________________________________________________
    protected override void Start()
    {
        base.Start();

        state = BOMBENEMY_STATE.IDLE;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case BOMBENEMY_STATE.IDLE:
                Idle();
                break;

            case BOMBENEMY_STATE.MOVE:
                Move();
                break;

            case BOMBENEMY_STATE.ATTACK:
                Attack();
                break;

            case BOMBENEMY_STATE.HURT:
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
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        StartCoroutine(ChangeState(BOMBENEMY_STATE.MOVE, idleTime));
    }
    void Move()
    {
        direction = player.transform.position - gameObject.transform.position;
        rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
        //プレーヤーに向けて移動

        if (direction.magnitude <= attackDistance)
        {
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(ChangeState(BOMBENEMY_STATE.ATTACK, idleTime));
        }
    }
    void Attack()
    {
        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude > attackDistance)
        {
            StartCoroutine(ChangeState(BOMBENEMY_STATE.IDLE, 0f));
        }

        if (attacked == false)
        {
            GameObject bomb = Instantiate(enemyBomb, spawnPoint.position, spawnPoint.rotation);
            bomb.GetComponent<EnemyBomb>().SetTarget(player.transform.position);

            StartCoroutine(DestroyBomb(bomb));
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
        StartCoroutine(ChangeState(BOMBENEMY_STATE.IDLE, hurtTime));
    }
    //____________________________________________________________________________________________________________________________


    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(BOMBENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;
    }
    IEnumerator DestroyBomb(GameObject bomb)
    {
        yield return new WaitForSeconds(bombLifetime);
        Destroy(bomb);
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow.WithAlpha(0.3f);
        Gizmos.DrawSphere(transform.position, attackDistance);
    }
    //____________________________________________________________________________________________________________________________


    //____________________________________________________________________________________________________________________________


    //___関数_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________
}