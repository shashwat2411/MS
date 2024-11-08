using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] float walkSpeed = 0.0f;
    [SerializeField] float rotateSpeed = 1.0f;



    float currentSpeed;
    float targetSpeed;

    #region ���͒l
    Vector2 moveInput;


    #endregion


    Vector3 playerMovement;
    Vector3 playerMovementWorldSpace;


    
    [Header("Dash Staff")]
    [SerializeField]
    private bool isDashing = false;
    //public float dashTime = 1.0f;
    private float dashTimeLeft = 1.0f;
    //public float dashCooldown;
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
    public int lv;
    public float nowExp = 0;


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


    #region ���� Stuff
    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        playerMovement.x = moveInput.x;
        playerMovement.z = moveInput.y;
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
            if (Time.time > (lastDash + playerData.dashCooldown))
            {
                ReadyToDash();
            }
        }
    }

    #endregion
    private void FixedUpdate()
    {
        if(!playerAttack.isHold)
        {
            Dash();
            if (isDashing)
                return;
            CaculateInputDirection();
            Move();
            Rotate();

        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            playerAttack.RangeMove(playerMovement);
        }

    
    }


    
    private void Move() { 
        targetSpeed = walkSpeed;
        targetSpeed *= moveInput.magnitude;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.5f*Time.deltaTime);
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
    ///�@
    /// </summary>
    private void Interact()
    {

        if (playerSensor.SensorCheck(transform, playerMovementWorldSpace,SENSORTYPE.INTERACT))
        {
            Debug.Log("act!");
        }

    }

    /// <summary>
    ///�@�_�b�V��
    /// </summary>
    void Dash()
    {

        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
               
                // ���͂̕����Ƀ_�b�V��
                if (playerMovement.magnitude != 0)
                {
                    rigidbody.velocity = new Vector3(dashSpeed * playerMovement.x,
                                                0,
                                                dashSpeed * playerMovement.z);

                    Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
                    transform.rotation =  Quaternion.RotateTowards(transform.rotation, targetRotation,90);

                }
                // �v���[���[�������Ă�������Ƀ_�b�V��
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
        // TODO:�n�`�͈̔͂̃`�F�b�N
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

    /// <summary>
    /// ���x���A�b�v���̃{�[�i�X
    /// </summary>
    /// <param name="bd"></param>
    public void ApplyBonus(BonusData bd)
    {
        playerData.ApplyBonus(bd);
    }
}
