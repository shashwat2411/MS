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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") == true)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject == player)
        {
            //player�@��ŏ�ԂɕύX
            Destroy(gameObject);
        }
    }
}