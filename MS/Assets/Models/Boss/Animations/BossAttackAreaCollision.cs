using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackAreaCollision : MonoBehaviour
{
    public float counter = 0f;
    public float stayTime;
    public BossEnemy owner;

    private bool once = false;
    private bool twice = false;
    private void OnTriggerStay(Collider other)
    {
        if (once == false)
        {
            counter += Time.deltaTime;
        }
        else if(twice == false)
        {
            twice = true;
            Invoke("ResetTimer", stayTime * 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ResetTimer();
    }

    private void FixedUpdate()
    {
        if (counter > stayTime && once == false)
        {
            once = true;
            owner.SetNextPhaseToLaserBeam();
        }
    }

    private void ResetTimer()
    {
        counter = 0f;
        once = false;
        twice = false;
    }
}
