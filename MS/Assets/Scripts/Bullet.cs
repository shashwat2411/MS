using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3f;
    public float lifetime = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void Initiate(Vector3 direction)
    {
        GetComponent<Rigidbody>().velocity = direction.normalized * speed;

        Invoke("DestroyBullet", lifetime);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
