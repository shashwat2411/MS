using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    float Hp, MaxHp, Attack, Defence, exp, Nextexp;
    int Lv;


    PlayerManager player;

    [SerializeField]
    TextMeshProUGUI LV, HP, Atk, Def,EXP;

    Scrollbar scrol;

    int selectionNo;
    [SerializeField]
    GameObject[] Selection;
    [SerializeField]
    GameObject cur;
    [SerializeField]
    TextMeshProUGUI CurText;
    [SerializeField]
    Scrollbar scrollbar;

    bool Rt, Lt;

    string[] Text = new string[2] { "GoToTitle", "OPTION" };

    [SerializeField]
    GameObject[] SkillIcon;

    [SerializeField]
    GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        Rt = false;
        Lt = false;

        scrollbar.value = 0;

        player = FindFirstObjectByType<PlayerManager>();

        selectionNo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DrawPlayerStatus();


        cur.transform.localPosition = Selection[selectionNo].transform.localPosition;
        CurText.text = Text[selectionNo];

        if (Rt == true)
        {
            scrollbar.value += 0.05f;
            if (scrollbar.value >= 1)
            {
                scrollbar.value = 1;
                Rt = false;
            }
        }

        if (Lt == true)
        {
            scrollbar.value -= 0.05f;
            if (scrollbar.value <= 0)
            {
                scrollbar.value = 0;
                Lt = false;
            }
        }

    }

    void DrawPlayerStatus()
    {
        MaxHp = player.GetComponent<PlayerManager>().playerData.maxHp;
        Hp = player.GetComponent<PlayerManager>().playerData.hp;
        Attack = player.GetComponent<PlayerManager>().playerData.attack;
        Defence = player.GetComponent<PlayerManager>().playerData.defence;
        exp = player.GetComponent<PlayerManager>().playerData.exp;
        Nextexp = player.GetComponent<PlayerManager>().playerData.nextExp;
        Lv = player.GetComponent<PlayerManager>().playerData.lv;


        HP.text = "HP:" + Hp.ToString() + "/" + MaxHp.ToString();
        Atk.text = "Attack:" + Attack.ToString();
        Def.text = "Defence:" + Defence.ToString();
        LV.text = "Lv." + Lv.ToString();
        EXP.text = "EXP:" + exp.ToString() + "/" + Nextexp.ToString();

        //---------------------------------------------------------------------

        int count = player.playerPrefabs.itemCountPair.Count;
        Sprite bonusicon = null;
        int bonusLv = 0;

        foreach (KeyValuePair<string, int> item in player.playerPrefabs.itemCountPair)
        {
            //Lv
            bonusLv = item.Value;

            //ÉAÉCÉRÉì
            for(int i=0;i< BonusSettings.Instance.playerBonusItems.Count; i++)
            {
                if(item.Key!= BonusSettings.Instance.playerBonusItems[i].name) { continue; }

                bonusicon = BonusSettings.Instance.playerBonusItems[i].icon;
                break;
            }
            
        }

        for(int a = 0; a < count; a++)
        {
            SkillIcon[a].SetActive(true);
            SkillIcon[a].GetComponent<Image>().sprite = bonusicon;
            SkillIcon[a].transform.GetChild(0).GetComponent<TextMeshPro>().text = "Lv." + bonusLv.ToString();
        }

    }

    public void ParameterChange(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (scrollbar.value == 0)
        {
            Rt = true;
        }
        else
        {
            Lt = true;
        }
        
        
    }

    public void CursorMove(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.x < 0.3f)   //Left
        {
            selectionNo += 1;
            if (selectionNo > 1)
            {
                selectionNo = 0;
            }

        }
        if (moveInput.x > 0.3f) //Right
        {
            selectionNo -= 1;
            if (selectionNo < 0)
            {
                selectionNo = 1;
            }

        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        switch (selectionNo)
        {
            case 0:
                //title
                GameManager.GetComponent<GameManager>().ReturnTitle();
                Time.timeScale = 1;

                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                break;
            case 1:
                //config

                break;
           
        }
    }

  


    void ReturnToTitle()
    {
        GameManager.GetComponent<GameManager>().ReturnTitle();
    }

    void ConfigMenu()
    {

    }

}
