using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoison : ThrowableEnemyObject
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject == player && owner != player)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            EnemyBase enemyBase = player.GetComponent<EnemyBase>();

            if (playerManager != null) { playerManager.playerHP.Damage(damage); }
            else if (enemyBase != null) { enemyBase.Damage(damage); }

            Destroy(gameObject);
        }
    }
}