using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatch : MonoBehaviour
{
    [SerializeField]
    float waitTime = 5.0f;
    [SerializeField]
    float waitTime2 = 5.0f;

    public float delay = 5f;
    public float delay2 = 5f;

    [SerializeField]
    int firstSpawnCount = 2;
    [SerializeField]
    int secondSpawnCount = 2;

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

        for (int i = firstSpawnCount; i < secondSpawnCount; i++)
        {
            StartCoroutine(enemies[i].DissolveIn(0f, 1f));
            yield return new WaitForSeconds(waitTime2);
        }

        yield return new WaitForSeconds(delay2);

        for (int i = secondSpawnCount; i < enemies.Length; i++)
        {
            StartCoroutine(enemies[i].DissolveIn(0f, 1f));
            yield return new WaitForSeconds(waitTime);
        }

    }
}
