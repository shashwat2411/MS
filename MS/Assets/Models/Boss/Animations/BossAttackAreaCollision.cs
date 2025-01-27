using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackAreaCollision : MonoBehaviour
{
    public float counter = 0f;
    public float stayTime;
    public BossEnemy owner;

    private bool once = false;
    private void OnTriggerStay(Collider other)
    {
        if (once == false)
        {
            counter += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        counter = 0f;
        once = false;
    }

    private void FixedUpdate()
    {
        if (counter > stayTime && once == false)
        {
            once = true;
            owner.SetNextPhaseToLaserBeam();
        }
    }
}
