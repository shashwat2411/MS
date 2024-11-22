using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    [SerializeField] float Hp_Now;
    [SerializeField] float Hp_Max;

    [SerializeField]
    GameObject RedBar, GreenBar;

    PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        GreenBar.GetComponent<Image>().fillAmount = 1;
        RedBar.GetComponent<Image>().fillAmount = 1;


        Hp_Max = 100.0f;
        Hp_Now = 100.0f;

        player = FindFirstObjectByType<PlayerManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //現在のHPを取得
        Hp_Now = player.playerData.hp;
        Hp_Max = player.playerData.maxHp;

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

        if(RedBar.GetComponent<Image>().fillAmount <= GreenBar.GetComponent<Image>().fillAmount)
        {
            RedBar.GetComponent<Image>().fillAmount = GreenBar.GetComponent<Image>().fillAmount;
        }

    }

    public void Damage(float value)
    {
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


}
