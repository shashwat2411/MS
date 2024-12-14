using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public enum ChargePhase
{
    Entry,
    Low,
    Middle,
    High = 5,
    Max = 99
}

public class PlayerAttack : MonoBehaviour
{


    [Header("Close Range")]
    public float attackTime = 0.2f;
    public GameObject collider;
    private LockOnGrowth growth;

    [Header("")]
    public float attackMoveRange;


    [Header("Charge Phase")]
    [SerializeField]
    ChargePhase chargePhase = ChargePhase.Entry;

    [Range(0, 100)]
    public float lowRange;

    [Range(0, 100)]
    public float middleRange;

    [Range(0, 100)]
    public float highRange;

   

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
    public float attackRangeMoveFactor = 1.0f;

    Transform attackArea;

    Vector3 initLocalPosition;

    PlayerManager playerManager;
    PlayerData playerData;
    public List<GameObject> multiBullets;
    public List<GameObject> enemies;

    Vector3 attackTarget;

    public ParticleSystem chargeEffect;
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
        growth = collider.GetComponent<LockOnGrowth>();

        collider.GetComponent<MenkoAttack>().Initiate(playerManager.playerPrefabs.bullet);

        initLocalPosition = attackArea.localPosition;


        ResetCollider();

        chargeEffect.Stop();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHold)
        {
            if (holdtime < playerData.maxChargeTime)
            {

                holdtime += Time.deltaTime * playerData.chargeSpeed;

                //collider.GetComponent<Transform>().localScale = new Vector3(playerData.maxAimSize / holdtime, 0.2f, playerData.maxAimSize / holdtime);


                growth.growthValue = Mathf.Lerp(0f, 1f, (holdtime - 1f) / (playerData.maxChargeTime - 1f));


            }
            else
            {
                ParticleSystem.MainModule main = chargeEffect.main;
                main.loop = false;
            }
            //MoveToTarget(attackTarget);
            // GetCloestEnemy();
        }

        playerData.charge = holdtime;
        SwitchChargePhase();
    
    }


    void SwitchChargePhase()
    {
        if (isHold)
        {
            chargePhase = ChargePhase.Entry;
            if (holdtime > playerData.maxChargeTime * lowRange / 100.0f)
            {
                chargePhase = ChargePhase.Low;
            }
            if (holdtime > playerData.maxChargeTime * middleRange / 100.0f)
            {
                chargePhase = ChargePhase.Middle;
            }
            if (holdtime > playerData.maxChargeTime * highRange / 100.0f)
            {
                chargePhase = ChargePhase.High;
            }
            if(holdtime >= playerData.maxChargeTime)
            {
                chargePhase = ChargePhase.Max;
            }
        }
        
       
    }

    /// <summary>
    /// triggerÇï˙ÇµÇΩÇÁÅAÇﬂÇÒÇ±î≠éÀ
    /// </summary>
    /// <param name="context"></param>
    void AttackFinish(InputAction.CallbackContext context)
    {

        if (isHold && !afterShock)
        {
            chargeEffect.Stop();
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            IniteMenko();

            // Reset
            isHold = false;
            afterShock = true;
            holdtime = 1.0f;

            //growth.outerCircle.GetComponent<MeshRenderer>().enabled = false;
            //growth.innerCircle.gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(growth.Deactivate(growth.GetOuterMaterial()));
            StartCoroutine(growth.Deactivate(growth.GetInnerMaterial()));

            //collider.GetComponent<SphereCollider>().enabled = false;

            Invoke("ResetCollider", attackTime);

            ParticleSystem.MainModule main = chargeEffect.main;
            main.loop = false;
        }

    }


    void HoldAttack(InputAction.CallbackContext context)
    {
        if (!isHold && !afterShock)
        {
            chargeEffect.Play();
            isHold = true;

            //growth.outerCircle.GetComponent<MeshRenderer>().enabled = true;
            //growth.innerCircle.gameObject.GetComponent<MeshRenderer>().enabled = true;

            StartCoroutine(growth.Activate(growth.GetOuterMaterial()));
            StartCoroutine(growth.Activate(growth.GetInnerMaterial()));

            growth.SetInitialPosition(growth.GenerateRandomPosition());

            ParticleSystem.MainModule main = chargeEffect.main;
            main.loop = true;

            chargeEffect.Play();

            //GetCloestEnemy();
        }


    }


    void CloseRangeAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Close Range Attack");
        Invoke("ResetCollider", attackTime);

        //growth.outerCircle.GetComponent<MeshRenderer>().enabled = true;
        //growth.innerCircle.gameObject.GetComponent<MeshRenderer>().enabled = true;

        //growth.Activate(growth.GetOuterMaterial(), 0.1f);
        //growth.Activate(growth.GetInnerMaterial(), 0.1f);
        //collider.GetComponent<SphereCollider>().enabled = true;
    }


    //TODO:: To be called by menko attack animaton events
    void ResetCollider()
    {

        afterShock = false;
        holdtime = 1.0f;

        collider.GetComponent<Transform>().localScale = new Vector3(playerData.maxAimSize, playerData.maxAimSize, playerData.maxAimSize);
        collider.GetComponent<Transform>().localPosition = initLocalPosition;

        //StartCoroutine(growth.Deactivate(growth.GetOuterMaterial()));
        //StartCoroutine(growth.Deactivate(growth.GetInnerMaterial()));

        growth.innerCircle.localScale = new Vector3(growth.GetInitialRadius(), growth.innerCircle.localScale.y, growth.GetInitialRadius());
        //collider.GetComponent<SphereCollider>().enabled = false;
    }



    void ResetCooldown()
    {

        shoot = true;
    }



    /// <summary>
    ///  „ÇÅ„Çì„ÅìÁîüÊàÅE
    /// </summary>
    void IniteMenko()
    {

        //Vector3 startPoint = this.transform.position + Vector3.up * 1.0f;
        Vector3 startPoint = chargeEffect.transform.position;

        Vector3 endPoint = new Vector3(growth.innerCircle.position.x, 0.0f, growth.innerCircle.position.z);
        //float offset = 1.5f;
        //if (holdtime < 3.5f)
        //{
        //      endPoint = GetRandomAttackPos(collider.transform.position, offset);
        //}


        //Debug.Log(holdtime +"  " + endPoint );

        Vector3 dir = endPoint - startPoint;
        dir.Normalize();



        collider.GetComponent<MenkoAttack>().IniteMultiMenko(startPoint, growth.innerCircle, 
                                playerData.maxAttackSize, playerData.attack, holdtime,chargePhase);

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

    Vector3 GetRandomAttackPos(Vector3 initPos, float offset)
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
                                attackArea.position.x + playerInput.x * Time.deltaTime * playerData.atkMoveSpeed * attackRangeMoveFactor,
                                attackArea.position.y,
                                attackArea.position.z + playerInput.z * Time.deltaTime * playerData.atkMoveSpeed * attackRangeMoveFactor
                                );

        var localPoint = transform.InverseTransformPoint(newPos);
        attackArea.localPosition = Vector3.ClampMagnitude(localPoint, attackMoveRange);


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

        attackArea.transform.position = Vector3.Lerp(attackArea.transform.position, Target, 1.5f * Time.deltaTime);
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