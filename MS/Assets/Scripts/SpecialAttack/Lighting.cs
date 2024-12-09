using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Lighting : MonoBehaviour, IAtkEffect
{
    public float damage;

    static float factor = 1.0f;

    public float offsetParamater = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        var offset = new Vector3(Random.Range(offsetParamater, -offsetParamater), 0, Random.Range(offsetParamater, 0.0f));
        this.transform.position += offset;

      
        this.transform.localScale = Vector3.one * Mathf.Clamp(damage / 30.0f, 0f, 1f); 
        Destroy(gameObject,lifetime);
        this.damage = damage* factor;
        
        Debug.Log("Lighting:  " + factor);
    }

    public void LevelUp()
    {
        factor += 0.2f;
        Debug.Log("LevelUp   " + factor);
    }

    public void ResetLevel()
    {
        factor = 1.0f;
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
