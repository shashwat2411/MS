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

    [Header("攻撁E��動篁E��")]
    public float attackMoveRange;



    [HideInInspector] public float collisionDamage = 10f;



    public GameObject bullet;
    private bool shoot = true;

    [Header("InputSystem")]
    public InputActionReference closeRangeAttack;
    public InputActionReference longRangeAttack;

    //
    float holdtime = 1.0f;
    public bool isHold = false;
    public bool afterShock = false;

    //攻撁E�E位置
    Transform attackArea;

    //攻撁E�E初期生�E位置
    Vector3 initLocalPosition;


    PlayerData playerData;
    public List<GameObject> enemies;

    Vector3 attackTarget;
    void Start()
    {

        #region Input
        closeRangeAttack.action.started += HoldAttack;



        closeRangeAttack.action.canceled += AttackFinish;

        #endregion

        playerData = GetComponent<PlayerManager>().playerData;

        attackArea = GetComponentsInChildren<Transform>()[1];
        initLocalPosition = attackArea.localPosition;
     

        ResetCollider();
    }

    void test()
    {

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
                    new Vector3(playerData.maxAttackSize / holdtime, 0.2f, playerData.maxAttackSize / holdtime) ;

                

            }
            //MoveToTarget(attackTarget);
           // GetCloestEnemy();
        }
    }

    /// <summary>
    /// trigger���������A�߂񂱔���
    /// </summary>
    /// <param name="context"></param>
    void AttackFinish(InputAction.CallbackContext context)
    {

        if (isHold && !afterShock)
        {
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            IniteMenko();

            // Reset
            isHold = false;
            afterShock = true;
            holdtime = 1.0f;
            collider.GetComponent<MeshRenderer>().enabled = false;
            //collider.GetComponent<SphereCollider>().enabled = false;
            Invoke("ResetCollider", attackTime);



        
          
       
        }
      
    }


    void HoldAttack(InputAction.CallbackContext context)
    {
        if(!afterShock)
        {
            isHold = true;
            collider.GetComponent<MeshRenderer>().enabled = true;
            GetCloestEnemy();
        }
       

    }


    void CloseRangeAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Close Range Attack");
        Invoke("ResetCollider", attackTime);
    
        collider.GetComponent<MeshRenderer>().enabled = true;
        //collider.GetComponent<SphereCollider>().enabled = true;
    }


    void ResetCollider()
    {

        afterShock = false;
        holdtime = 1.0f;

        collider.GetComponent<Transform>().localScale =new Vector3( playerData.maxAttackSize,0.05f, playerData.maxAttackSize);
        collider.GetComponent<Transform>().localPosition = initLocalPosition;
        collider.GetComponent<MeshRenderer>().enabled = false;
        //collider.GetComponent<SphereCollider>().enabled = false;
    }

   

    void ResetCooldown()
    {
        
        shoot = true;
    }

  

    /// <summary>
    ///  めんこ生戁E
    /// </summary>
    void IniteMenko()
    {
       
        Vector3 startPoint = this.transform.position + Vector3.up * 1.0f;

        Vector3 endPoint = new Vector3(collider.transform.position.x,0.0f, collider.transform.position.z);
        float offset = 1.5f;
        if (holdtime < 3.5f)
        {
            endPoint = collider.transform.position +
                new Vector3(
                             Random.Range(-offset / holdtime, offset / holdtime),
                             0.0f,
                             Random.Range(-offset / holdtime, offset / holdtime)
                         );

        }


        //Debug.Log(holdtime +"  " + endPoint );

        Vector3 dir = endPoint - startPoint ;
        dir.Normalize();


        //Instantiate(bullet, startPoint, collider.transform.rotation).GetComponent<Bullet>().Initiate(dir, playerData.attack);
        var obj = ObjectPool.Instance.Get(bullet, startPoint, collider.transform.rotation);
        obj.GetComponent<Bullet>().Initiate(dir, endPoint,playerData.attack * holdtime);

    }


    public bool RangeMove(Vector3 playerInput)
    {


      var newPos = new Vector3(  
                              attackArea.position.x + playerInput.x * Time.deltaTime * playerData.atkMoveSpeed,
                              attackArea.position.y,
                              attackArea.position.z + playerInput.z * Time.deltaTime * playerData.atkMoveSpeed
                              );

        var localPoint = transform.InverseTransformPoint(newPos);
        attackArea.localPosition =  Vector3.ClampMagnitude(localPoint, attackMoveRange);


        if (attackArea.localPosition.z < 0.5f)
        {
            attackArea.localPosition = new Vector3(attackArea.localPosition.x, attackArea.localPosition.y, 0.5f);
        }

        return true;

    }

    public bool MoveToTarget(Vector3 Target)
    {
        if (Target == null)
        {
            return false;
        }

        attackArea.transform.position = Vector3.Lerp(attackArea.transform.position, Target, 1.5f*Time.deltaTime);
        attackArea.transform.position = new Vector3(attackArea.transform.position.x, 0.2f, attackArea.transform.position.z);
        return true;

    }


    private Vector3 GetCloestEnemy()
    {
        if (enemies.Count == 0)
            return Vector3.zero;

        float firstLength = Vector3.Distance(enemies[0].transform.position, transform.position);
        attackTarget = enemies[0].transform.position;

        foreach (var enemy in enemies)
        {
            float nowLength = Vector3.Distance(enemy.transform.position, transform.position);
            if (nowLength < firstLength)
            {
                firstLength = nowLength;
                attackTarget = enemy.transform.position;
            }
        }

        return attackTarget;
    }

}
