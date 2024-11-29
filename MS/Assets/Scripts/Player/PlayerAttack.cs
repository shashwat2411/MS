using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttack : MonoBehaviour
{


    [Header("Close Range")]
    public float attackTime = 0.2f;
    public GameObject collider;

    [Header("謾ｻ謦・ｧｻ蜍慕ｯ・峇")]
    public float attackMoveRange;



    [HideInInspector] public float collisionDamage = 10f;



    GameObject bullet;
    private bool shoot = true;

    [Header("InputSystem")]
    public InputActionReference closeRangeAttack;
    public InputActionReference longRangeAttack;

    //
    float holdtime = 1.0f;
    public bool isHold = false;
    public bool afterShock = false;


    Transform attackArea;

    Vector3 initLocalPosition;

    PlayerManager playerManager;
    PlayerData playerData;
    public List<GameObject> multiBullets;
    public List<GameObject> enemies;

    Vector3 attackTarget;
    void Start()
    {

        #region Input
        closeRangeAttack.action.started += HoldAttack;



        closeRangeAttack.action.canceled += AttackFinish;

        #endregion

      
        playerManager = GetComponentInParent<PlayerManager>();
        playerData = playerManager.playerData;
        collider = GameObject.Instantiate(playerManager.playerPrefabs.attackArea,
                                          this.transform.position + (3.0f * this.transform.forward),
                                          this.transform.rotation,
                                          this.transform
                                          );
        
        attackArea = collider.transform;

        collider.GetComponent<MenkoAttack>().Initiate(playerManager.playerPrefabs.bullet);

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
                    new Vector3(playerData.maxAimSize / holdtime, 0.2f, playerData.maxAimSize / holdtime) ;

                

            }
            //MoveToTarget(attackTarget);
           // GetCloestEnemy();
        }
    }

    /// <summary>
    /// triggerを放したら、めんこ発射
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
            //GetCloestEnemy();
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

        collider.GetComponent<Transform>().localScale =new Vector3( playerData.maxAimSize,0.05f, playerData.maxAimSize);
        collider.GetComponent<Transform>().localPosition = initLocalPosition;
        collider.GetComponent<MeshRenderer>().enabled = false;
        //collider.GetComponent<SphereCollider>().enabled = false;
    }

   

    void ResetCooldown()
    {
        
        shoot = true;
    }

  

    /// <summary>
    ///  繧√ｓ縺鍋函謌・
    /// </summary>
    void IniteMenko()
    {
       
        Vector3 startPoint = this.transform.position + Vector3.up * 1.0f;

        Vector3 endPoint = new Vector3(collider.transform.position.x,0.0f, collider.transform.position.z);
        float offset = 1.5f;
        if (holdtime < 3.5f)
        {
            endPoint = GetRandomAttackPos(collider.transform.position, offset);
        }


        //Debug.Log(holdtime +"  " + endPoint );

        Vector3 dir = endPoint - startPoint ;
        dir.Normalize();



        collider.GetComponent<MenkoAttack>().IniteMultiMenko(startPoint, collider.transform, playerData.maxAttackSize, playerData.attack , holdtime);

        ////Instantiate(bullet, startPoint, collider.transform.rotation).GetComponent<Bullet>().Initiate(dir, playerData.attack);
        //var obj = ObjectPool.Instance.Get(playerManager.playerPrefabs.bullet, startPoint, collider.transform.rotation);
        //obj.GetComponent<BulletBase>().Initiate(dir, endPoint,playerData.maxAttackSize,playerData.attack * holdtime);


        //foreach(GameObject b in MenkoAttack.bullets)
        //{
        //    obj = ObjectPool.Instance.Get(b, startPoint, collider.transform.rotation);

        //    var multiEndPos = GetRandomAttackPos(endPoint, 10.0f);
        //    dir = multiEndPos - startPoint;
        //    dir.Normalize();

        //    obj.GetComponent<BulletBase>().Initiate(dir, multiEndPos, playerData.maxAttackSize, playerData.attack * holdtime);

        //}

    }

    Vector3 GetRandomAttackPos(Vector3 initPos,float offset)
    {
       Vector3 endPoint = initPos +
               new Vector3(
                            Random.Range(-offset / holdtime, offset / holdtime),
                            0.0f,
                            Random.Range(-offset / holdtime, offset / holdtime)
                        );

        return endPoint;    
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
