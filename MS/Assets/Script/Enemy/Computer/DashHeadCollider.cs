using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHeadCollider : MonoBehaviour
{
    public EnemyBase owner { get; private set; }
    void Start()
    {
        owner = GetComponentInParent<EnemyBase>();
    }
}
