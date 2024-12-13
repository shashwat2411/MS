using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GunEnemy;

public class KamikazeEnemy : EnemyBase
{
    public enum KAMIKAZEENEMY_STATE
    {
        IDLE,
        MOVE,
        HURT,
    }

    [Header("State Time")]
    public KAMIKAZEENEMY_STATE state;
    public float idleTime;
    public float hurtTime;


    //___���z�֐���Override_________________________________________________________________________________________________________________________
    protected override void Start()
    {
        base.Start();

        state = KAMIKAZEENEMY_STATE.IDLE;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case KAMIKAZEENEMY_STATE.IDLE:
                Idle();
                break;

            case KAMIKAZEENEMY_STATE.MOVE:
                Move();
                break;

            case KAMIKAZEENEMY_STATE.HURT:
                Hurt();
                break;
        }
    }
    protected override void OnCollision(GameObject collided)
    {
        base.OnCollision(null);

        if (collided.gameObject == player)
        {
            healthBar.Damage(healthBar.health + 1f);

            //�v���[���[�ւ̃_���[�W
            player.GetComponent<PlayerManager>().playerHP.Damage(attackPower);
        }
    }

    public override void Damage(float value)
    {
        base.Damage(value);
        StartCoroutine(ChangeState(KAMIKAZEENEMY_STATE.HURT, 0f));
    }

    public override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
    //____________________________________________________________________________________________________________________________


    //____�X�e�[�g________________________________________________________________________________________________________________________
    void Idle()
    {
        stopRotation = false;   //��]�ĉ�
        stopMovement = false;

        StartCoroutine(ChangeState(KAMIKAZEENEMY_STATE.MOVE, idleTime));
    }
    void Hurt()
    {
        StartCoroutine(ChangeState(KAMIKAZEENEMY_STATE.IDLE, hurtTime));
    }
    //____________________________________________________________________________________________________________________________


    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(KAMIKAZEENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;

        if (value == KAMIKAZEENEMY_STATE.IDLE)
        {
            agent.gameObject.transform.position = transform.position;
        }
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________


    //___�֐�_________________________________________________________________________________________________________________________
    private void OnDestroy()
    {
        //���j����
    }
    //____________________________________________________________________________________________________________________________
}