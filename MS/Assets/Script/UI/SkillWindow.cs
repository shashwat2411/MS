using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SkillWindow : MonoBehaviour
{

    public BonusData Bonus;

    public BonusItem Item;

    [SerializeField]
    Image Icon;

    [SerializeField] Image card;

    [SerializeField]
    TextMeshProUGUI Name, Description;

   public PlayerManager player;


    private void OnEnable()
    {
        if(player == null)
        {
            player = FindFirstObjectByType<PlayerManager>();
        }

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

    }


    public void DrawBonus()
    {
        

        if (Bonus != null)
        {
            Name.text = Bonus.name;
            Description.text = Bonus.description;
            Icon.sprite = Bonus.icon;
            card.sprite = Bonus.icon;

        }

        if (Item != null)
        {
            Name.text = Item.name;
            Description.text = Item.bonusList[GetSkillLv(Item)].description;
            Icon.sprite = Item.icon;
            card.sprite = Item.icon;


        }

        if (Icon.sprite != null) { Debug.Log(gameObject.name + "-DrawBonus-" + Icon.sprite.name); }
    }

    int GetSkillLv(BonusItem item)
    {
        player = FindFirstObjectByType<PlayerManager>();

        int skilllv = 0;

        if (player.playerPrefabs.itemCountPair.ContainsKey(item.name))
        {
            skilllv = player.playerPrefabs.itemCountPair[item.name];
        }
        else
        {
            skilllv = 0;
        }


        return skilllv;
    }

    public void CardReset()
    {
        Bonus = null;
        Item = null;
    }

  
   
}
