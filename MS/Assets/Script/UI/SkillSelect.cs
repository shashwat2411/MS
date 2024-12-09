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

    // Start is called before the first frame update
    void Start()
    {
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
        int b1, b2, b3;
        b1 = b2 = b3 = 0;

        int c = BonusSettings.Instance.playerBonusDatas.Count;

        int i = Random.Range(0, c);

        b1 = i;
        //bonus1Œˆ’è
        i = Random.Range(0, c);

        b2 = i;
        while (b1 == b2)
        {
            i = Random.Range(0, c);

            b2 = i;
        }
        //bonus2Œˆ’è
        i = Random.Range(0, c);

        b3 = i;
        while (b1 == b3 || b2 == b3)
        {
            i = Random.Range(0, c);

            b3 = i;
        }
        //bonus3Œˆ’è

        BonusWindow[0].GetComponent<SkillWindow>().Bonus = BonusSettings.Instance.playerBonusDatas[b1];
        BonusWindow[1].GetComponent<SkillWindow>().Bonus = BonusSettings.Instance.playerBonusDatas[b2];
        BonusWindow[2].GetComponent<SkillWindow>().Bonus = BonusSettings.Instance.playerBonusDatas[b3];
        for (int j = 0; j < 3; j++)
        {
            BonusWindow[j].GetComponent<SkillWindow>().DrawBonus();
        }

        // Bonus = BonusSettings.Instance.bonusDatas[i];
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

        player.GetComponent<PlayerManager>().ApplyBonus(BonusWindow[SelectNo].GetComponent<SkillWindow>().Bonus);

       

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

        Time.timeScale = 1f;
    }


    public void LevelUpDebug(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        LevelUp();
    }

    public void LevelUp()
    {
        Time.timeScale = 0;

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
    }

    public void BonusScelectEnd()
    {
        AnimeStart = false;

        b1 = b2 = b3 = false;

        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(false);
        }
    }

}
