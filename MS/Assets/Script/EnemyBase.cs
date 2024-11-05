using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected float hp = 100f;
    protected float maxHp = 100f;

    protected HealthBar healthBar;
    protected GameObject player;
    protected Rigidbody rigidbody;

    virtual protected void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        player = FindFirstObjectByType<PlayerManager>().gameObject;
        rigidbody = GetComponent<Rigidbody>();
    }

    virtual protected void FixedUpdate()
    {

    }
    virtual protected void OnCollision(GameObject collided)
    {

    }

    public void Damage(float value)
    {
        healthBar.Damage(value);
    }

    public void Death()
    {

    }

    protected void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision.gameObject);
    }
}
