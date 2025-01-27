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
    PlayerInput playerInput;

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

    [ColorUsage(true, true)] public Color selectedColor;
    [ColorUsage(true, true)] private Color originalColor;


    // Start is called before the first frame update
    void Start()
    {
        IsAllSkillLvMax = false;

        IsBonusSelect = false;

        AnimeStart = false;

        b1 = b2 = b3 = false;

        AnimeCon = GetComponent<Animator>();

        player = FindFirstObjectByType<PlayerManager>();

        playerInput = player.GetComponent<PlayerInput>();   

        SelectNo = 0;

        originalColor = BonusWindow[0].GetComponent<Image>().material.GetColor("_Color");

        for (int i = 0; i < 3; i++)
        {
            Image image = BonusWindow[i].GetComponent<Image>();

            image.material = Instantiate(image.material);
        }
        //RandomBonus();

        //for (int i = 0; i < 3; i++)
        //{
        //    BonusWindow[i].GetComponent<SkillWindow>().DrawBonus();
        //}

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

                int max = 20;
                while (CheckBonusIsUsed(type, i) && max > 0)
                {
                    max--;
                    i = Random.Range(0, c1);
                }
                bonustype1.Add(i);

                BonusWindow[cardNo].GetComponent<SkillWindow>().Bonus = BonusSettings.Instance.playerBonusDatas[i];
                break;
            case 1:
                int c2 = BonusSettings.Instance.playerBonusItems.Count;

                i = Random.Range(0, c2);

                int max2 = 20;
                while ((!player.playerPrefabs.CheckItemNotMax(BonusSettings.Instance.playerBonusItems[i]) || CheckBonusIsUsed(type, i)) && max2 > 0)
                {
                    max2--;
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
            //BonusWindow[i].gameObject.GetComponent<Image>().color = Color.white;
            BonusWindow[i].GetComponent<Image>().material.SetColor("_Color", originalColor);
        }
        //Cursor.SetActive(true);

        BonusWindow[SelectNo].gameObject.GetComponent<RectTransform>().localScale = originalSize * 1.15f;
        //BonusWindow[SelectNo].gameObject.GetComponent<Image>().color = Color.cyan;
        BonusWindow[SelectNo].GetComponent<Image>().material.SetColor("_Color", selectedColor);

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
        if (AnimeStart) return;

        SoundManager.Instance.PlaySE("BonusSelect");
        if (BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus != null)
        {
            player.GetComponent<PlayerManager>().ApplyBonus(BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus);

        } else if(BonusWindow[SelectNo].GetComponent<SkillWindow>().Item != null)
        {
            player.GetComponent<PlayerManager>().playerPrefabs.GetTopItemBonus(BonusWindow[SelectNo].GetComponent<SkillWindow>().Item);
        }
        //mainCharacter.GetComponent<PlayerManager>().playerData.
        //mainCharacter.GetComponent<HoldSkill>().AddPlayerBonusData(BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus);


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



        playerInput.SwitchCurrentActionMap("Player");


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
        SoundManager.Instance.PlaySE("LevelUp", 2.5f);

        playerInput.SwitchCurrentActionMap("UI");

        Time.timeScale = 0;

        IsBonusSelect = true;

        AnimeStart = true;
     

        RandomBonus();

        AnimeCon.SetBool("Start", AnimeStart);
        AnimeCon.SetBool("Bonus1", b1);
        AnimeCon.SetBool("Bonus2", b2);
        AnimeCon.SetBool("Bonus3", b3);


        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(true);

            BonusWindow[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(6f, 6f, 1f);
            BonusWindow[i].GetComponent<Image>().material.SetColor("_Color", originalColor);
        }
        //Cursor.SetActive(true);


        BonusWindow[SelectNo].gameObject.GetComponent<RectTransform>().localScale = new Vector3(8f, 8f, 1f);
        BonusWindow[SelectNo].GetComponent<Image>().material.SetColor("_Color", selectedColor);

    }

    public void AnimationReset()
    {
        AnimeStart = false;

        b1 = b2 = b3 = false;

        
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


    public void CheckActionMap()
    {
        StartCoroutine(ChangeActionMapToUI());
    }

    IEnumerator ChangeActionMapToUI()
    {
        yield return null;

        if (playerInput.currentActionMap.name == "Player")
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

}
