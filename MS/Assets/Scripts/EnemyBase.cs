using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    float hp = 100f;
    float maxHp = 100f;



    private HealthBar healthBar;

    [SerializeField]
    private HighLightFlash highLightFlash;

    private Coroutine hurtFlashCoroutine;

    private FSM fsm;
    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        highLightFlash = GetComponent<HighLightFlash>();
        fsm = GetComponent<FSM>();
        fsm.parameter.health = maxHp;
    }

    void FixedUpdate()
    {
        
    }

    public void Damage(float value)
    {
        fsm.GetHit(value);

        hp-= value;
        healthBar.Damage(value);

        if (this.hurtFlashCoroutine != null)
        {
            StopCoroutine(this.hurtFlashCoroutine);
        }

        if (hp <= 0)
        {
            Death();
        }
        else
        {
            this.hurtFlashCoroutine = StartCoroutine(highLightFlash.HurtFlash());
        }
    }

    public void Death()
    {
        highLightFlash.Death();
    }
}
