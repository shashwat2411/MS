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
    [SerializeField] protected float cooldown = 0f;

    [Header("Material")]
    public EnemyMaterial computer;
    public EnemyMaterial body;
    public EnemyMaterial screen;

    //Hash Map
    protected static int _Attack = Animator.StringToHash("_Attack");
    protected static int _Walk = Animator.StringToHash("_Walk");

    //___仮想関数のOverride_________________________________________________________________________________________________________________________
    protected override void ScaleUp()
    {
        base.ScaleUp();

        float scale = transform.localScale.x;
        computer.SetMaxDissolveScale(scale);
        body.SetMaxDissolveScale(scale);
        screen.SetMaxDissolveScale(scale);
    }
    protected override void Start()
    {
        base.Start();

        computer.InstantiateMaterial();
        body.InstantiateMaterial();
        screen.InstantiateMaterial();

        animator.SetBool(_Attack, false);
        animator.SetBool(_Walk, false);

        state = DASHENEMY_STATE.IDLE;

        ScaleUp();
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
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, false);
                RotateTowards(player.transform.position);
                StartCoroutine(ChangeState(DASHENEMY_STATE.CHARGE, 0f));
            }
            else
            {
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, false);
                StartCoroutine(ChangeState(DASHENEMY_STATE.IDLE, 0f));
            }
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
            dialogue.ActivateDialogue();
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
    protected void CheckState()
    {
        if (attacked == true)
        {
            animator.SetBool(_Attack, false);
            animator.SetBool(_Walk, false);
            StartCoroutine(ChangeState(DASHENEMY_STATE.IDLE, 0f));
        }
        else
        {
            direction = player.transform.position - gameObject.transform.position;
            if (direction.magnitude >= attackDistance)
            {
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, true);
                StartCoroutine(ChangeState(DASHENEMY_STATE.MOVE, 0f));
            }
            else
            {
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, false);
                RotateTowards(player.transform.position);
                StartCoroutine(ChangeState(DASHENEMY_STATE.CHARGE, 0f));
            }
        }
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

        if(value  == DASHENEMY_STATE.ATTACK)
        {
            animator.SetBool(_Attack, true);
            animator.SetBool(_Walk, false);
        }
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________
}