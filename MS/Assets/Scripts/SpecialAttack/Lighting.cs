using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour, IAtkEffect
{
    public float damage;

    static float factor = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        var offset = new Vector3(Random.Range(2.0f, -2.0f), 0, Random.Range(2.0f, 0.0f));
        this.transform.position += offset;
        
        this.transform.localScale = Vector3.one * damage /25.0f; 
        Destroy(gameObject,lifetime);
        this.damage = damage* factor;
        
        Debug.Log("Lighting:  "+ damage);
    }

    public void LevelUp()
    {
        factor += 0.2f;
        Debug.Log("LevelUp   " + factor);
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
        
        }
    }
}
