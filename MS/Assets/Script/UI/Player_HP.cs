using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    float Hp_Now;
    float Hp_Max;

    [SerializeField]
    GameObject RedBar, GreenBar;

    // Start is called before the first frame update
    void Start()
    {
        GreenBar.GetComponent<Image>().fillAmount = 1;
        RedBar.GetComponent<Image>().fillAmount = 1;

        Hp_Max = Player.GetComponent<PlayerManager>().playerData.maxHp;
        Hp_Now = Player.GetComponent<PlayerManager>().playerData.hp;
    }

    // Update is called once per frame
    void Update()
    {
        //åªç›ÇÃHPÇéÊìæ
        Hp_Now = Player.GetComponent<PlayerManager>().playerData.hp;

        if (Hp_Now <= 0)
        {
            Hp_Now = 0;
        }
        //HPBarÇÃèàóù
        //GreenBarÇÃèàóù
        GreenBar.GetComponent<Image>().fillAmount = Hp_Now / Hp_Max;

        //ReDBarÇÃèàóù
        if (RedBar.GetComponent<Image>().fillAmount > GreenBar.GetComponent<Image>().fillAmount)
        {
            RedBar.GetComponent<Image>().fillAmount -= 0.005f;
        }

        if(RedBar.GetComponent<Image>().fillAmount <= GreenBar.GetComponent<Image>().fillAmount)
        {
            RedBar.GetComponent<Image>().fillAmount = GreenBar.GetComponent<Image>().fillAmount;
        }

    }
}
