using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Vector2 range;

    private void Awake()
    {
        EnemyBase[] enemies = GetComponentsInChildren<EnemyBase>();

        foreach(EnemyBase enemy in enemies)
        {
            float value = Random.Range(range.x, range.y);
            enemy.transform.localScale = Vector3.one * value;
        }
    }

    public int GetCount() { return transform.childCount; }
}
