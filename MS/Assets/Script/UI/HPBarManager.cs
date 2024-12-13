using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarManager : MonoBehaviour
{
    [SerializeField]
    GameObject Hpbar1;

    [SerializeField]
    GameObject Hpbar2;

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
    void Update()
    {
        Hp_Max = player.playerData.maxHp;
        Hp_Now = player.playerData.hp;

        SetPlayerHP();


       
    }

    public void SetPlayerHP()
    {

        if (Hp_Max > 100)
        {
            Hpbar1.GetComponent<Player_HP>().Hp_Max = 100.0f;

            Hpbar2.GetComponent<Player_HP>().Hp_Max = Hp_Max - 100.0f;

            Hpbar2.SetActive(true);
        }
        else
        {
            Hpbar1.GetComponent<Player_HP>().Hp_Max = Hp_Max;

            Hpbar2.GetComponent<Player_HP>().Hp_Max = 0.0f;

            Hpbar2.SetActive(false);
        }

        if (Hp_Now > 100)
        {
            Hpbar1.GetComponent<Player_HP>().Hp_Now = 100.0f;
                        
            Hpbar2.GetComponent<Player_HP>().Hp_Now = Hp_Now - 100.0f;

            Hpbar1.GetComponent<Player_HP>().active = false;
            Hpbar2.GetComponent<Player_HP>().active = true;
        }
        else
        {            
            Hpbar1.GetComponent<Player_HP>().Hp_Now = Hp_Now;
                        
            Hpbar2.GetComponent<Player_HP>().Hp_Now = 0.0f;

            Hpbar1.GetComponent<Player_HP>().active = true;
            Hpbar2.GetComponent<Player_HP>().active = false;
        }

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

    public void Heal(float value)
    {

        Hp_Now = Hp_Now + value;

        player.playerData.hp = Hp_Now;
    }

}
