using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeLvUI : MonoBehaviour
{
    bool LvUp;
    bool LvUp2;
    bool LvUp3;

    public int chargeLv_now;
    int chargeLv_before;

    float lowRange;
    float middleRange;
    float highRange;
    public float outlineOffset = 1.2f;

    Animator anime;
    PlayerManager player;


    [SerializeField] private Image[] ChargeBar = new Image[3];
    [SerializeField] private Image[] OutlineBar = new Image[3];

    //ImageÇÃÉJÉâÅ[ÇÕBaseColorÇæÇ©ÇÁHDRÇì¸ÇÍÇƒÇ‡îΩâfÇ≥ÇÍÇ»Ç¢
    [SerializeField] private Color ColorLv1;
    [SerializeField] private Color ColorLv2;
    [SerializeField] private Color ColorLv3;
    [SerializeField] private Color ColorMax;


    private void Start()
    {
        anime = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerManager>();

        chargeLv_now = 0;
        chargeLv_before = 0;

        lowRange = player.playerAttack.lowRange;
        middleRange = player.playerAttack.middleRange;
        highRange = player.playerAttack.highRange;

        Image[] bars = transform.GetComponentsInChildren<Image>();

        OutlineBar[0] = bars[0];
        OutlineBar[1] = bars[1];
        OutlineBar[2] = bars[2];
        ChargeBar[0] = bars[3];
        ChargeBar[1] = bars[4];
        ChargeBar[2] = bars[5];

        for(int i = 0; i < ChargeBar.Length; i++)
        {
            ChargeBar[i].fillAmount = 0;
            OutlineBar[i].fillAmount = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.playerAttack.isHold == true)
        {
            float charge = player.playerData.charge;
            float maxCharge = player.playerData.maxChargeTime;

            float lowRangeCharge = maxCharge * lowRange / 100.0f;
            float middleRangeCharge = maxCharge * middleRange / 100.0f;
            float highRangeCharge = maxCharge * highRange / 100.0f;

            //Level 1
            if (charge <= maxCharge * lowRange / 100.0f) 
            {
                ChargeBar[0].color = ColorLv1;
                ChargeBar[0].fillAmount = charge / lowRangeCharge;

                OutlineBar[0].fillAmount = charge / lowRangeCharge * outlineOffset;

            }
            //Level 2
            if (charge > lowRangeCharge && charge <= middleRangeCharge) 
            {
                chargeLv_now = 1;
                ChargeBar[0].fillAmount = 1.0f;

                ChargeBar[1].color = ColorLv2;
                ChargeBar[1].fillAmount = (charge - lowRangeCharge) / (middleRangeCharge - lowRangeCharge);

                OutlineBar[0].fillAmount = 1.0f;
                OutlineBar[1].fillAmount = (charge - lowRangeCharge) / (middleRangeCharge - lowRangeCharge) * outlineOffset;
            }
            //Level 3
            if (charge <= highRangeCharge && charge > middleRangeCharge)
            {
                chargeLv_now = 2;
                ChargeBar[1].fillAmount = 1.0f;

                ChargeBar[2].color = ColorLv3;
                ChargeBar[2].fillAmount = (charge - middleRangeCharge) / (highRangeCharge - middleRangeCharge);

                OutlineBar[1].fillAmount = 1.0f;
                OutlineBar[2].fillAmount = (charge - middleRangeCharge) / (highRangeCharge - middleRangeCharge) * outlineOffset;
            }
            //Level Max
            if(charge > maxCharge * highRange / 100.0f)
            {
                chargeLv_now = 3;

                for (int i = 0; i < ChargeBar.Length; i++)
                {
                    ChargeBar[i].color = Color.Lerp(ChargeBar[i].color, ColorMax, 0.1f);
                }
            }
        }
        else
        {
            for (int i = 0; i < ChargeBar.Length; i++)
            {
                ChargeBar[i].fillAmount = 0;
                OutlineBar[i].fillAmount = 0;
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
            if (chargeLv_now == 1)
            {
                LvUp = true;
            }
            if (chargeLv_now == 2)
            {
                LvUp2 = true;
            }
            if (chargeLv_now == 3)
            {
                LvUp3 = true;
            }

        }

        anime.SetBool("LvUp", LvUp);
        anime.SetBool("LvUp2", LvUp2);
        anime.SetBool("LvUp3", LvUp3);

        chargeLv_before = chargeLv_now;
    }

    public void AnimeEnd()
    {
        LvUp = false;
        LvUp2 = false;
        LvUp3 = false;
    }
}
