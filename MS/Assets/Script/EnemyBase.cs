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
    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        highLightFlash = GetComponent<HighLightFlash>();
    }

    void FixedUpdate()
    {
        
    }

    public void Damage(float value)
    {
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
