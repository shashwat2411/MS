using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : ThrowableEnemyObject
{
    private bool grounded = false;
    private float countdownValue;
    private Vector3 fixedPosition;

    public ParticleSystem[] explosionSystem;
    public MeshRenderer fuseMaterial;
    protected override void Start()
    {
        base.Start();

        grounded = false;

        fuseMaterial.material = Instantiate(fuseMaterial.material);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (grounded == true) { transform.position = fixedPosition; }

        countdownValue = (maxLifetime - lifetime) / maxLifetime;
        fuseMaterial.material.SetFloat("_Transparency", countdownValue);
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
            fixedPosition = transform.position;
            grounded = true;
            return;
        }

        if (collision.gameObject == player && owner != player)
        {
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}