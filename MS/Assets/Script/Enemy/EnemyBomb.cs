using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : ThrowableEnemyObject
{
    public ParticleSystem[] explosionSystem;
    private bool grounded = false;
    protected override void Start()
    {
        base.Start();

        grounded = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (grounded == true)
        {
            transform.GetChild(0).parent = null;

            explosionSystem[0].gameObject.SetActive(true);
            explosionSystem[1].gameObject.SetActive(true);

            explosionSystem[0].Play();
            explosionSystem[1].Play();
        }
        else
        {
            explosionSystem[2].transform.parent = null;

            explosionSystem[2].gameObject.GetComponent<ParticleSystemDestroyer>().enabled = true;

            explosionSystem[2].gameObject.SetActive(true);
            explosionSystem[2].Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (other.gameObject == player && owner != player)
        //{
        //    PlayerManager manager = player.GetComponent<PlayerManager>();
        //    //プレーヤーへのダメージ
        //    manager.playerHP.Damage(owner.GetComponent<EnemyBase>().attackPower);
        //    //other.GetComponent<MeshRenderer>().material.color = Color.green;
        //}
        if (collision.gameObject.GetComponent<ShieldAbility>() != null) { return; }

        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }

        Debug.Log("Collision : " + collision.gameObject.tag);

        Destroy(gameObject);
    }
}