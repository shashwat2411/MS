using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : ThrowableEnemyObject
{
    public ParticleSystem[] explosionSystem; 
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

        transform.GetChild(0).parent = null;

        explosionSystem[0].gameObject.SetActive(true);
        explosionSystem[1].gameObject.SetActive(true);

        explosionSystem[0].Play();
        explosionSystem[1].Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && owner != player)
        {
            PlayerManager manager = player.GetComponent<PlayerManager>();
            //プレーヤーへのダメージ
            manager.playerHP.Damage(owner.GetComponent<EnemyBase>().attackPower);
            //other.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        Destroy(gameObject);
    }
}