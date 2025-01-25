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
    public ParticleSystem dashEffectPref;

    bool doubleDash = false;
    bool doubleDashReady = false;
    float dashCount;
    float dashCountMax = 1;

    public bool isDashing { get; private set; } = false;

    Vector3 dashOrientation;

    [Header("Dash Incibility")]
    public bool dashIncibility;
    [SerializeField]
    public float invincibilityTimeLeft;


 

    [Header("Double Dash Interval")]
    [SerializeField]
    float secondDashIntervalTime =0.5f;
    [SerializeField]
    float secondDashIntervalTimeLeft;



    [Header("Dash CD UI Staff")]
    public Image dashCoolDownMask;
    public Image doubleDashCoolDownMask;


     string nameSE;

    PlayerData playerData;
    PlayerManager playerManager;
    Rigidbody rb;
    Animator animator;


    public void GetDashDown(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (doubleDashReady && dashCount == 1)
            {
                doubleDash = true;
            }
            else if (Time.time > (lastDash + playerData.dashCooldown) && !playerManager.playerAttack.throwAnimPlay)
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
        animator = GetComponent<Animator>();    
        playerData =playerManager.playerData;
        nameSE = playerManager.dashSE;
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
            
            dashEffectPref.Stop();
            //二回目のダッシュ
            if (doubleDash)
            {
                doubleDash = false;
                ReadyToDash();
            }
            //ダッシュ完全終了
            else
            {
                
                dashCoolDownMask.fillAmount += 1.0f / playerData.dashCooldown * Time.deltaTime;
                doubleDashCoolDownMask.fillAmount += 1.0f / playerData.dashCooldown * Time.deltaTime;
            }
        }


        if (doubleDashReady)
        {
            secondDashIntervalTimeLeft -= Time.deltaTime;

            doubleDashCoolDownMask.fillAmount -= 1f / secondDashIntervalTime * Time.deltaTime;

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
        if (dashCount > 0)
        {
            isDashing = true;
            

            dashTimeLeft = playerData.dashTime;
            

            invincibilityTimeLeft = 
                (playerData.dashInvincibilityTime > playerData.dashTime)? 
                    playerData.dashInvincibilityTime : playerData.dashTime;
            dashIncibility = true;

            lastDash = Time.time + playerData.dashTime;

            //dashCoolDownMask.fillAmount = 0.0f;
            StartCoroutine(DashDown(0.5f));

            dashCount--;

            dashEffectPref.Play();
            SoundManager.Instance.PlaySE(nameSE);

            CalculateAndTurnDir();
        }
        else
        {
            Debug.Log("Cant Dash");
        }

    }
    private IEnumerator DashDown(float duration)
    {
        float elapsed = 0f;

        float amount = dashCoolDownMask.fillAmount;
        float amount2 = 1f;

        if (dashCount <= 0) { amount2 = dashCoolDownMask.fillAmount; }

        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            dashCoolDownMask.fillAmount = elapsed / duration * amount;
            if (dashCount <= 0) { doubleDashCoolDownMask.fillAmount = elapsed / duration * amount2; }
            yield return null;
        }

        dashCoolDownMask.fillAmount = 0f;
        if (dashCount <= 0) { doubleDashCoolDownMask.fillAmount = 0f; }
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
        if(dashCountMax < 2)
        {
            dashCountMax++;
        }
       
        
    }




}
