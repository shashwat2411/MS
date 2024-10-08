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

    Vector2 moveInput;
    Vector3 playerMovement;

    Transform cameraTransform;
    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;    
    }

    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        playerMovement.x = moveInput.x;
        playerMovement.z = moveInput.y;

        //Debug.Log(playerMovement);
    }

    private void FixedUpdate()
    {
       
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



 
}
