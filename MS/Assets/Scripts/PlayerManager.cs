using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{

    [SerializeField] float noramlWalkSpeed = 0.0f;
    [SerializeField] float attackingWalkSpeed = 0.0f;
    [SerializeField] float rotateSpeed = 1.0f;



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
    
    [Header("Dash CD UI Staff")]
    public Image dashCoolDownMask;


    Transform cameraTransform;
    Rigidbody rigidbody;
    PlayerSensor playerSensor;
    Collider collider;

    PlayerAttack playerAttack;


    [Header("Player Data Staff")]
    public PlayerData playerData;

    void Start()
    {
     
    }

    private void Awake()
    {

        rigidbody = GetComponent<Rigidbody>();
        playerSensor = GetComponent<PlayerSensor>();
        collider = GetComponent<Collider>();
        playerAttack = GetComponent<PlayerAttack>();

        playerData = CharacterSettings.Instance.playerData.GetCopy();

        cameraTransform = Camera.main.transform;    
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

        Debug.Log(playerAttackMovement);
    }

    public void GetInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            BonusData testBonus = BonusSettings.Instance.bonusDatas[1];
            ApplyBonus(testBonus);

            Interact();
        }
    }

    public void GetDashDown(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (Time.time > (lastDash + playerData.dashCooldown) && !playerAttack.isHold)
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
        }
    
        

        if (!playerAttack.afterShock)
        {
            Move();
            Rotate();
            CaculateInputDirection();
        }
       
    }


    private void Move() {
        targetSpeed = playerAttack.isHold ? attackingWalkSpeed:noramlWalkSpeed;
        targetSpeed *= moveInput.magnitude;

        if(currentSpeed > targetSpeed)
        {
            currentSpeed /= 2;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.3f * Time.deltaTime);
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
       // playerMovement = playerTransform.InverseTransformVector(playerMovementWorldSpace);
    }


    /// <summary>
    ///　
    /// </summary>
    private void Interact()
    {

        if (playerSensor.SensorCheck(transform, playerMovementWorldSpace,SENSORTYPE.INTERACT))
        {
            Debug.Log("act!");
        }

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
               
                // 入力の方向にダッシュ
                if (playerMovement.magnitude != 0)
                {
                    rigidbody.velocity = new Vector3(dashSpeed * playerMovement.x,
                                                0,
                                                dashSpeed * playerMovement.z);

                    Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
                    transform.rotation =  Quaternion.RotateTowards(transform.rotation, targetRotation,90);

                }
                // プレーヤーが向いている方向にダッシュ
                else
                {
                    rigidbody.velocity = new Vector3(dashSpeed * transform.forward.x,
                                               0,
                                               dashSpeed * transform.forward.z);
                }

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

            dashTimeLeft = playerData.dashTime;

            lastDash = Time.time;

            dashCoolDownMask.fillAmount = 1.0f;
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

}
