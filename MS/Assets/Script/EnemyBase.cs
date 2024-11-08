using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    protected float hp = 100f;
    protected float maxHp = 100f;

    [Header("Movement")]
    public float speed;
    public float rotationSpeed;
    protected Vector3 direction;
    protected bool stopRotation;

    [Header("References")]
    protected HealthBar healthBar;
    protected GameObject player;
    protected Rigidbody rigidbody;
    private GameObject canvas;

    [Header("Attack")]
    public float attackDistance;
    public float attackPower;
    public float attackSpeed;
    protected bool attacked;

    virtual protected void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        player = FindFirstObjectByType<PlayerManager>().gameObject;
        rigidbody = GetComponent<Rigidbody>();

        attacked = false;
        stopRotation = false;

        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    virtual protected void FixedUpdate()
    {
        RotateTowards();
    }

    virtual protected void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    virtual protected void OnCollision(GameObject collided)
    {

    }

    virtual protected void RotateTowards()
    {
        if (stopRotation == false)
        {
            Vector3 dir = player.transform.position - gameObject.transform.position;

            dir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
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
