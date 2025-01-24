using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;

public class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyMaterial
    {
        public Renderer renderer;
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

        public Color GetColor() { return material.GetColor( _Color ); }
        public void SetColor(Color color) { material.SetColor(_Color, color);}
        public bool GetDissolvedOut()
        {
            if (dissolve <= minDissolve) { return true; }
            else { return false; }
        }
        public bool GetDissolvedIn()
        {
            if (dissolve >= maxDissolve) { return true; }
            else { return false; }
        }

        public void SetDissolveToMax()
        {
            dissolve = maxDissolve;
            material.SetFloat(_Dissolve, dissolve);
        }
        public void SetDissolveToMin()
        {
            dissolve = minDissolve;
            material.SetFloat(_Dissolve, dissolve);
        }

        public void SetMaxDissolveScale(float scale)
        {
            maxDissolve *= scale;
            material.SetFloat(_MaxDissolve, maxDissolve);
            SetDissolveToMax();
        }
        public void SetMinDissolveScale(float scale)
        {
            minDissolve *= scale;
            material.SetFloat(_MinDissolve, minDissolve);
        }

        public IEnumerator DissolveOut(float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                dissolve = Mathf.Lerp(maxDissolve, minDissolve, elapsedTime / duration);
                material.SetFloat(_Dissolve, dissolve);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            dissolve = minDissolve;
            material.SetFloat(_Dissolve, dissolve);
            yield return null;
        }
        public IEnumerator DissolveIn(float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                dissolve = Mathf.Lerp(minDissolve, maxDissolve, elapsedTime / duration);
                material.SetFloat(_Dissolve, dissolve);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            dissolve = maxDissolve;
            material.SetFloat(_Dissolve, dissolve);
            yield return null;
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
    protected bool stopLooking;

    [Header("References")]
    [SerializeField] protected Animator animator;
    protected HealthBar healthBar;
    protected GameObject player;
    protected Rigidbody rigidbody;
    protected NavMeshAgent agent;
    protected EnemyDialogue dialogue;
    protected Transform areaChecker;
    private GameObject canvas;
    private Transform mainCamera;
    protected Vector3 extraForce;

    [Header("Attack")]
    public float attackDistance;
    public float attackPower;
    public float attackSpeed;
    [SerializeField] protected bool attacked;

    public bool dead = false;

    virtual protected void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        player = FindFirstObjectByType<PlayerManager>().gameObject;
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponentInChildren<NavMeshAgent>();
        mainCamera = Camera.main.transform;

        attacked = false;
        stopRotation = false;
        stopMovement = false;
        stopLooking = false;
        dead = false;

        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        dialogue = canvas.GetComponentInChildren<EnemyDialogue>();

        agent.gameObject.transform.parent = null;
        Destroy(agent.GetComponent<MeshRenderer>());
        Destroy(agent.GetComponent<MeshFilter>());

        areaChecker = transform;
    }

    virtual protected void FixedUpdate()
    {
        extraForce *= 0.95f;

        rigidbody.angularVelocity = Vector3.zero;

        if (stopMovement == false)
        {
            rigidbody.velocity = agent.velocity + extraForce;
            if (stopLooking == false) { transform.LookAt(Vector3.Lerp(transform.position, transform.position + rigidbody.velocity, 0.6f)); }
        }
    }

    virtual protected void LateUpdate()
    {
        if (canvas != null)
        {
            canvas.transform.LookAt(canvas.transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
        }
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
        var expEffect = Instantiate(mainCamera.parent.GetComponent<EffectPrefabManager>().expEffect, transform.position, transform.rotation);
        ParticleSystem particle = expEffect.GetComponent<ParticleSystem>();

        ParticleSystem.EmissionModule emission = particle.emission;

        int stageNum = FindFirstObjectByType<StageNumber>().stageNum; 

        ParticleSystem.Burst burst = new ParticleSystem.Burst();
        burst.time = 0f;   
        burst.count = 51 + stageNum * 25;

         FindFirstObjectByType<EnemyManager>();
        emission.SetBursts(new ParticleSystem.Burst[] { burst });


    }

    virtual public void Knockback(Vector3 direction, float power)
    {
        extraForce = direction.normalized * power;
        //rigidbody.AddForce(direction.normalized * power, ForceMode.Impulse);
    }

    virtual protected void ScaleUp()
    {
        float scale = transform.localScale.x;

        attackDistance *= scale;
        healthBar.maxHealth *= scale;
        healthBar.health = healthBar.maxHealth;
    }

    public GameObject GetPlayer() { return player; }

    //___Gizmos_________________________________________________________________________________________________________________________
    virtual protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackDistance);
    }
    //____________________________________________________________________________________________________________________________

    public Animator GetAnimator() { return animator; }
}