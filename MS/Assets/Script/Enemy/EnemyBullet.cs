using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : ThrowableEnemyObject
{
    protected override void Start()
    {
        base.Start();

        moveOn = true;
        transform.LookAt(new Vector3(target.x, 0f, target.z));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<ShieldAbility>() != null) { return; }

        if (collision.gameObject == player && owner != player)
        {
            player.GetComponent<PlayerManager>().playerHP.Damage(damage);
            Destroy(gameObject);
        }
        else if (owner == player)
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Damage(damage);
                Destroy(gameObject);
            }
        }
    }
}
