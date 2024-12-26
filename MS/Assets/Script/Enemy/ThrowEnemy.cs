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
    [SerializeField] protected float cooldown = 0f;
    protected ThrowableEnemyObject itemReference;

    //Hash Map
    protected static int _Attack = Animator.StringToHash("_Attack");
    protected static int _Walk = Animator.StringToHash("_Walk");

    //___仮想関数のOverride_________________________________________________________________________________________________________________________
    protected override void Start()
    {
        base.Start();

        animator.SetBool(_Attack, false);
        animator.SetBool(_Walk, false);

        state = THROWENEMY_STATE.IDLE;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (attacked == true)
        {
            cooldown += Time.deltaTime;
            if (cooldown >= attackCooldownTime)
            {
                cooldown = 0f;
                attacked = false;
            }
        }

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
    }

    public override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    protected void Idle()
    {
        stopRotation = false;   //回転再会
        stopMovement = false;

        CheckState();
    }
    protected override void Move()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            base.Move();
        }

        direction = player.transform.position - gameObject.transform.position;
        if (direction.magnitude < attackDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            if (attacked == false)
            {
                animator.SetBool(_Attack, true);
                animator.SetBool(_Walk, false);
                RotateTowards(player.transform.position);
                StartCoroutine(ChangeState(THROWENEMY_STATE.ATTACK, 0f));
            }
            else
            {
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, false);
                StartCoroutine(ChangeState(THROWENEMY_STATE.IDLE, 0f));
            }
        }
    }
    protected void Attack()
    {

    }
    protected void Hurt()
    {
        StartCoroutine(ChangeState(THROWENEMY_STATE.IDLE, hurtTime));
    }

    public void AttackInstantiate()
    {
        dialogue.ActivateDialogue();
        itemReference = Instantiate(enemyItem, spawnPoint.position, spawnPoint.rotation).GetComponent<ThrowableEnemyObject>();

        itemReference.SetTarget(player.transform.position);
        itemReference.SetOwner(gameObject);
        itemReference.SetDamage(attackPower);
        itemReference.SetMaxLifetime(itemLifetime);
    }

    protected void CheckState()
    {
        if(attacked == true)
        {
            animator.SetBool(_Attack, false);
            animator.SetBool(_Walk, false);
            StartCoroutine(ChangeState(THROWENEMY_STATE.IDLE, 0f));
        }
        else
        {
            direction = player.transform.position - gameObject.transform.position;
            if (direction.magnitude >= attackDistance)
            {
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, true);
                StartCoroutine(ChangeState(THROWENEMY_STATE.MOVE, 0f));
            }
            else
            {
                animator.SetBool(_Attack, true);
                animator.SetBool(_Walk, false);
                RotateTowards(player.transform.position);
                StartCoroutine(ChangeState(THROWENEMY_STATE.ATTACK, 0f));
            }
        }
    }

    public void AttackOver()
    {
        attacked = true;
        cooldown = 0f;


        CheckState();
    }
    //____________________________________________________________________________________________________________________________


    //___Coroutines_________________________________________________________________________________________________________________________
    protected IEnumerator ChangeState(THROWENEMY_STATE value, float delayTime)
    {
        if (state == value) { yield return null; }

        yield return new WaitForSeconds(delayTime);
        state = value;

        if (value == THROWENEMY_STATE.IDLE)
        {
            agent.gameObject.transform.position = transform.position;
        }
    }

    protected IEnumerator SpawnItem(float interval)
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