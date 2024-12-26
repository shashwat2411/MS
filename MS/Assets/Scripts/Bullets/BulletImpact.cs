using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [HideInInspector] public float damage;

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.Damage(damage, true);
            return;
        }

        BossEnemy boss = other.gameObject.GetComponent<BossEnemy>();
        if(boss)
        {
            boss.Damage(damage, true);
        }
    }
}
