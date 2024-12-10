using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Lighting : MonoBehaviour, IAtkEffect
{
    public float damage;

    // list
    [SerializeField]
    List<float> damageFactor = new List<float>();

    public float offsetParamater = 3.0f;

    static int lv = 1;

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

        var index = lv - 1;
        if (index >= damageFactor.Count)
        {
            index = damageFactor.Count - 1; 
        }

        this.damage = damage * damageFactor[index];

        Debug.Log("Lighting:  " + this.damage);
    }

    public void LevelUp()
    {
        lv++;
        
       // Debug.Log("LevelUp   " + damageFactor);
    }

    public void ResetLevel()
    {
        lv = 1;
     
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
