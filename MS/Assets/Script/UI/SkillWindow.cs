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


  
    // Start is called before the first frame update
    void Start()
    {
        
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
            Description.text = Item.description;
            Icon.sprite = Item.icon;
        }
    }

    public void CardReset()
    {
        Bonus = null;
        Item = null;
    }

  
   
}
