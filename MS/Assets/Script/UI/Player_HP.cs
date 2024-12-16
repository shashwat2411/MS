using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    public float Hp_Now;
    public float Hp_Max;

    [SerializeField]
    GameObject RedBar, GreenBar;

    PlayerManager player;

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        GreenBar.GetComponent<Image>().fillAmount = 1;
        RedBar.GetComponent<Image>().fillAmount = 1;


        Hp_Max = 100.0f;
        Hp_Now = 100.0f;

        player = FindFirstObjectByType<PlayerManager>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) { Recover(20f); }


        

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (active == true) 
        {
            

            if (Hp_Now <= 0)
            {
                Hp_Now = 0;
            }
            //HPBarの処理
            //GreenBarの処理
            GreenBar.GetComponent<Image>().fillAmount = Hp_Now / Hp_Max;

            //ReDBarの処理
            if (RedBar.GetComponent<Image>().fillAmount > GreenBar.GetComponent<Image>().fillAmount)
            {
                RedBar.GetComponent<Image>().fillAmount -= 0.005f;
            }

            if (RedBar.GetComponent<Image>().fillAmount <= GreenBar.GetComponent<Image>().fillAmount)
            {
                RedBar.GetComponent<Image>().fillAmount = GreenBar.GetComponent<Image>().fillAmount;
            }
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
        else if(result <= 0f)
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

        if(effect == true)
        {
            GameObject recoveryEffect = Instantiate(player.playerHpRecoveryEffect.gameObject, player.transform);
            recoveryEffect.transform.localPosition = new Vector3(0f, 0.17f, 0f);
        }
    }




}
