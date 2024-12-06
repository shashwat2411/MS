using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssisitedAiming : MonoBehaviour
{
    List<GameObject> allEnemies = new List<GameObject>();

    PlayerAttack playerAttack;
    PlayerManager playerManager;
    
    private void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerManager = GetComponentInParent<PlayerManager>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy && !allEnemies.Contains(other.gameObject))
        {
            allEnemies.Add(enemy.gameObject);
        }
    }

    void FixedUpdate()
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            if (allEnemies[i] == null)
            {
                allEnemies.RemoveAt(i); 
            }
        }

       if (allEnemies.Count > 0)
       {
            playerAttack.attackRangeMoveFactor = 0.6f;
            
       }
       else
       {
           playerAttack.attackRangeMoveFactor = 1.0f;
           
       }

    }

    private void OnDisable()
    {
        allEnemies.Clear(); 
    }


    private void OnTriggerExit(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy && allEnemies.Contains(other.gameObject))
        {
            allEnemies.Remove(enemy.gameObject);
        }
    }
}
