using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttack : MonoBehaviour
{
    [Header("Close Range")]
    public float attackTime = 0.1f;
    public float damage = 10f;
    public GameObject collider;
    public float maxAttackSize = 2.5f;


    [Header("Long Range")]
    public float cooldown = 0.1f;
    public GameObject bullet;
    private bool shoot = true;

    [Header("InputSystem")]
    public InputActionReference closeRangeAttack;
    public InputActionReference longRangeAttack;

    float holdtime = 1.0f;
    bool isHold = false;
    

    float holdIncreaseFlag = 1.0f;

    
    void Start()
    {
        closeRangeAttack.action.started += HoldAttack;
        longRangeAttack.action.started += LongRangeAttack;



        closeRangeAttack.action.canceled += AttackFinish;



        ResetCollider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isHold)
        {
            collider.GetComponent<Transform>().localScale = Vector3.one * holdtime;

            if (holdtime > maxAttackSize || holdtime < 1.0f)
            {
                holdIncreaseFlag = -holdIncreaseFlag;
            }
           
            holdtime += Time.deltaTime * holdIncreaseFlag;

           
        }

      
    }


    void AttackFinish(InputAction.CallbackContext context)
    {
        isHold = false;
        holdtime = 1.0f;


        Invoke("ResetCollider", attackTime);

        //collider.GetComponent<MeshRenderer>().enabled = true;
        collider.GetComponent<BoxCollider>().enabled = true;
    }

    void HoldAttack(InputAction.CallbackContext context)
    {
        isHold = true;
        collider.GetComponent<MeshRenderer>().enabled = true;

    }


    void CloseRangeAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Close Range Attack");
        Invoke("ResetCollider", attackTime);

        collider.GetComponent<MeshRenderer>().enabled = true;
        collider.GetComponent<BoxCollider>().enabled = true;
    }
    void ResetCollider()
    {

        collider.GetComponent<Transform>().localScale = Vector3.one;
        collider.GetComponent<MeshRenderer>().enabled = false;
        collider.GetComponent<BoxCollider>().enabled = false;
    }

    void LongRangeAttack(InputAction.CallbackContext context)
    {
        if (shoot == true)
        {
            Debug.Log("Long Range Attack");
            Instantiate(bullet, collider.transform.position, collider.transform.rotation).GetComponent<Bullet>().Initiate(transform.forward);
            Invoke("ResetCooldown", cooldown);
            shoot = false;
        }
    }

    void ResetCooldown()
    {
        shoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.Damage(damage);
            Debug.Log(damage);
        }
    }
}
