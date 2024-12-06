using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    [SerializeField]
    float ParameterUpProbability;
    [SerializeField]
    float SkillProbability;

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

    }

    void RandomBonus()
    {
        // 1.カードがどのタイプの能力が出るか決める(決めた確率でランダム)
        // 2.それぞれ乱数で決める(完全ランダム)
        // 3.それぞれのランダムは別に関数作る
        // 4.同じやつは出ないようにする
        // 5.消費能力は後日追加
        //
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


        //Debug.Log("Time.timeScale=" + Time.timeScale);

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

        RandomBonus();

        

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(true);
        }
        //Cursor.SetActive(true);


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

        Time.timeScale = 1;
    }

}
