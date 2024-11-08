using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttack : MonoBehaviour
{
    [Header("Close Range")]
    public float attackTime = 0.2f;
    public GameObject collider;

    [Header("çUåÇà⁄ìÆîÕàÕ")]
    public float attackMoveRange;


    public bool afterShock = false;

    public GameObject bullet;
    private bool shoot = true;

    [Header("InputSystem")]
    public InputActionReference closeRangeAttack;
    public InputActionReference longRangeAttack;

    float holdtime = 1.0f;
    public bool isHold = false;

    Transform attackArea;

    Vector3 initLocalPosition;


    float holdIncreaseFlag = 1.0f;

    PlayerData playerData;
    
    void Start()
    {

        #region Input
        closeRangeAttack.action.started += HoldAttack;
        longRangeAttack.action.started += LongRangeAttack;



        closeRangeAttack.action.canceled += AttackFinish;

        #endregion

        playerData = GetComponent<PlayerManager>().playerData;

        attackArea = GetComponentsInChildren<Transform>()[1];
        initLocalPosition = attackArea.localPosition;



        ResetCollider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isHold)
        {
            if (holdtime < playerData.maxChargeTime)
            {

                holdtime += Time.deltaTime * playerData.chargeSpeed;
          
                collider.GetComponent<Transform>().localScale =
                    new Vector3(playerData.maxAttackSize / holdtime, 0.5f, playerData.maxAttackSize / holdtime) ;
                    

            }
        }

      
    }


    void AttackFinish(InputAction.CallbackContext context)
    {


        GetComponent<Rigidbody>().velocity = Vector3.zero;
        IniteMenko();
        afterShock = true;


        // Reset
        isHold = false;
        holdtime = 1.0f;
        Invoke("ResetCollider", attackTime);
      


        collider.GetComponent<SphereCollider>().enabled = true;
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
        collider.GetComponent<SphereCollider>().enabled = true;
    }


    void ResetCollider()
    {
        
        afterShock = false;
        holdtime = 1.0f;

        collider.GetComponent<Transform>().localScale =new Vector3( playerData.maxAttackSize,0.5f, playerData.maxAttackSize);
        collider.GetComponent<Transform>().localPosition = initLocalPosition;
        collider.GetComponent<MeshRenderer>().enabled = false;
        collider.GetComponent<SphereCollider>().enabled = false;
    }

    void LongRangeAttack(InputAction.CallbackContext context)
    {
        //if (shoot == true)
        //{
        //    Debug.Log("Long Range Attack");
        //    Instantiate(bullet, collider.transform.position, collider.transform.rotation).GetComponent<Bullet>().Initiate(transform.forward,playerData.attack);
        //    Invoke("ResetCooldown", cooldown);
        //    shoot = false;
        //}
    }

    void ResetCooldown()
    {
        
        shoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    /// <summary>
    ///  ÇﬂÇÒÇ±ê∂ê¨
    /// </summary>
    void IniteMenko()
    {
       
        Vector3 startPoint = this.transform.position + Vector3.up * 3.0f;

        Vector3 endPoint = collider.transform.position;
        float offset = 1.5f;
        if (holdtime < 2.5f)
        {
            endPoint = collider.transform.position +
                new Vector3(
                             Random.Range(-offset / holdtime, offset / holdtime),
                             0.0f,
                             Random.Range(-offset / holdtime, offset / holdtime)
                         );

        }


        Debug.Log(holdtime +"  " + endPoint);
        Vector3 dir = endPoint - startPoint;

        Instantiate(bullet, startPoint, collider.transform.rotation).GetComponent<Bullet>().Initiate(dir, playerData.attack);

    }

    public bool RangeMove(Vector3 playerInput)
    {


      var newPos = new Vector3(  
                              attackArea.position.x + playerInput.x * Time.deltaTime * playerData.atkMoveSpeed,
                              attackArea.position.y,
                              attackArea.position.z + playerInput.z * Time.deltaTime * playerData.atkMoveSpeed
                              );

       var offset = newPos - this.transform.position;

        //  attackArea.localPosition = Vector3.zero + Vector3.ClampMagnitude(offset, attackMoveRange);

        attackArea.position = newPos;
        //attackArea.localPosition =Vector3.ClampMagnitude(offset, attackMoveRange);


        //if (attackArea.localPosition.z <0.5f)
        //{
        //    attackArea.localPosition = new Vector3(attackArea.localPosition.x, attackArea.localPosition.y, 0.5f);
        //}                                              




        return true;

    }
}
