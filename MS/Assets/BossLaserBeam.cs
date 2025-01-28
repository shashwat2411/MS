using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserBeam : MonoBehaviour
{

    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerManager>();
        if (player)
        {
            var boss = GetComponentInParent<BossEnemy>();
            player.Damage(boss.laserBeamAttackPower);
            Debug.Log("Laser hit");

        }
    }
}
