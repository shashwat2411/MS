using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{

    public ThrowEnemy parent;
    int moveHash;
    int attackHash;
    Animator animator;
    ThrowEnemy.THROWENEMY_STATE state;

    // Start is called before the first frame update
    void Start()
    {
       
        animator = GetComponent<Animator>();
        moveHash = Animator.StringToHash("Move");
        attackHash = Animator.StringToHash("Attack");
    }


    void FixedUpdate()
    {

        
        if(parent.state == ThrowEnemy.THROWENEMY_STATE.MOVE)
        {
            animator.SetBool(moveHash, true);
        }
        else if (parent.state == ThrowEnemy.THROWENEMY_STATE.IDLE)
        {
            animator.SetBool(moveHash, false);
        }


        if(parent.state == ThrowEnemy.THROWENEMY_STATE.ATTACK)
        {
            animator.SetBool(attackHash, true);
        }
        else
        {
            animator.SetBool(attackHash, false);
        }
    }
}
