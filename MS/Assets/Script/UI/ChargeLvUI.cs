using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeLvUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] ChargeBar;

    Vector2 BarScale;

    PlayerManager player;

    public int chargeLv_now;
    int chargeLv_before;

    float lowRange, middleRange, highRange;

    bool LvUp;

    Animator anime;

    // HDRカラーピッカー
    [ColorUsage(true, true), SerializeField] private Color ColorLv1;
    [ColorUsage(true, true), SerializeField] private Color ColorLv2;
    [ColorUsage(true, true), SerializeField] private Color ColorLv3;


    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();

        player = player = FindFirstObjectByType<PlayerManager>();

        chargeLv_now = 0;
        chargeLv_before = 0;

        lowRange = player.playerAttack.lowRange;
        middleRange = player.playerAttack.middleRange;
        highRange = player.playerAttack.highRange;

        for(int i = 0; i < ChargeBar.Length; i++)
        {
            ChargeBar[i].GetComponent<Image>().fillAmount = 0;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.playerAttack.isHold == true)
        {

            if (player.playerData.charge <= player.playerData.maxChargeTime * lowRange / 100.0f) 
            {
                ChargeBar[0].GetComponent<Image>().color = ColorLv1;
                ChargeBar[0].GetComponent<Image>().fillAmount = player.playerData.charge / (player.playerData.maxChargeTime * lowRange / 100.0f);

            }
            if (player.playerData.charge > player.playerData.maxChargeTime * lowRange / 100.0f
                && player.playerData.charge <= player.playerData.maxChargeTime * middleRange / 100.0f) 
            {
                chargeLv_now = 1;
                ChargeBar[0].GetComponent<Image>().fillAmount = 1.0f;
                ChargeBar[1].GetComponent<Image>().color = ColorLv2;
                ChargeBar[1].GetComponent<Image>().fillAmount = (player.playerData.charge - (player.playerData.maxChargeTime * lowRange / 100.0f))
                    / (player.playerData.maxChargeTime * middleRange / 100.0f - player.playerData.maxChargeTime * lowRange / 100.0f);
            }
            if (player.playerData.charge <= player.playerData.maxChargeTime * highRange / 100.0f 
                && player.playerData.charge > player.playerData.maxChargeTime * middleRange / 100.0f)
            {
                chargeLv_now = 2;
                ChargeBar[1].GetComponent<Image>().fillAmount = 1.0f;
                ChargeBar[2].GetComponent<Image>().color = ColorLv3;
                ChargeBar[2].GetComponent<Image>().fillAmount = (player.playerData.charge - (player.playerData.maxChargeTime * middleRange / 100.0f))
                    / (player.playerData.maxChargeTime * highRange / 100.0f - player.playerData.maxChargeTime * middleRange / 100.0f);
            }
            if(player.playerData.charge > player.playerData.maxChargeTime * highRange / 100.0f)
            {
                chargeLv_now = 3;
                ChargeBar[2].GetComponent<Image>().color= Color.HSVToRGB(Time.time * 2 % 1, 1, 1);
            }


        }
        else
        {
            for (int i = 0; i < ChargeBar.Length; i++)
            {
                ChargeBar[i].GetComponent<Image>().fillAmount = 0;
                chargeLv_now = 0;
                chargeLv_before = 0;
            }
        }


        CheckLvUp();
    }

   

    void CheckLvUp()
    {
        if (chargeLv_now != chargeLv_before)
        {
            LvUp = true;
        }

        anime.SetBool("LvUp", LvUp);

        chargeLv_before = chargeLv_now;
    }

    public void AnimeEnd()
    {
        LvUp = false;
    }
}
