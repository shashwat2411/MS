using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarManager : MonoBehaviour
{
    [SerializeField]
    Player_HP Hpbar1;

    [SerializeField]
    Player_HP Hpbar2;

    PlayerManager player;

    float Hp_Max;
    float Hp_Now;

    [SerializeField, Range(0.0f, 100.0f)]
    float MaxScale;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();

        Hp_Max = player.playerData.maxHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Hp_Max = player.playerData.maxHp;
        Hp_Now = player.playerData.hp;

        SetPlayerHP();
    }

    public void SetPlayerHP()
    {

        if (Hp_Max > 100)
        {
            Hpbar1.Hp_Max = 100.0f;

            Hpbar2.Hp_Max = Hp_Max - 100.0f;

            Hpbar2.gameObject.SetActive(true);

            SetSecondHPLength();
        }
        else
        {
            Hpbar1.Hp_Max = Hp_Max;

            Hpbar2.Hp_Max = 0.0f;

            Hpbar2.gameObject.SetActive(false);
        }

        if (Hp_Now > 100)
        {
            Hpbar1.Hp_Now = 100.0f;
                   
            Hpbar2.Hp_Now = Hp_Now - 100.0f;

            Hpbar1.active = false;
            Hpbar2.active = true;
        }
        else
        {            
            Hpbar1.Hp_Now = Hp_Now;
                  
            Hpbar2.Hp_Now = 0.0f;

            Hpbar1.active = true;
            Hpbar2.active = false;
        }

    }

    void SetSecondHPLength()
    {
        float len;

        len = 0.7f / 100.0f * (Hp_Max - 100.0f);

        Hpbar2.transform.localScale = new Vector3(len, Hpbar2.transform.localScale.y);
    }

    public void Damage(float value)
    {
        if (player.invincibility)
        {
            return;
        }

        float result = Hp_Now - value;
        if (result > 0f)
        {
            Hp_Now = result;
            player.Damage();
        }
        else if (result <= 0f)
        {
            Hp_Now = 0f;
            player.Death();
        }

        player.playerData.hp = Hp_Now;
    }

    public void Recover(float value, bool effect = true)
    {
        float result = Hp_Now + value;

        if (result <= Hp_Max)
        {
            Hp_Now = result;
        }
        else if (result > Hp_Max)
        {
            Hp_Now = Hp_Max;
        }

        player.playerData.hp = Hp_Now;

        if (effect == true)
        {
            GameObject recoveryEffect = Instantiate(player.playerHpRecoveryEffect.gameObject, player.transform);
            recoveryEffect.transform.localPosition = new Vector3(0f, 0.17f, 0f);
        }
    }
}
