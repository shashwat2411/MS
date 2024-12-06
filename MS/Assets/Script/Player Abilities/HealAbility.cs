using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : PlayerAbility,IAtkEffect
{

    [SerializeField] float initeDeclineInterval = 5.0f;
    [SerializeField] static float nowDeclineInterval = 5.0f;
    float curTime = 0;


    protected override void Start()
    {
        base.Start();
        ResetLevel();

        transform.parent = player.transform;
    }
    // Update is called once per frame
    protected override void  FixedUpdate()
    {
        // TODO:: while battle
        if(true && player.playerData.hp < player.playerData.maxHp)
        {
            curTime += Time.deltaTime;
            if (curTime >= nowDeclineInterval)
            {
                Debug.Log("Heal Hp decline" + nowDeclineInterval);
                player.Heal();
                curTime = 0;
            }
        }
        else
        {
            curTime = 0.0f;
        }
    
       
    }

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {

    }

    public void LevelUp()
    {
        nowDeclineInterval -= 0.5f;
    }

    public void ResetLevel()
    {
        nowDeclineInterval = initeDeclineInterval;
    }

}
