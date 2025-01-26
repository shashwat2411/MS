using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBase;

public class EnemyPoison : ThrowableEnemyObject
{
    public float rotationValue = 1f;

    public float dissolveDuration = 0.5f;
    public EnemyMaterial phone;
    public EnemyMaterial screen;

    private bool dead = false;
    protected override void Start()
    {
        base.Start();

        phone.InstantiateMaterial();
        screen.InstantiateMaterial();

        deathDelay = dissolveDuration;
        dead = false;
    }

    protected override void FixedUpdate()
    {
        if (dead == false)
        {
            base.FixedUpdate();

            transform.Rotate(new Vector3(1f, 0f, 0f), rotationValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            Destructor();
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

    protected override void Destructor()
    {
        dead = true;
        Destroy(gameObject, deathDelay);
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<Rigidbody>());
        StartCoroutine(phone.DissolveOut(dissolveDuration));
        StartCoroutine(screen.DissolveOut(dissolveDuration));
    }

}