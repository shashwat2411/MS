using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatch : MonoBehaviour
{
    [SerializeField]
    float waitTime = 5.0f;
    [SerializeField]
    int firstSpawnCount = 2;

    EnemyBase[] enemies;
    

    // Start is called before the first frame update
    void Awake()
    {
        Inite();
    }


   protected void Inite()
    {
        enemies = GetComponentsInChildren<EnemyBase>();
        foreach (EnemyBase enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }




        StartCoroutine(SpawnEnemy());
    }

    void SpawnFirstEnemy()
    {
        for (int i = 0; i < firstSpawnCount; i++)
        {
            enemies[i].gameObject.SetActive(true);
        }
    }


    IEnumerator SpawnEnemy()
    {
        for(int i = 0;i< firstSpawnCount; i++)
        {
            enemies[i].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(waitTime);

        for (int i = firstSpawnCount; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
        }

    }


    // Update is called once per frame
    void Update()
    {

       

    }
}
