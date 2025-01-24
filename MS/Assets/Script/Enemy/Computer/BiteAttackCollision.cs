using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAttackCollision : MonoBehaviour
{
    public DashEnemy owner;

    private void OnTriggerEnter(Collider other)
    {
        owner.GetPlayer().GetComponent<PlayerManager>().playerHP.Damage(owner.attackPower);
    }
}
