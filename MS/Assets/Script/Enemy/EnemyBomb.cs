using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : ThrowableEnemyObject
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        //爆風
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && owner != player)
        {
            PlayerManager manager = player.GetComponent<PlayerManager>();
            //プレーヤーへのダメージ
            manager.playerHP.Damage(owner.GetComponent<EnemyBase>().attackPower);
            //other.GetComponent<MeshRenderer>().material.color = Color.green;
            Destroy(gameObject);
        }
    }
}