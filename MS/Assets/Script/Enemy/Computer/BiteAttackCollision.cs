using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAttackCollision : MonoBehaviour
{
    public DashEnemy owner;
    private PlayerManager player;
    private void Start()
    {
        player = owner.GetPlayer().GetComponent<PlayerManager>();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        player.playerHP.Damage(owner.attackPower);
    }
}
