using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    List<GameObject> allEnemies = new List<GameObject>();

    public float damage;
    static float factor = 5.0f;
    [SerializeField] float knockForce = 3000.0f;

    float curTime = 0;


    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        Destroy(gameObject, lifetime * factor);

        this.damage = 2.0f;
    }

    public void LevelUp()
    {
        factor += 0.4f;
        
        Debug.Log("LevelUp   " + factor);
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
        KnockBackAllEnemies();

    }


    void KnockBackAllEnemies()
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            if (allEnemies[i] != null)
            {
                var enemyBase = allEnemies[i].GetComponent<EnemyBase>();
                if (!enemyBase.dead)
                {
                    var enemyRigidBody = enemyBase.GetComponent<Rigidbody>();
                    var startPos = this.transform.position;
                    var dir = enemyBase.transform.position - startPos;
                    dir.y = 0.0f;
                    dir.Normalize();

                    enemyRigidBody.AddForce(dir * knockForce);

                    enemyBase.Damage(this.damage);
                    
                }
                  
              
                allEnemies.Remove(allEnemies[i]);
            }
        }

       // Destroy(this.gameObject);
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
