using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatch : MonoBehaviour
{
    [SerializeField]
    float waitTime = 5.0f;

    public float delay = 5f;

    [SerializeField]
    int firstSpawnCount = 2;

    EnemyBase[] enemies;
    

    void Awake()
    {
        Inite();
    }


   protected void Inite()
    {
        enemies = GetComponentsInChildren<EnemyBase>();

        StartCoroutine(SpawnEnemy());
    }


    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < firstSpawnCount; i++)
        {
            StartCoroutine(enemies[i].DissolveIn(0f, 1f));
            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(delay);

        for (int i = firstSpawnCount; i < enemies.Length; i++)
        {
            StartCoroutine(enemies[i].DissolveIn(0f, 1f));
            yield return new WaitForSeconds(waitTime);
        }

    }
}
