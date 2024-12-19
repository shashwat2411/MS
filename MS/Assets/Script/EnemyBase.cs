using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyMaterial
    {

        public MeshRenderer renderer;
        [ColorUsage(false, true)] public Color color;


        [HideInInspector] public float dissolve;
        [HideInInspector] public float minDissolve;
        [HideInInspector] public float maxDissolve;
        [HideInInspector, ColorUsage(false, true)] public Color originalColor;
        [HideInInspector] public Material material;

        //Hash Map
        [HideInInspector] public int _Color;
        [HideInInspector] public int _Dissolve;
        [HideInInspector] public int _MinDissolve;
        [HideInInspector] public int _MaxDissolve;

        public void InstantiateMaterial()
        {
            if (renderer != null)
            {
                material = Instantiate(renderer.material);
                renderer.material = material;

                _Color = Shader.PropertyToID("_Color");
                _Dissolve = Shader.PropertyToID("_Dissolve");
                _MinDissolve = Shader.PropertyToID("_MinDissolve");
                _MaxDissolve = Shader.PropertyToID("_MaxDissolve");

                dissolve = material.GetFloat(_Dissolve);
                minDissolve = material.GetFloat(_MinDissolve);
                maxDissolve = material.GetFloat(_MaxDissolve);
                originalColor = material.GetColor(_Color);
            }
        }
    };

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
    [SerializeField] protected Animator animator;
    protected HealthBar healthBar;
    protected GameObject player;
    protected Rigidbody rigidbody;
    protected NavMeshAgent agent;
    protected EnemyDialogue dialogue;
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
        dialogue = canvas.GetComponentInChildren<EnemyDialogue>();

        agent.gameObject.transform.parent = null;
        Destroy(agent.GetComponent<MeshRenderer>());
        Destroy(agent.GetComponent<MeshFilter>());
    }

    virtual protected void FixedUpdate()
    {
        rigidbody.angularVelocity = Vector3.zero;

        if (stopMovement == false)
        {
            rigidbody.velocity = agent.velocity;
            transform.LookAt(Vector3.Lerp(transform.position, transform.position + rigidbody.velocity, 0.6f));
        }
    }

    virtual protected void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    virtual protected void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision.gameObject);
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

    virtual public void Damage(float value, bool killingBlow = false)
    {
        healthBar.Damage(value, killingBlow);
    }

    virtual public void Death()
    {
        dead = true;
        Destroy(agent.gameObject);
        Instantiate(Camera.main.transform.parent.GetComponent<EffectPrefabManager>().expEffect, transform.position, transform.rotation);
    }

    virtual public void Knockback(Vector3 direction, float power)
    {
        rigidbody.AddForce(direction.normalized * power, ForceMode.Impulse);
    }

    //___Gizmos_________________________________________________________________________________________________________________________
    virtual protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackDistance);
    }
    //____________________________________________________________________________________________________________________________
}