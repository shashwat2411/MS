using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,Patrol,Chase,React,Attack,Hurt,Death
}

[Serializable]
public class Parameter
{
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public float chaseDistance;

    public Transform[] patrolPoints;
    public Transform[] chasePoints;

    public Transform target;

    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;

    public float attackTime;

    public PlayerManager player;

    public Animator animator;

    public GameObject dropObject;

    public bool getHit;
    public int damage=1;

    
    public HighLightFlash highLightFlash;
    public Coroutine hurtFlashCoroutine;
}

public class FSM : MonoBehaviour
{
    public Parameter parameter;

    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle,new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hurt, new HurtState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);

        parameter.animator = GetComponent<Animator>();
        parameter.highLightFlash = GetComponent<HighLightFlash>();
    }

    // Update is called once per frame
    void Update()
    {
       
        currentState.OnUpdate();
        
   
        
    }
    
    public void TransitionState(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();

    }

    public void FilpTo(Transform target)
    {
        if (target != null)
        {
            if(transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            parameter.target = other.transform;
            Debug.Log($"find {parameter.target}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // parameter.target = null;
           // Debug.Log($"lost ");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position,parameter.attackArea);
    }


    public void GetHit(float value)
    {
        parameter.getHit = true;
        parameter.health -= value;

    }
    public void DestoryEnermy()
    {
        Destroy(gameObject);

        foreach(Transform gameObject in parameter.chasePoints)
        {
            Destroy(gameObject.gameObject);
        }

        foreach (Transform gameObject in parameter.patrolPoints)
        {
            Destroy(gameObject.gameObject);
        }

        if (parameter.dropObject != null)
        {
            Instantiate(parameter.dropObject, transform.position + new Vector3(0.0f, 0.5f, 0.0f), Quaternion.identity);
        }
       
    }
}
