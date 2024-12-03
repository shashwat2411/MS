using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFire : MonoBehaviour,IAtkEffect
{
    List<GameObject> allEnemies = new List<GameObject>();

    public float damage;
    static float factor = 5.0f;

    public float declineInterval = 0.5f;
    float curTime = 0;
    

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        Destroy(gameObject, lifetime * factor);

        this.damage = 2.0f;
    }

    public void LevelUp()
    {
        factor += 0.4f;
        declineInterval -= 0.1f;
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
        curTime += Time.deltaTime;
        if (curTime >= declineInterval)
        {
            DamageAllEnemies();
            curTime = 0;
        }


    }


    void DamageAllEnemies()
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            if (allEnemies[i] != null)
            {
                var enemyBase = allEnemies[i].GetComponent<EnemyBase>();
                if (enemyBase.dead)
                {
                    allEnemies.Remove(allEnemies[i]);
                }
                else
                {
                    enemyBase.Damage(this.damage);
                }
            }
           
           
           
        }
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
