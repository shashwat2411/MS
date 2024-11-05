using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : IState
{
    private FSM manager;
    private Parameter parameter;

    private float timer;
    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

   public void OnEnter()
    {
       // parameter.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hurt);
        }

        if (parameter.target != null
            && Vector3.Distance(parameter.target.position, manager.transform.position)<parameter.chaseDistance
            )
        {
            manager.TransitionState(StateType.Chase);
        }
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }

}


public class PatrolState : IState
{
    private FSM manager;
    private Parameter parameter;

    private int patrolPosition = 0;
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //parameter.animator.Play("Move");
    }

    public void OnUpdate()
    {
        manager.FilpTo(parameter.patrolPoints[patrolPosition]);

        manager.transform.position = Vector3.MoveTowards(manager.transform.position , 
            parameter.patrolPoints[patrolPosition].position , parameter.moveSpeed * Time.deltaTime);

       

        manager.transform.LookAt(parameter.patrolPoints[patrolPosition].position);
        //Quaternion targetRotation = Quaternion.FromToRotation(manager.transform.forward, targetDir);
        //manager.transform.rotation = Quaternion.RotateTowards(manager.transform.rotation, targetRotation, 20 * Time.deltaTime);


        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hurt);
        }

        if (parameter.target != null
            && Vector3.Distance(parameter.target.position, manager.transform.position) < parameter.chaseDistance
            )
        {
            manager.TransitionState(StateType.Chase);
        }

        if (Mathf.Abs(manager.transform.position.x - parameter.patrolPoints[patrolPosition].position.x) < 0.1f)
        {
            manager.TransitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        patrolPosition++;
        if(patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0;
        }
    }

}



public class ChaseState : IState
{
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Chase");
        //parameter.animator.Play("Move");
    }

    public void OnUpdate()
    {
        manager.FilpTo(parameter.target);

        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hurt);
        }

        if (parameter.target)
        {
            manager.transform.LookAt(parameter.target.position);

            manager.transform.position = Vector3.MoveTowards(manager.transform.position,
           parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(parameter.target.position, manager.transform.position) > (parameter.chaseDistance + 2.0f)
            )
        {
            parameter.target = null;
            manager.TransitionState(StateType.Idle);
        }


        // TODO: attack
        if (false)
        {
            manager.TransitionState(StateType.Attack);
        }
    }

    public void OnExit()
    {

    }

}


public class ReactState : IState
{
    private FSM manager;
    private Parameter parameter;

    public ReactState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

}


public class AttackState : IState
{
    private FSM manager;
    private Parameter parameter;
    private bool hit;
    private AnimatorStateInfo info;
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //parameter.animator.Play("Attack");
        hit = false;
    }

    public void OnUpdate()
    {
        //info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hurt);
        }


     
    }

    public void OnExit()
    {

    }

}


public class HurtState : IState
{
    private FSM manager;
    private Parameter parameter;

    private AnimatorStateInfo info;

    public HurtState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Hurt");
        //parameter.animator.Play("Hurt");
    }

    public void OnUpdate()
    {
        //info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if(parameter.health <= 0)
        {
            manager.TransitionState(StateType.Death);
        }
        //else if(info.normalizedTime >= 0.95f)
        //{
        parameter.target = GameObject.FindWithTag("Player").transform;

        manager.TransitionState(StateType.Chase);
        //}
    }

    public void OnExit()
    {
        parameter.getHit = false;
    }

}


public class DeathState : IState
{
    private FSM manager;
    private Parameter parameter;
    private float timer;
    public DeathState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //parameter.animator.Play("Death");
        
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5f)
        {
            manager.DestoryEnermy();
        }


    }

    public void OnExit()
    {

    }

}
