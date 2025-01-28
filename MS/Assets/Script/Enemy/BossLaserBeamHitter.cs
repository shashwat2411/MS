using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserBeamHitter : MonoBehaviour
{

    BoxCollider boxCollider;
    public GameObject laserBeamEffect;

    // Start is called before the first frame update
    void Awake()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = laserBeamEffect.transform.rotation;    
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
