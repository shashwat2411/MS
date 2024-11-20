using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour, IAtkEffect
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        Destroy(gameObject,lifetime);
        this.damage = damage;
    }

        // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.Damage(damage);
            Debug.Log("Lighting hit ");
        }
    }
}
