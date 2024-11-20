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


    [SerializeField]
    float currentSpeed;
    float targetSpeed;

    #region 入力値
    Vector2 moveInput;

    Vector3 playerMovement;
    Vector3 playerAttackMovement;
    Vector3 playerMovementWorldSpace;
    #endregion



    [SerializeField] public Player_HP playerHP;



    [Header("Dash Staff")]
    [SerializeField]
    private bool isDashing = false;
    private float dashTimeLeft = 1.0f;
    private float lastDash;
    public float dashSpeed;
    bool dashOrientationFlag;
    Vector3 dashOrientation;



    [Header("Dash CD UI Staff")]
    public Image dashCoolDownMask;


    Transform cameraTransform;
    Rigidbody rigidbody;
    PlayerSensor playerSensor;
    Collider collider;

    PlayerAttack playerAttack;


    [Header("Player Data Staff")]
    public PlayerData playerData;
    [Header("Player Prefabs Staff")]
    public PlayerPrefabs playerPrefabs;



    #region Animator StateMachine Hash
    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int aimHash;

    #endregion

    Animator animator;
  

    void Start()
    {
     
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerSensor = GetComponent<PlayerSensor>();
        collider = GetComponent<Collider>();
        playerAttack = GetComponent<PlayerAttack>();

        playerData = CharacterSettings.Instance.playerData.GetCopy();
        playerPrefabs = CharacterSettings.Instance.playerPrefabs.GetCopy();



        cameraTransform = Camera.main.transform;


        postureHash = Animator.StringToHash("Posture");
        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        turnSpeedHash = Animator.StringToHash("RotateSpeed");
        aimHash = Animator.StringToHash("Aim");
        animator.SetFloat("ScaleFactor",0.5f/animator.humanScale);

    }


    #region 入力 Stuff
    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        playerMovement.x = moveInput.x;
        playerMovement.z = moveInput.y;
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

    public void GetDashDown(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (Time.time > (lastDash + playerData.dashCooldown) )
            {
                ReadyToDash();
            }
        }
    }

    #endregion

    private void FixedUpdate()
    {
  
        Dash();
        if (isDashing)
            return;

        if (playerAttack.isHold)
        {
            playerAttack.RangeMove(playerAttackMovement);
            //playerAttack.MoveToTarget(GetCloestEnemy());
           
        }
    
        

       
        CaculateInputDirection();
        SwitchPlayerStates();
        SetAnimator();
     
       
    }

    void SwitchPlayerStates()
    {

   
        if (moveInput.magnitude == 0)
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

        playerPrefabs.ApplyBonus(BonusSettings.Instance.bonusItems[0]);
        //if (playerSensor.SensorCheck(transform, playerMovementWorldSpace,SENSORTYPE.INTERACT))
        //{
            
        //    playerPrefabs.ApplyReplace(BonusSettings.Instance.replaceDatas[0]);
        //    Debug.Log("act!");
        //}

    }

    /// <summary>
    ///　ダッシュ
    /// </summary>
    void Dash()
    {

        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                rigidbody.velocity = dashOrientation;
                Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180);
                       
                dashTimeLeft -= Time.deltaTime;


            }
            else
            {
                isDashing = false;
            
            }
        }


        dashCoolDownMask.fillAmount -= 1.0f / playerData.dashCooldown * Time.deltaTime;
    }


    void ReadyToDash()
    {
        // TODO:地形の範囲のチェック
        if (true)
        {
            isDashing = true;

            dashOrientationFlag = true;
            
            dashTimeLeft = playerData.dashTime;

            lastDash = Time.time;

            dashCoolDownMask.fillAmount = 1.0f;


            //ダッシュ方向計算
            if (playerMovement.magnitude != 0)
            {
                dashOrientation = new Vector3(dashSpeed * playerMovement.x,
                                            0,
                                            dashSpeed * playerMovement.z);
            }
            // プレーヤーが向いている方向にダッシュ
            else
            {
                dashOrientation = new Vector3(dashSpeed * transform.forward.x,
                                           0,
                                           dashSpeed * transform.forward.z);
            }

        }
        else
        {
            Debug.Log("Cant Dash");
        }
       
    }

    public void Damage()
    {

    }
    public void Death()
    {
    }


    void LevelUp()
    {
        playerData.lv++;
        if((playerData.lv %5) == 0)
        {
            playerData.nextExp *= 1.2f;
        }

        //Bonus Menu


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

        if (!isDashing)
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
            rigidbody.velocity =Vector3.zero;
        }
        else if (!isDashing)
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
