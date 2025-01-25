using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandCollision : BossBodyCollision
{
    private void OnCollisionEnter(Collision collision)
    {
        owner.BossOnCollision(collision.gameObject);
    }
}
