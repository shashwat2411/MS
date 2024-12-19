using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttackEvent : MonoBehaviour
{
    public GhostEnemy owner;


    public void Attack()
    {
        owner.AttackInstantiate();
        owner.GetItem().start = false;
    }

    public void Launch()
    {
        owner.GetItem().start = true;
    }
    public void AttackOver()
    {
        owner.AttackOver();
    }
}
