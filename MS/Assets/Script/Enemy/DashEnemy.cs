using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : EnemyBase
{
    public enum DASHENEMY_STATE
    {
        IDLE,
        WALK,
        ATTACK,
        DAMAGE
    }

    public DASHENEMY_STATE state;
    protected override void Start()
    {
        base.Start();

        state = DASHENEMY_STATE.IDLE;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case DASHENEMY_STATE.IDLE:
                Idle();
                break;

            case DASHENEMY_STATE.WALK:
                Walk();
                break;

            case DASHENEMY_STATE.ATTACK:
                Attack();
                break;

            case DASHENEMY_STATE.DAMAGE:
                Damage();
                break;
        }
    }

    void Idle()
    {

    }
    void Walk()
    {

    }
    void Attack()
    {

    }
    void Damage()
    {

    }
}