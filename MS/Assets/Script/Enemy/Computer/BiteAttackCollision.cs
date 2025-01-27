using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAttackCollision : MonoBehaviour
{
    public DashEnemy owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner.GetPlayer())
        {
            if (owner.attackedOnce == false)
            {
                owner.GetPlayer().GetComponent<PlayerManager>().playerHP.Damage(owner.attackPower);
                owner.attackedOnce = true;
            }
        }
    }
}
