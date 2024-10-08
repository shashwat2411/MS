using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    float hp = 100f;
    float maxHp = 100f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void Damage(float value)
    {
        if(hp - value > 0){ hp -= value;}
        else { hp = 0; }
    }
}
