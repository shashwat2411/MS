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

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();
        //Time.timeScale = 0;
        SelectNo = 0;

        RandomBonus();

        for (int i = 0; i < 3; i++)
        {
            BonusWindow[i].GetComponent<SkillWindow>().DrawBonus();
        }

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

        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(false);
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

        RandomBonus();

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

        for (int i = 0; i < BonusWindow.Count(); i++)
        {
            BonusWindow[i].SetActive(true);
        }
        Cursor.SetActive(true);

        Time.timeScale = 0f;
    }
}
