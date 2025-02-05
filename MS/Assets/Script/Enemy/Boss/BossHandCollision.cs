using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandCollision : BossBodyCollision
{
    private void OnTriggerEnter(Collider collision)
    {
        owner.BossOnCollision(collision.gameObject);
    }
}
