using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3f;
    public float lifetime = 10f;
    Vector3 hitPos = Vector3.zero;

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





    public void Initiate(Vector3 direction,Vector3 hitPosition ,float damage = 1.0f)
    {
        GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        this.damage = damage/10.0f;
        hitPos = hitPosition;
        Invoke("DestroyBullet", lifetime);
    }


    private void OnTriggerEnter(Collider other)
    {
     
       if (other.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().velocity =Vector3.zero;
            //this.transform.position = new Vector3(this.transform.position.x, 0.2f, this.transform.position.z);
            this.transform.position = hitPos+ Vector3.up * 0.1f;

            impactArea.SetActive(true);
            impactEffect.SetActive(true);
            impactArea.transform.localScale = Vector3.one * this.damage;
            impactEffect.transform.localScale =new Vector3(this.damage, this.damage, this.damage/2);

            Debug.Log(this.damage);
            Invoke("DestroyBullet", lifetime);
        }
    }

    void DestroyBullet()
    {
        impactArea.SetActive(false);
        impactEffect.SetActive(false);
        ObjectPool.Instance.Push(gameObject);


        //Destroy(impactArea);
        //Destroy(impactEffect);
        //Destroy(gameObject);
    }
}
