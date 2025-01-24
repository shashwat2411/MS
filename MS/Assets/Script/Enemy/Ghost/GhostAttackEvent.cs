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

        float scale = owner.transform.localScale.x;
        owner.GetItem().localScale = new Vector3(0.75f, 0.75f, 0.75f) * scale;
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
