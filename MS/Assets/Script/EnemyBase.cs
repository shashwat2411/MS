using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    float hp = 100f;
    float maxHp = 100f;

    private HealthBar healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void FixedUpdate()
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
