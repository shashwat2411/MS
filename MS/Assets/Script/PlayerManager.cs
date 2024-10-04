using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float walkSpeed = 0.0f;
    [SerializeField] float rotateSpeed = 1.0f;



    float currentSpeed;
    float targetSpeed;

    #region ì¸óÕíl
    Vector2 moveInput;


    #endregion


    Vector3 playerMovement;
    Vector3 playerMovementWorldSpace;


    Transform cameraTransform;
    Rigidbody rigidbody;
    PlayerSensor playerSensor;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerSensor = GetComponent<PlayerSensor>();
    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;    
    }


    #region ì¸óÕ Stuff
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

    #endregion
    private void FixedUpdate()
    {
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
    ///Å@
    /// </summary>
    private void Interact()
    {

        if (playerSensor.SensorCheck(transform, playerMovementWorldSpace))
        {
            Debug.Log("act!");
        }

    }



}
