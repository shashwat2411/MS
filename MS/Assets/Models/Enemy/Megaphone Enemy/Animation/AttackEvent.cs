using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public ThrowEnemy owner;

    public void Attack()
    {
        owner.AttackInstantiate();
    }
}
