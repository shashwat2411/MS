using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.XR;
using static GunEnemy;
using static ThrowEnemy;

public class KamikazeEnemy : EnemyBase
{
    public enum KAMIKAZEENEMY_STATE
    {
        IDLE,
        MOVE
    }

    [Header("State Time")]
    public KAMIKAZEENEMY_STATE state;
    public float idleTime;
    public float hurtTime;

    [Header("Material")]
    public EnemyMaterial body;

    //___���z�֐���Override_________________________________________________________________________________________________________________________
    protected override void ScaleUp()
    {
        base.ScaleUp();

        body.InstantiateMaterial();

        float scale = transform.localScale.x;
        body.SetMaxDissolveScale(scale);
    }
    protected override void Start()
    {
        base.Start();

        ScaleUp();

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