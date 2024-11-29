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

        //����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && owner != player)
        {
            PlayerManager manager = player.GetComponent<PlayerManager>();
            //�v���[���[�ւ̃_���[�W
            manager.playerHP.Damage(owner.GetComponent<EnemyBase>().attackPower);
            //other.GetComponent<MeshRenderer>().material.color = Color.green;
            Destroy(gameObject);
        }
    }
}