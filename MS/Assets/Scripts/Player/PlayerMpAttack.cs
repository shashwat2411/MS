using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMpAttack : MonoBehaviour
{
    public float mpConsumption = 30.0f;
    public InputActionReference mpAttack;

    GameObject mpAttackArea;


    public float mpAttackTime = 2.0f;

    public float lastAttack = 0;
    public float mpAttackCoolDown = 5.0f;
    public float mpAttackCoolDownLeft = 5.0f;

    bool mpAttackReady;
    public bool isMpAttacking;

    PlayerManager playerManager;
    PlayerData playerData;

    void Start()
    {
        lastAttack = 0;

        playerManager = GetComponentInParent<PlayerManager>();
        playerData = playerManager.playerData;
    }

    void FixedUpdate()
    {
       if(mpAttackReady)
       {
           MpAttack();
       }

        mpAttackCoolDownLeft -= Time.deltaTime;
    }

    public void MpAttackReady()
    {
        if (Time.time > (lastAttack + mpAttackCoolDown))
        {
            mpAttackReady = true;
        }
    }

    void MpAttack()
    {
        float resMp = 50f;//playerData.mp - mpConsumption;
        if (resMp >= 0)
        {
            playerData.mp = resMp;
        }
        else
        {
            Debug.Log("low mp");
            return;
        }

        mpAttackReady = false;
        isMpAttacking = true;
        mpAttackArea = ObjectPool.Instance.Get(playerManager.playerPrefabs.mpAttackArea, transform.position, transform.rotation);

        mpAttackArea.GetComponentInChildren<KnockBack>().Initiate(0f, 0f);

        mpAttackCoolDownLeft = mpAttackCoolDown;

        Invoke("ResetAttackArea", mpAttackTime);

        lastAttack = Time.time;
    }

    private void ResetAttackArea()
    {
        isMpAttacking = false;

        //if (mpAttackArea != null)
        //{
        //    ObjectPool.Instance.Push(mpAttackArea);
        //}
    }
}
