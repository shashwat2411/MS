using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttack : MonoBehaviour
{
    [Header("Close Range")]
    public float attackTime = 0.1f;
    public GameObject collider;

    [Header("ダメージ時間")]
    public float damage = 10f;
    [Header("最大チャージ時間")]
    public float maxChargeTime = 2.5f;
    [Header("最大攻撃範囲")]
    public Vector3 maxAttackSize = Vector3.one;
    [Header("チャージ速度")]
    public float chargeSpeed = 1.0f;
    [Header("攻撃移動範囲")]
    public float attackMoveRange;




    [Header("Long Range")]
    public float cooldown = 0.1f;
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

    
    void Start()
    {

        #region Input
        closeRangeAttack.action.started += HoldAttack;
        longRangeAttack.action.started += LongRangeAttack;



        closeRangeAttack.action.canceled += AttackFinish;

        #endregion



        attackArea = GetComponentsInChildren<Transform>()[1];
        initLocalPosition = attackArea.localPosition;



        ResetCollider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isHold)
        {
            if (holdtime < maxChargeTime)
            {

                holdtime += Time.deltaTime * chargeSpeed;
          
                collider.GetComponent<Transform>().localScale =
                    new Vector3(maxAttackSize.x / holdtime, maxAttackSize.y, maxAttackSize.z / holdtime) ;
                    

            }
        }

      
    }


    void AttackFinish(InputAction.CallbackContext context)
    {

        Debug.Log("cancel");
        IniteMenko();

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
        isHold = false;
        holdtime = 1.0f;

        collider.GetComponent<Transform>().localScale = maxAttackSize;
        collider.GetComponent<Transform>().localPosition = initLocalPosition;
        collider.GetComponent<MeshRenderer>().enabled = false;
        collider.GetComponent<SphereCollider>().enabled = false;
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

    /// <summary>
    ///  めんこ生成
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

        Instantiate(bullet, startPoint, collider.transform.rotation).GetComponent<Bullet>().Initiate(dir);

    }

    public bool RangeMove(Vector3 playerInput)
    {


      var newPos = new Vector3(
                                attackArea.localPosition.x + playerInput.x * Time.deltaTime * 3.0f,
                                attackArea.localPosition.y,
                                attackArea.localPosition.z + playerInput.z * Time.deltaTime * 3.0f
                                );

        var offset = newPos - Vector3.zero;

        attackArea.localPosition = Vector3.zero + Vector3.ClampMagnitude(offset, attackMoveRange);


        if (attackArea.localPosition.z <0.5f)
        {
            attackArea.localPosition = new Vector3(attackArea.localPosition.x, attackArea.localPosition.y, 0.5f);
        }                                              
                                                     



        return true;

    }
}
