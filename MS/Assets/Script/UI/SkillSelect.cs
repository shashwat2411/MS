using System.Collections;
using System.Collections.Generic;
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

       
    }

    public void BonusMenuAction()
    {

        SelectNo = 0;

        for (int i = 0; i < 3; i++)
        {
            BonusWindow[i].GetComponent<SkillWindow>().DrawBonus();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //左右スキル選択
        Cursor.transform.localPosition = BonusWindow[SelectNo].transform.localPosition;

    }
    public void BonusChoose(InputAction.CallbackContext context)
    {
       
        if (!context.started) return;

        if (context.action.name == "Left")
        {
            SelectNo -= 1;
            if (SelectNo < 0)
            {
                SelectNo = 2;
            }
        }
        if (context.action.name == "Right")
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

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
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

        BonusMenuAction();

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
