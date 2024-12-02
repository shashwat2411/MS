using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerManager;


public class PlayerManager : MonoBehaviour
{
    public enum LocomotionState
    {
        Idle,
        Run,
        Walk
    };


    public enum PlayerState
    {
        Normal,
        Aim
    }

    LocomotionState locomotionState = LocomotionState.Idle;

    [SerializeField] float noramlRunSpeed = 0.0f;
    [SerializeField] float ChargeRunSpeed = 0.0f;
    [SerializeField] float rotateSpeed = 1.0f;


    bool lockMovement = false;

    [SerializeField]
    float currentSpeed;
    float targetSpeed;

    #region 入力値
    Vector2 moveInput;

    public Vector3 playerMovement { get; private set; }
    Vector3 playerAttackMovement;
    public Vector3 playerMovementWorldSpace;
    #endregion



    [HideInInspector] public Player_HP playerHP;
    [HideInInspector] public PlayerExp playerExp;

    [HideInInspector] public bool invincibility = false;
    [HideInInspector] public bool hurtInvincibility = false;
    [HideInInspector] public float hurtInvincibilityTimeLeft;

    static List<GameObject> sp = new List<GameObject>();

    Transform cameraTransform;
    Rigidbody rigidbody;
    PlayerSensor playerSensor;
    Collider collider;

    PlayerAttack playerAttack;
    PlayerMpAttack playerMpAttack;
    PlayerDash playerDash;


    //Bonus
    GameObject BonusMenu;

    [SerializeField]
    GameObject playerAblities;

    [Header("Player Data Staff")]
    public PlayerData playerData;
    [Header("Player Prefabs Staff")]
    public PlayerPrefabs playerPrefabs;
    public ParticleSystem playerDamageEffect;



    #region Animator StateMachine Hash
    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int aimHash;

    #endregion

    Animator animator;

    BonusItem bonusItem;
    void Start()
    {
        BonusMenu = GameObject.Find("BonusSelect");
     
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerSensor = GetComponent<PlayerSensor>();
        collider = GetComponent<Collider>();
        playerAttack = GetComponentInChildren<PlayerAttack>();
        playerMpAttack = GetComponent<PlayerMpAttack>();    
        playerDash = GetComponent<PlayerDash>();    

        playerData = CharacterSettings.Instance.playerData.GetCopy();
        playerPrefabs = CharacterSettings.Instance.playerPrefabs.GetCopy();

        playerPrefabs[PlayerPrafabType.playerPermanentAblity] =
                        ObjectPool.Instance.Get(playerAblities, new Vector3(0.0f, -5.0f, 0.0f), transform.rotation);

        cameraTransform = Camera.main.transform;

       #region Animator setting
        postureHash = Animator.StringToHash("Posture");
        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        turnSpeedHash = Animator.StringToHash("RotateSpeed");
        aimHash = Animator.StringToHash("Aim");
        animator.SetFloat("ScaleFactor",0.5f/animator.humanScale);

       #endregion


        playerHP = FindFirstObjectByType<Player_HP>();
        playerExp = FindFirstObjectByType<PlayerExp>();

    }


    #region 入力 Stuff
    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        playerMovement = new Vector3(moveInput.x, 0.0f, moveInput.y);   
    }


    public void GetAttackMoveInput(InputAction.CallbackContext ctx)
    {
        var input = ctx.ReadValue<Vector2>();
        playerAttackMovement.x = input.x;
        playerAttackMovement.z = input.y;

       // Debug.Log(playerAttackMovement);
    }

    public void GetInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            Interact();
        }
    }

    public void GetActionChange(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && playerAttack.isHold)
        {
            lockMovement = !lockMovement;
        }
    }

    public void GetMpAttackPressed(InputAction.CallbackContext context)
    {
        if(!playerAttack.isHold 
            && !playerAttack.afterShock 
            && !playerDash.isDashing)
        {
            playerMpAttack.MpAttackReady();
        }
        
    }

    #endregion

    private void FixedUpdate()
    {

        playerDash.Dash();
        if (playerDash.isDashing)
            return;

        if (playerAttack.isHold)
        {
            playerAttack.RangeMove(playerAttackMovement);
            //playerAttack.MoveToTarget(GetCloestEnemy());

        }
        else
        {
            lockMovement = false;
        }


        HurtInvincibleCheck();
        invincibility = playerDash.dashIncibility || hurtInvincibility;

        CaculateInputDirection();
        SwitchPlayerStates();
        SetAnimator();
     
       
    }



    void SwitchPlayerStates()
    {

   
        if (moveInput.magnitude == 0 || lockMovement)
        {
            locomotionState = LocomotionState.Idle;
        }else
        {
            locomotionState = LocomotionState.Run;
        }

      
    }


    private void Move() {

        targetSpeed = playerAttack.isHold ? ChargeRunSpeed:noramlRunSpeed;
        targetSpeed *= moveInput.magnitude;

        if(currentSpeed > targetSpeed)
        {
            currentSpeed /= 2;
        }
       

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 2.0f*Time.deltaTime);
        rigidbody.velocity = new Vector3((playerMovement * currentSpeed).x, rigidbody.velocity.y, (playerMovement * currentSpeed).z);
    }

    private void Rotate()
    {
      if(moveInput.Equals(Vector2.zero))
          return;

    

        Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed*Time.deltaTime);
    }


    void CaculateInputDirection()
    {
        Vector3 camForwardProjection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        playerMovementWorldSpace = camForwardProjection * moveInput.y + cameraTransform.right * moveInput.x;
        playerMovementWorldSpace = transform.InverseTransformVector(playerMovementWorldSpace);
        //Debug.Log(moveInput + " " + test);

    }


    /// <summary>
    ///　プレイヤーとのインタラクションのための機能。現在は報酬のテストに使用
    /// </summary>
    private void Interact()
    {
        // playerPrefabs.ApplyReplace(BonusSettings.Instance.replaceDatas[0]);
        
        playerPrefabs.GetTopItemBonus(BonusSettings.Instance.playerBonusItems[4]);
        playerPrefabs.GetTopItemBonus(BonusSettings.Instance.playerBonusItems[0]);

        //if (playerSensor.SensorCheck(transform, playerMovementWorldSpace,SENSORTYPE.INTERACT))
        //{
            
        //    playerPrefabs.ApplyReplace(BonusSettings.Instance.replaceDatas[0]);
        //    Debug.Log("act!");
        //}

    }

 


    public void Damage()
    {

        hurtInvincibility = true;
        hurtInvincibilityTimeLeft = playerData.hurtInvincibilityTime;
       

        Instantiate(playerDamageEffect.gameObject, transform.position, transform.rotation);
        //StartCoroutine(Camera.main.gameObject.GetComponent<GameEffects>().HitStop(0.3f));

    }
    public void Death()
    {
    }

    public void CheckPlayerDataState()
    {
        playerData.hp = (playerData.hp >= playerData.maxHp) ? playerData.maxHp : playerData.hp;
        playerData.mp = (playerData.mp >= playerData.maxMp) ? playerData.maxMp : playerData.mp;

    }

    public void HurtInvincibleCheck()
    {

        if (hurtInvincibility)
        {
            hurtInvincibilityTimeLeft -= Time.deltaTime;
            hurtInvincibility = (hurtInvincibilityTimeLeft <= 0) ? false : true;

        }

    }


    void LevelUp()
    {
        playerData.lv++;
        if((playerData.lv %5) == 0)
        {
            playerData.nextExp *= 1.2f;
        }

        //Bonus Menu
        BonusMenu.SetActive(true);

    }

    /// <summary>
    /// レベルアップ時のボーナス
    /// </summary>
    /// <param name="bd"></param>
    public void ApplyBonus(BonusData bd)
    {
        playerData.ApplyBonus(bd);
    }

    /// <summary>
    /// 経験値を与える
    /// </summary>
    /// <param name="exp"></param>
    public void ApplyExp(float exp)
    {
        var toNextLeft = playerData.nextExp - playerData.exp - exp;
        // 過ごした経験値
        if(toNextLeft <= 0)
        {
            LevelUp();
            playerData.exp = -toNextLeft;
        }
        else
        {
            playerData.exp = playerData.exp + exp;
        }
       
    }


    void SetAnimator()
    {
        if (playerAttack.afterShock)
        {
            animator.SetFloat(postureHash, 2f, 1.0f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(postureHash, 1f, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovementWorldSpace.magnitude * noramlRunSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovementWorldSpace.magnitude * noramlRunSpeed, 0.1f, Time.deltaTime);
                    break;
            }

        }

        // attacking layer
        animator.SetBool(aimHash, playerAttack.isHold);

        if (!playerDash.isDashing)
        {
            // Rotate
            float rad = Mathf.Atan2(playerMovementWorldSpace.x, playerMovementWorldSpace.z);
            animator.SetFloat(turnSpeedHash, rad, 0.5f, Time.deltaTime);
            transform.Rotate(0, rad * rotateSpeed * Time.deltaTime, 0f);
        }
    }


    private void OnAnimatorMove()
    {
        if (playerAttack.afterShock)
        {
            rigidbody.velocity = Vector3.zero;
        }
        else if (!playerDash.isDashing)
        {
            rigidbody.velocity = animator.velocity;
         //   Debug.Log(animator.velocity);
        }
    }

    public Vector2 GetMovementInput()
    {
        return moveInput;
    }
   
}
