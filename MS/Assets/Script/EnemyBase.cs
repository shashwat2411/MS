using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    protected float hp = 100f;
    protected float maxHp = 100f;

    [Header("Movement")]
    //public float speed;
    //public float rotationSpeed;
    protected Vector3 direction;
    protected bool stopRotation;
    protected bool stopMovement;

    [Header("References")]
    protected HealthBar healthBar;
    protected GameObject player;
    protected Rigidbody rigidbody;
    protected NavMeshAgent agent;
    private GameObject canvas;

    [Header("Attack")]
    public float attackDistance;
    public float attackPower;
    public float attackSpeed;
    protected bool attacked;

    public bool dead = false;

    virtual protected void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        player = FindFirstObjectByType<PlayerManager>().gameObject;
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponentInChildren<NavMeshAgent>();

        attacked = false;
        stopRotation = false;
        stopMovement = false;
        dead = false;


        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;

        agent.gameObject.transform.parent = null;
    }

    virtual protected void FixedUpdate()
    {
        rigidbody.angularVelocity = Vector3.zero;

        //RotateTowards();
        if (stopMovement == false)
        {
            rigidbody.velocity = agent.velocity;

            transform.LookAt(Vector3.Lerp(transform.position, transform.position + agent.velocity, 0.6f));
        }
    }

    virtual protected void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    virtual protected void OnCollision(GameObject collided)
    {

    }

    virtual protected void RotateTowards(Vector3 direction)
    {
        if (stopRotation == false)
        {
            Vector3 dir = (direction - gameObject.transform.position).normalized;
            //Vector3 dir = agent.velocity.normalized;
            //if (dir.magnitude <= 0.0001f) { dir = new Vector3(0.01f, 0.01f, 0.01f); }


            dir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.6f);
        }
    }

    virtual protected void Move()
    {
        agent.isStopped = false;

        agent.SetDestination(player.transform.position);
    }

    virtual public void Damage(float value)
    {
        healthBar.Damage(value);
    }

    virtual public void Death()
    {
        dead = true;
        Destroy(agent.gameObject);
    }

    virtual public void Knockback(Vector3 direction, float power)
    {
        rigidbody.AddForce(direction.normalized * power, ForceMode.Impulse);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision.gameObject);
    }

    //___Gizmos_________________________________________________________________________________________________________________________
    virtual protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackDistance);
    }
    //____________________________________________________________________________________________________________________________
}