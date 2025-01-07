using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkillSelect : MonoBehaviour
{
    [SerializeField]
    GameObject[] BonusWindow;

    [SerializeField]
    GameObject Cursor;

    int SelectNo;

    PlayerManager player;

    Animator AnimeCon;
    public bool b1, b2, b3, AnimeStart;


    Vector3 originalSize;

    [SerializeField]
    float ParameterUpProbability;
    [SerializeField]
    float SkillProbability;

    public string selectSE;

    bool IsBonusSelect;

    bool IsAllSkillLvMax;

    List<int> bonustype1 = new List<int>();
    List<int> bonustype2 = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        IsAllSkillLvMax = false;

        IsBonusSelect = false;

        AnimeStart = false;

        b1 = b2 = b3 = false;

        AnimeCon = GetComponent<Animator>();

        player = FindFirstObjectByType<PlayerManager>();
        
        SelectNo = 0;

        RandomBonus();

        for (int i = 0; i < 3; i++)
        {
            BonusWindow[i].GetComponent<SkillWindow>().DrawBonus();
        }

        originalSize = BonusWindow[0].GetComponent<RectTransform>().localScale;

    }

    void RandomBonus()
    {
        for (int i = 0; i < BonusWindow.Count(); i++) 
        {
            BonusWindow[i].GetComponent<SkillWindow>().CardReset();

            BonusRandom(BonusTypeRandom(), i);

            BonusWindow[i].GetComponent<SkillWindow>().DrawBonus();
        }
    }

    bool CheckIsAllSkillLvMax()
    {
        for (int i = 0; i < BonusSettings.Instance.playerBonusItems.Count; i++) 
        {
            if (player.playerPrefabs.CheckItemNotMax(BonusSettings.Instance.playerBonusItems[i]))
            {
                return false;
            }

            if(i== (BonusSettings.Instance.playerBonusItems.Count - 1))
            {
                return true;
            }
        }


        return false;
    }

    int BonusTypeRandom()
    {

        int p, result;

        result = 0;

        p = Random.Range(0, 100);

        if(p < ParameterUpProbability)
        {
            result = 0;
        }
        else
        {
            if (CheckIsAllSkillLvMax())
            {
                result = 0;
            }
            else
            {
                result = 1;
            }
        }

        return result;
    }

    void BonusRandom(int type,int cardNo)
    {
        int i;

        switch (type)
        {
            case 0:
                int c1 = BonusSettings.Instance.playerBonusDatas.Count;

                i = Random.Range(0, c1);

                while (CheckBonusIsUsed(type, i))
                {
                    i = Random.Range(0, c1);
                }
                bonustype1.Add(i);

                BonusWindow[cardNo].GetComponent<SkillWindow>().Bonus = BonusSettings.Instance.playerBonusDatas[i];
                break;
            case 1:
                int c2 = BonusSettings.Instance.playerBonusItems.Count;

                i = Random.Range(0, c2);

                while (!player.playerPrefabs.CheckItemNotMax(BonusSettings.Instance.playerBonusItems[i]) || CheckBonusIsUsed(type, i))  
                {
                    i = Random.Range(0, c2);
                }

                bonustype2.Add(i);

                BonusWindow[cardNo].GetComponent<SkillWindow>().Item = BonusSettings.Instance.playerBonusItems[i];
                break;
            case 2:
                break;
        }

    }

    bool CheckBonusIsUsed(int type,int No)
    {
        switch (type)
        {
            case 0:
                return bonustype1.Contains(No);
                
            case 1:
                return bonustype2.Contains(No);
                
            case 2:
                return false;
                
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        //¶‰EƒXƒLƒ‹‘I‘ð
        Cursor.transform.localPosition = BonusWindow[SelectNo].transform.localPosition;

        AnimeCon.SetBool("Start", AnimeStart);
        AnimeCon.SetBool("Bonus1", b1);
        AnimeCon.SetBool("Bonus2", b2);
        AnimeCon.SetBool("Bonus3", b3);

        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].gameObject.GetComponent<RectTransform>().localScale = originalSize;
            BonusWindow[i].gameObject.GetComponent<Image>().color = Color.white;
        }
        //Cursor.SetActive(true);

        BonusWindow[SelectNo].gameObject.GetComponent<RectTransform>().localScale = originalSize * 1.15f;
        BonusWindow[SelectNo].gameObject.GetComponent<Image>().color = Color.cyan;

        // Debug.Log("Time.timeScale=" + Time.timeScale);


    }
    public void BonusChoose(InputAction.CallbackContext context)
    {
       
        if (!context.started) return;

        SoundManager.Instance.PlaySE(selectSE);

        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.x < 0.3f)   //Left
        {
            SelectNo -= 1;
            if (SelectNo < 0)
            {
                SelectNo = 2;
            }
        }
        if (moveInput.x > 0.3f) //Right
        {
            SelectNo += 1;
            if (SelectNo > 2)
            {
                SelectNo = 0;
            }
        }
    }

    public void BonusSelect(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (IsBonusSelect != true) return;


        if (BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus != null)
        {
            player.GetComponent<PlayerManager>().ApplyBonus(BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus);

        } else if(BonusWindow[SelectNo].GetComponent<SkillWindow>().Item != null)
        {
            player.GetComponent<PlayerManager>().playerPrefabs.GetTopItemBonus(BonusWindow[SelectNo].GetComponent<SkillWindow>().Item);
        }
        //player.GetComponent<PlayerManager>().playerData.
        //player.GetComponent<HoldSkill>().AddPlayerBonusData(BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus);


        switch (SelectNo)
        {
            case 0:
                b1 = true;
                break;
            case 1:
                b2 = true;
                break;
            case 2:
                b3 = true;
                break;
        }


        Cursor.SetActive(false);

        

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");


        bonustype1.Clear();
        bonustype2.Clear();

    }


    public void LevelUpDebug(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        LevelUp();
    }

    public void LevelUp()
    {
        Time.timeScale = 0;

        IsBonusSelect = true;

        AnimeStart = true;
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

        RandomBonus();

        AnimeCon.SetBool("Start", AnimeStart);
        AnimeCon.SetBool("Bonus1", b1);
        AnimeCon.SetBool("Bonus2", b2);
        AnimeCon.SetBool("Bonus3", b3);


        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(true);

            BonusWindow[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(6f, 6f, 1f);
            BonusWindow[i].gameObject.GetComponent<Image>().color = Color.white;
        }
        //Cursor.SetActive(true);


        BonusWindow[SelectNo].gameObject.GetComponent<RectTransform>().localScale = new Vector3(8f, 8f, 1f);
        BonusWindow[SelectNo].gameObject.GetComponent<Image>().color = Color.red;

    }

    public void AnimationReset()
    {
        AnimeStart = false;

        b1 = b2 = b3 = false;

        Debug.Log("false");
    }

    public void BonusScelectEnd()
    {
        AnimeStart = false;

        b1 = b2 = b3 = false;

        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(false);
        }

        IsBonusSelect = false;

        Time.timeScale = 1;
    }

}
