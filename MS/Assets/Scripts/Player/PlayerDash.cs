using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{


    [Header("Dash Staff")]
    private float dashTimeLeft = 1.0f;
    private float lastDash=0;
    public float dashSpeed;
    public GameObject dashEffectPref;
    GameObject dashEffect;

    Vector3 dashOrientation;

    public bool dashIncibility;
    public float invincibilityTimeLeft;


    bool doubleDash= false;
    bool doubleDashReady= false;
    float dashCount;
    float dashCountMax = 1;
    
    float secondDashIntervalTime =0.5f;
    float secondDashIntervalTimeLeft;


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
            if (doubleDashReady && dashCount == 1)
            {
              
                doubleDash = true;
            }
            else if (Time.time > (lastDash + playerData.dashCooldown))
            {
                dashCount = dashCountMax;
                ReadyToDash();
                SecondDashReady();
              
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
            //ダッシュ中
            if (dashTimeLeft > 0)
            {
                rb.velocity = dashOrientation;
              

                dashTimeLeft -= Time.deltaTime;

                invincibilityTimeLeft -= Time.deltaTime;

                dashIncibility = (invincibilityTimeLeft <= 0)?false:true;
               
            }
            else
            {
                isDashing = false;
            }
        }
        //ダッシュ終了
        else
        {
            dashEffectPref.GetComponent<ParticleSystem>().Stop();
            //二回目のダッシュ
            if (doubleDash)
            {
                doubleDash = false;
                ReadyToDash();
            }
            //ダッシュ完全終了
            else
            {
                
              
                dashCoolDownMask.fillAmount -= 1.0f / playerData.dashCooldown * Time.deltaTime;
            }
        }


        if (doubleDashReady)
        {
            secondDashIntervalTimeLeft -= Time.deltaTime;
            if (secondDashIntervalTimeLeft < 0)
            {
                doubleDashReady = false;
            }
        }
       


    }

    void SecondDashReady()
    {
        if(dashCountMax >=2)
        {

            doubleDashReady = true;
            secondDashIntervalTimeLeft = secondDashIntervalTime;
        }
        
    }
    void ReadyToDash()
    {
        // TODO:地形の範囲のチェック
        if (dashCount>0)
        {
            isDashing = true;


            dashTimeLeft = playerData.dashTime;
            

            invincibilityTimeLeft = 
                (playerData.dashInvincibilityTime > playerData.dashTime)? 
                    playerData.dashInvincibilityTime : playerData.dashTime;
            dashIncibility = true;

            lastDash = Time.time + playerData.dashTime;

            dashCoolDownMask.fillAmount = 1.0f;

            dashCount--;

            dashEffectPref.GetComponent<ParticleSystem>().Play();

            CalculateAndTurnDir();
        }
        else
        {
            Debug.Log("Cant Dash");
        }

    }


    void CalculateAndTurnDir()
    {
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
        if (playerManager.playerMovement.magnitude != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerManager.playerMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180);
        }

    }

    public void LevelUp()
    {
        dashCountMax++;
        
    }




}
