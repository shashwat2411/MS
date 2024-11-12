using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3f;
    public float lifetime = 1f;

    Collider collider;

    [SerializeField]
    GameObject impactArea;
    [SerializeField]
    GameObject impactEffect;

    public float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }





    public void Initiate(Vector3 direction,float damage = 1.0f)
    {
        GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        this.damage = damage;
       // Invoke("DestroyBullet", lifetime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().velocity =Vector3.zero;
            this.transform.position = new Vector3(this.transform.position.x, 0.2f, this.transform.position.z);


            impactArea.SetActive(true);
            impactEffect.SetActive(true);

            Debug.Log(transform.position);
            Invoke("DestroyBullet", lifetime);
        }
    }

    void DestroyBullet()
    {

     
        Destroy(impactArea);
        Destroy(impactEffect);
        Destroy(gameObject);
    }
}
