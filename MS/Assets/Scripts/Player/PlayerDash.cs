using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{


    [Header("Dash Staff")]
    private float dashTimeLeft = 1.0f;
    private float lastDash;
    public float dashSpeed;
    bool dashOrientationFlag;
    Vector3 dashOrientation;
    public bool isDashing { get; private set; } = false;
    [Header("Dash CD UI Staff")]
    public Image dashCoolDownMask;


    PlayerData playerData;
    PlayerManager playerManager;

    Rigidbody rb;



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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();  
        playerManager = GetComponent<PlayerManager>();  
        playerData =playerManager.playerData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    ///　ダッシュ
    /// </summary>
   public void Dash()
    {

        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = dashOrientation;
                if (playerManager.playerMovement.magnitude != 0)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(playerManager.playerMovement, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180);
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

            dashOrientationFlag = true;

            dashTimeLeft = playerData.dashTime;

            lastDash = Time.time;

            dashCoolDownMask.fillAmount = 1.0f;


            //ダッシュ方向計算
            if (playerManager.playerMovement.magnitude != 0)
            {
                dashOrientation = new Vector3(dashSpeed * playerManager.playerMovement.x,
                                            0,
                                            dashSpeed * playerManager.playerMovement.z);
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

}
