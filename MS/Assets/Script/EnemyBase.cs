using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected float hp = 100f;
    protected float maxHp = 100f;

    protected HealthBar healthBar;

    virtual protected void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    virtual protected void FixedUpdate()
    {
        
    }

    public void Damage(float value)
    {
        healthBar.Damage(value);
    }

    public void Death()
    {

    }
}
