using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

        if (collided.gameObject == player)
        {
            player.GetComponent<MeshRenderer>().material.color = Color.red;
            healthBar.Damage(player.GetComponent<PlayerAttack>().collisionDamage);
            //プレーヤーへのダメージ
        }
    }
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        StartCoroutine(ChangeState(GUNENEMY_STATE.MOVE, idleTime));
    }
    void Move()
    {
        direction = player.transform.position - gameObject.transform.position;
        rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
        //プレーヤーに向けて移動

        if (direction.magnitude <= attackDistance)
        {
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(ChangeState(GUNENEMY_STATE.ATTACK, idleTime));
        }
    }
    void Attack()
    {
        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude > attackDistance)
        {
            StartCoroutine(ChangeState(GUNENEMY_STATE.IDLE, 0f));
        }

        if (attacked == false)
        {
            GameObject bullet = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = direction.normalized * attackSpeed * Time.deltaTime;
            StartCoroutine(DestroyBullet(bullet));
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
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);
        Destroy(bullet);
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