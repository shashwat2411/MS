using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartphoneAttackEvent : MonoBehaviour
{
    public SmartphoneEnemy owner;

    public void Attack()
    {
        owner.AttackInstantiate();

        float scale = owner.transform.localScale.x;
        owner.GetItem().transform.localScale = owner.GetItem().transform.localScale * scale;
    }
    public void AttackOver()
    {
        owner.AttackOver();
    }
}
