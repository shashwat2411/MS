using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : ThrowableEnemyObject
{
    protected override void Start()
    {
        base.Start();

        moveOn = true;
        transform.LookAt(target);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ShieldAbility>() != null) { return; }

        if (collision.gameObject == player && owner != player)
        {
            Destroy(gameObject);
        }
    }
}
