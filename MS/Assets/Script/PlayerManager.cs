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
    public float dashTime = 1.0f;
    private float dashTimeLeft = 1.0f;
    public float dashCooldown;
    private float lastDash;
    public float dashSpeed;
    
    [Header("Dash CD UI Staff")]
    public Image dashCoolDownMask;


    Transform cameraTransform;
    Rigidbody rigidbody;
    PlayerSensor playerSensor;
    Collider collider;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerSensor = GetComponent<PlayerSensor>();
        collider = GetComponent<Collider>();
    }

    private void Awake()
    {
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
            Interact();
        }
    }

    public void GetDashDown(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (Time.time > (lastDash + dashCooldown))
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
        CaculateInputDirection();
        Move();
        Rotate();
       
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

        if (playerSensor.SensorCheck(transform, playerMovementWorldSpace))
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
                collider.enabled = false;
                // ���͂̕����Ƀ_�b�V��
                if (playerMovement.magnitude != 0)
                {
                    rigidbody.velocity = new Vector3(dashSpeed * playerMovement.x,
                                                0,
                                                dashSpeed * playerMovement.z);
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
                collider.enabled = true;
            }
        }


        dashCoolDownMask.fillAmount -= 1.0f / dashCooldown * Time.deltaTime;
    }


    void ReadyToDash()
    {
        // TODO:�n�`�͈̔͂̃`�F�b�N
        if (true)
        {
            isDashing = true;

            dashTimeLeft = dashTime;

            lastDash = Time.time;

            dashCoolDownMask.fillAmount = 1.0f; 
        }
       
    }
}
