using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : ThrowableEnemyObject
{
    private bool grounded;
    private bool collided;
    private Vector3 fixedPosition;
    protected override void Start()
    {
        base.Start();

        grounded = false;
        collided = false;
    }

    protected override void FixedUpdate()
    {
        if (grounded == false)
        {
            base.FixedUpdate();
        }
        else
        {
            transform.position = fixedPosition;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        //ÉvÉåÅ[ÉÑÅ[ÇÃë´é~Çﬂ
        if(collided == true)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            fixedPosition = transform.position;
            grounded = true;
            return;
        }

        if (collision.gameObject == player && owner != player)
        {
            collided = true;
            //other.GetComponent<MeshRenderer>().material.color = Color.green;
            Destroy(gameObject);
        }
    }
}
