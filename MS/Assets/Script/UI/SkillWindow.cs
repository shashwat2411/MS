using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillWindow : MonoBehaviour
{

    public BonusData Bonus;

    public BonusItem Item;

    [SerializeField]
    Image Icon;

    [SerializeField]
    TextMeshProUGUI Name, Description;

    PlayerManager player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();
       
    }

 


    public void DrawBonus()
    {
        

        if (Bonus != null)
        {
            Name.text = Bonus.name;
            Description.text = Bonus.description;
            Icon.sprite = Bonus.icon;

        }

        if (Item != null)
        {
            Name.text = Item.name;
            Description.text = Item.bonusList[GetSkillLv(Item)].description;
            Icon.sprite = Item.icon;


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
