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
    public Transform area;

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

        computer.InstantiateMaterial();
        body.InstantiateMaterial();
        screen.InstantiateMaterial();

        float scale = transform.localScale.x;
        computer.SetMaxDissolveScale(scale);
        body.SetMaxDissolveScale(scale);
        screen.SetMaxDissolveScale(scale);

        //if (player.GetComponent<PlayerManager>().tutorial == true)
        {
            ResetEnemy();
        }
    }
    protected override void Start()
    {
        base.Start();

        animator.SetBool(_Attack, false);
        animator.SetBool(_Walk, false);

        state = DASHENEMY_STATE.IDLE;

        areaChecker = area;

        stopRotation = false;
        stopLooking = true;

        ScaleUp();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        RotateTowards(player.transform.position);

        if (attacked == true)
        {
            cooldown += Time.deltaTime;
            if (cooldown >= attackCooldownTime)
            {
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
        //base.OnCollision(null);

        //if(collided.gameObject == mainCharacter)
        //{
        //    if (state == DASHENEMY_STATE.ATTACK)
        //    {
        //        mainCharacter.GetComponent<PlayerManager>().playerHP.Damage(attackPower);
        //    }
        //}
    }
    public override void Damage(float value, bool killingBlow = false)
    {
        base.Damage(value, killingBlow);
    }

    public override void Death()
    {
        base.Death();

        GetComponent<Collider>().enabled = false;
        animator.speed = 0f;

        StartCoroutine(computer.DissolveOut(dissolveOutDuration));
        StartCoroutine(body.DissolveOut(dissolveOutDuration));
        StartCoroutine(screen.DissolveOut(dissolveOutDuration));

        Destroy(gameObject, dissolveOutDuration);
    }
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        //stopRotation = false;   //回転再会
        stopMovement = false;

        CheckState();
    }
    protected override void Move()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            base.Move();
        }

        if (dead == false)
        {
            direction = player.transform.position - areaChecker.position;
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
    }
    void Charge()
    {
        //RotateTowards(player.transform.position);
        stopMovement = true;

        direction = player.transform.position - areaChecker.position;
    }
    void Attack()
    {

    }

    public void ChangeToAttack()
    {
        attacked = false;
        StartCoroutine(ChangeState(DASHENEMY_STATE.ATTACK, 0f));
    }
    public void InitiateDash()
    {
        if (attacked == false)
        {
            attacked = true;
            stopMovement = false;

            dialogue.ActivateDialogue();
            Knockback(direction.normalized, attackSpeed);
        }
    }

    public void AttackOver()
    {
        attacked = true;
        cooldown = 0f;

        CheckState();
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
            direction = player.transform.position - areaChecker.position;
            if (direction.magnitude >= attackDistance)
            {
                animator.SetBool(_Attack, false);
                animator.SetBool(_Walk, true);
                StartCoroutine(ChangeState(DASHENEMY_STATE.MOVE, 0f));
            }
            else
            {
                animator.SetBool(_Attack, true);
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

        if (dead == false)
        {
            if (value == DASHENEMY_STATE.IDLE || value == DASHENEMY_STATE.CHARGE)
            {
                agent.gameObject.transform.position = transform.position;
            }
        }
    }

    public void ResetEnemy()
    {
        computer.renderer.enabled = false;
        body.renderer.enabled = false;
        screen.renderer.enabled = false;

        computer.SetDissolveToMin();
        body.SetDissolveToMin();
        screen.SetDissolveToMin();
    }
    public override IEnumerator DissolveIn(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);

        yield return null;

        computer.renderer.enabled = true;
        body.renderer.enabled = true;
        screen.renderer.enabled = true;

        StartCoroutine(computer.DissolveIn(duration));
        StartCoroutine(body.DissolveIn(duration));
        StartCoroutine(screen.DissolveIn(duration));

        yield return new WaitForSeconds(duration);

        GetComponent<BoxCollider>().enabled = true;
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    protected override void OnDrawGizmosSelected()
    {
        areaChecker = area;
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(areaChecker.position, attackDistance);
    }
    //____________________________________________________________________________________________________________________________
}