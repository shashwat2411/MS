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

        DashHeadCollider enemyCollider = other.gameObject.GetComponent<DashHeadCollider>();
        if (enemyCollider)
        {
            enemyCollider.owner.Damage(damage, true);
            return;
        }

        BossBodyCollision bossCollider = other.gameObject.GetComponent<BossBodyCollision>();
        if (bossCollider)
        {
            if (bossCollider.owner)
            {
                bossCollider.owner.Damage(damage, true);
                return;
            }
        }
    }
}
